

$(document).ready(function () {
  
    var table = $('#table_user').DataTable({
        destroy: true,
        ordering: true,
        order: [[0, "desc"]],
        responsive: true,
        info: false,
        paging: true,
        deferRender: true,
        pageLength: 7,

        ajax: {
            url: '/User/GetAllUser', // Remplacez par l'URL de votre API qui retourne la liste des utilisateurs
            type: 'POST', // Méthode HTTP
            dataSrc: '' // Propriété de l'objet JSON qui contient les données (utilisez '' si les données sont à la racine)
        },

        columns: [
            { data: 'userId', title: 'ID' },
            { data: 'name', title: 'Name' },
            { data: 'surname', title: 'Surname' },
            { data: 'username', title: 'Username' }, 
            { data: 'roleName', title: 'Role' }, 
            { data: 'email', title: 'Email' },
            {
                title: 'Actions',
                searchable: false,
                sortable: false,
                render: function (data, type, row) {
                    return `<a class="btn btn-sm btn-primary btn-edit" style="color:white" data-id="${row.userId}"><i class="fe-edit"></i></a>
                        <a class="btn btn-sm btn-danger btn-delete" style="color:white" data-id="${row.userId}"><i class="fas fa-trash"></i></a>`;
                }
            }
        ],

        "initComplete": function (settings, json) {
            $('div.dataTables_wrapper div.dataTables_filter input').attr('placeholder', 'Recherche').attr('class', 'form-control');
        },
        language: {
            "search": "",
            "zeroRecords": "Aucun enregistrement",
            paginate: {
                previous: "Previous",
                next: "Next",
            },
        }
        ,
        buttons: [
            {
                className: "btn btn-sm btn-primary btn-min-width ml-20",
                text: '<i class="ft-refresh"> Actualiser</i>',
                action: function () {
                    table.ajax.reload();
                },
            },
            {
                text: '<i class="ti ti-plus ti-xs me-0 me-sm-2"></i><span class="d-none d-sm-inline-block">New User</span>',
                className: 'class="btn btn-info waves-effect waves-light"',
                attr: {
                    'data-toggle' : 'modal',
                    'data-target': '#modalAddUser',
                    'id': 'btnAddUser'
                }
            }

            
        ],


        dom:
            '<"card-header d-flex flex-wrap pb-2"' +
            '<f>' +
            '<"d-flex justify-content-center justify-content-md-end align-items-baseline"' +
            '<"dt-action-buttons d-flex justify-content-center flex-md-row mb-3 mb-md-0 ps-1 ms-1 align-items-baseline ml-3"l><"btn-group ml-3"B>' + // Ajout de la classe ml-3 (marge-left: 3) pour ajouter un espace de 3 unités Bootstrap
            '>' +
            '>t' +
            '<"row mx-2"' +
            '<"col-sm-12 col-md-6"i>' +
            '<"col-sm-12 col-md-6"p>' +
            '>',
        columnDefs: [
            {
                targets: -1, // Dernière colonne (Actions)
                width: "120px", // Largeur fixe pour les boutons
                className: 'text-center', // Centrage du contenu
                orderable: false, // Désactivation du tri sur cette colonne
            }
        ]
    });

    const userInput = document.getElementById('Email');
    const inputError = document.getElementById('inputError');

    const userInputUsername = document.getElementById('Username'); 
    const inputErrorUsername = document.getElementById('inputErrorUsername');

    //-- gestion de la soumission du formulaire Creat / Update
    $('#userForm').on('submit', function (event) {
        event.preventDefault();
        var formData = $(this).serialize();
        var actionUrl = $(this).attr('action');

        //verif username validation
        var inputValueUsername = userInputUsername.value.trim();
        ValidationUsername(inputValueUsername);
        
        //verif email validation
        var inputValue = userInput.value.trim();
        if (inputValue != '') {
            ValidationEmail(userInput);
        } else {
            inputError.textContent = '';
        }

        $.ajax({
            url: actionUrl,
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    $('#userForm')[0].reset();
                    $('#modalAddUser').modal('hide');
                    $('#UserId').prop('disabled', false);
                    alert(response.message);
                    table.ajax.reload();
                } else {
                    if (response.erreurValidation) {
                        // Effacer les erreurs précédentes spécifiques
                        $('#userForm span.text-danger').each(function () {
                            $(this).html('');
                        });

                        // Afficher les erreurs de validation
                        var errors = response.erreurValidation;
                        for (var key in errors) {
                            if (errors.hasOwnProperty(key)) {
                                var errorMessages = errors[key];
                                var errorElement = $('span[data-valmsg-for="' + key + '"]');
                                for (var i = 0; i < errorMessages.length; i++) {
                                    var errorMessage = $('<span class="text-danger"></span>').text(errorMessages[i]);
                                    errorElement.append(errorMessage);
                                }
                            }
                        }
                    }
                    if (response.message) {
                        alert(response.message);
                    }
                }
            },
            error: function (xhr, status, error) {
                alert("Sorry !! Server Error")
            }
        });
    });
    //---------------------------------------------

    //--- Ouvir le modal pour l'ajout d'utilisateur
    $('#btnAddUser').on('click', function () {
        $('#userForm').attr('action', '/User/Register');
        $('#userForm')[0].reset();
        $('.text-danger').html('');
        $('#UserId').prop('disabled', true);
        $('#userModalLabel').text('Create User');
    });
    //----------------------------------------------

    //--- Ouvir le model pour l'edit d'un utilisateur
    $('#table_user').on('click', '.btn-edit', function () {
        var userId = $(this).data('id');
        $.post('/User/GetUserById', { id: userId }, function (user) {
            $('#userForm').attr('action', '/User/Update');
            $('#userForm')[0].reset();
            $('.text-danger').html('');
            $('#userModalLabel').text('Edit user');
            $('#UserId').prop('disabled', false);
            $('#userForm').find('input[name="UserId"]').val(user.userId);
            $('#userForm').find('input[name="Name"]').val(user.name);
            $('#userForm').find('input[name="Surname"]').val(user.surname);
            $('#userForm').find('input[name="Username"]').val(user.username);
            $('#userForm').find('input[name="Email"]').val(user.email);
            $('#userForm').find('select[name="Role"]').val(user.roleName);
            $('#modalAddUser').modal('show');
            
        });
    });
    //------------------------------------------------------

    //-- ouvrir modal Confirm delete 
    $('#table_user').on('click', '.btn-delete', function () {
        var userId = $(this).data('id');
        $('#UserIdToDelete').val(userId);
        $('#modalConfirmDelete').modal('show');
    });
    //-------------------------------------

    //-- Soumission de la du formulaire de suppression
    $('#userFormDelete').on('submit', function (event) {
        event.preventDefault();
        var formData = $(this).serialize();
        var actionUrl = $(this).attr('action');

        $.ajax({
            url: actionUrl,
            type: 'POST',
            data: formData,
            success: function (response) {
                $('#modalConfirmDelete').modal('hide');

                if (response.success) {
                    alert(response.message);
                    table.ajax.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr, status, error) {
                $('#modalConfirmDelete').modal('hide');
                alert('Erreur lors de la suppression de l\'utilisateur.');
            }
        });
    });
    //--------------------------------------------------

    //-- Visibilité password --
    const togglePassword = document.getElementById('togglePassword');
    const passwordInput = document.querySelector('.credit-card-mask');

    togglePassword.addEventListener('click', function () {
        const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordInput.setAttribute('type', type);
        this.querySelector('i').classList.toggle('fa-eye');
        this.querySelector('i').classList.toggle('fa-eye-slash');
    });
    //-------------------------------------

    //-- validation e-mail ---
    function ValidationEmail(inputValue) {
        const userInput = document.getElementById('Email');
        var inputValue = userInput.value.trim();
        // Expression régulière pour valider l'input (sans espaces, commence par une lettre, au moins 5 caractères)
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+\w+$/;
        //valide
        if (regex.test(inputValue)) {
            inputError.textContent = '';
        } else {
            inputError.textContent = 'Adress Email Invalid';
            retutn
        }
    }
    //-----------------------------------

    //-- Validation Username --
    function ValidationUsername(inputValue) {
        
        const regex = /^[a-zA-Z][a-zA-Z0-9]{4,}$/;
        //valide
        if (regex.test(inputValue)) {
            inputErrorUsername.textContent = '';
        } else {
            inputErrorUsername.textContent = 'Username that contains at least 5 characters, starts with a letter, and contains no spaces or special characters';
            retutn
        }
    }
    //-----------------------------

});