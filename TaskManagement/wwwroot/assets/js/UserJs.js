

$(document).ready(function () {
  
    $('#table_user').DataTable({
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
                    return '<button class="btn btn-sm btn-warning btn-edit mr-2" data-id="' + row.userId + '"><i class="fas fa-user-edit"></i></button>' +
                        '<button class="btn btn-sm btn-danger btn-delete" data-id="' + row.userId + '"><i class="fas fa-trash-alt"></i></button>';
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
                    $('#table_user').DataTable().ajax.reload();
                },
            },
            {
                text: '<i class="ti ti-plus ti-xs me-0 me-sm-2"></i><span class="d-none d-sm-inline-block">New User</span>',
                className: 'class="btn btn-info waves-effect waves-light"',
                attr: {
                    'data-toggle' : 'modal',
                    'data-target' : '#modalAdduser'
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
            '>'

    });
});