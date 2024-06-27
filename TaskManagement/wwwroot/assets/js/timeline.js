$(document).ready(function () {
    // Initialisation des select pickers

    let taskRowIdCounter = 2;

    // Récupération des projets au chargement de la page
    getProject(1);

    $(`.checkleave1`).change(function () {

        if ($(this).is(':checked')) {

            var rowId = $(this).closest('[class^="task-row"]').data('row-id');
            getLeaves(rowId);

        } else {
            var rowId = $(this).closest('[class^="task-row"]').data('row-id');
            getProject(rowId);

        }
    });

    // Formatage de l'input prix
    formatPrixInput();

    

    function formatPrixInput() {
        var inputPrix = $("#hoursEditUsrTask");
        inputPrix.each(function () {
            new Cleave(this, {
                numeral: true,
                numeralDecimalMark: '.',
                numeralDecimalScale: 2,
                numeralPositiveOnly: true,
                numeralThousandsGroupStyle: 'thousand',
                delimiter: '',
                numeralIntegerScale: 4,
            });
        });
    }

    function getLeaves(rowId) {
        
        var newRowSelect = $('#selectProjectIdCreate' + rowId);
        var newRowTask = $('#selectTaskId' + rowId);

        newRowSelect.selectpicker({
            liveSearch: true,
            actionsBox: true
        });

        $.ajax({
            url: '/Leaves/GetAllLeaves',
            type: 'GET',
            dataType: "JSON",
            success: function (res) {
                newRowSelect.empty();
                res.data.forEach(function (item) {
                    var option = $('<option>', {
                        value: item.leaveId,
                        text: item.reason
                    });
                    newRowSelect.append(option);
                });
                newRowSelect.attr('name', "leaveId");
                newRowSelect.removeAttr("required");
               
                newRowTask.empty();
                $('#labelleaveproject').text("Leave");
                newRowTask.prop('disabled', true);
                newRowTask.selectpicker("refresh");
                newRowSelect.selectpicker("refresh");
            },
            error: function (xhr, status, error) {
                console.error('Error fetching leaves:', error);
            }
        });
    }

    function getTasks(rowId, projectId) {
        var newRowTask = $('#selectTaskId' + rowId);
         newRowTask.selectpicker({
            liveSearch: true,
            actionsBox: true
         });

        $.ajax({
            url: '/Tasks/GetTaskByIdProject',
            type: 'POST',
            dataType: "JSON",
            data: { projectId: projectId },
            success: function (res) {
                newRowTask.empty();
                res.forEach(function (item) {
                    var option = $('<option>', {
                        value: item.taskId,
                        text: item.name
                    });
                    newRowTask.append(option);
                });
                newRowTask.selectpicker("refresh");
            },
            error: function (xhr, status, error) {
                console.error('Error fetching tasks:', error);
            }
        });
    }

    function getProject(rowId) {


        var newRowSelect = $('#selectProjectIdCreate' + rowId);
        var newRowTask = $('#selectTaskId' + rowId);

        newRowSelect.selectpicker({
            liveSearch: true,
            actionsBox: true
        });
        $.ajax({
            url: '/Project/GetAllProjects',
            type: 'GET',
            dataType: "JSON",
            success: function (res) {

                newRowSelect.empty();
                res.data.result.forEach(function (item) {
                    var option = $('<option>', {
                        value: item.projectId,
                        text: item.name
                    });
                    newRowSelect.append(option);
                });
                newRowSelect.attr('name', "projectId");
                newRowSelect.attr('required', "required");
                newRowTask.attr('required', "required");
                newRowTask.prop('disabled', false);
                $('#labelleaveproject').text("Project");
                newRowSelect.on('change', function () {
                    getTasks(rowId, newRowSelect.val());
                });
                newRowSelect.selectpicker("refresh");

                if (newRowSelect.find('option').length > 0) {
                    
                    newRowSelect.val(newRowSelect.find('option:first').val());

                    getTasks(rowId, newRowSelect.val());
                    newRowSelect.selectpicker('refresh');
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching projects:', error);
            }
        });
    }

    // Gestion de la soumission du formulaire avec AJAX
    //$('#formUserTaskHome').submit(function (e) {
    //    e.preventDefault();
    //    var formData = {
    //        date: $('input[asp-for="date"]').val(),
    //        isLeave: $('input[asp-for="isLeave"]').is(':checked'),
    //        projectId: $('.task-container').find('.task-row:last').find('.selectProjectIdCreate').val(),
    //        taskId: $('.task-container').find('.task-row:last').find('.selectTaskId').val(),
    //        hours: $('.task-container').find('.task-row:last').find('input[asp-for="hours"]').val()
    //    };

    //    $.ajax({
    //        url: '/UserTask/Create',
    //        type: 'POST',
    //        contentType: 'application/json',
    //        data: JSON.stringify(formData),
    //        success: function (data) {
    //            if (data.success) {
    //                alert('Task created successfully');
    //                location.reload();
    //            } else {
    //                alert('Error while creating task: ' + data.error);
    //            }
    //        },
    //        error: function (xhr, status, error) {
    //            console.error('An error occurred:', error);
    //            alert('An error occurred: ' + error);
    //        }
    //    });
    //});

    $('.btn-plus-small').click(function () {

        var newRow = $(`<div class="task-row${taskRowIdCounter} row align-items-end mb-3"  data-row-id="${taskRowIdCounter}">` +
                            '<div class="col-12 col-md-1 mt-3">' +
            `<input asp-for="isLeave" type="checkbox" class="checkleave${taskRowIdCounter}" />` +
                            '</div>' +
                            '<div class="col-12 col-md-3 mt-3">' +
            `<select placeholder="Selectpicker project" name="projectId" data-search="true" data-silent-initial-value-set="true" id="selectProjectIdCreate${taskRowIdCounter}" class="form-control w-100 selectProjectIdCreate${taskRowIdCounter}"></select>` +
                            '</div>' +
                            '<div class="col-12 col-md-4 mt-3">' +
            `<select placeholder="Selectpicker task" name="taskId" data-search="true" data-silent-initial-value-set="true" id="selectTaskId${taskRowIdCounter}" class="form-control w-100 selectTaskId${taskRowIdCounter}"></select>` +
                            '</div>' +
                            '<div class="col-12 col-md-1 mt-3">' +
                                '<input asp-for="hours" type="text" class="form-control hours-input" />' +
                            '</div>' +
                            '<div class="col-12 col-md-1 mt-3 btn-container">' +
                                `<button type="button" class="btn btn-delete btn-sm mt-auto">` +
                                '<i class="fa fa-trash"></i>' +
                                '</button>' +
                            '</div>' +
            '</div>');
        $('.task-container').append(newRow);

        var container = $('.task-container').find(`.task-row${taskRowIdCounter}`);


        container.find(`.checkleave${taskRowIdCounter}`).change(function () {

            if ($(this).is(':checked')) {

                var rowId = $(this).closest('[class^="task-row"]').data('row-id');
                getLeaves(rowId);

            } else {
                var rowId = $(this).closest('[class^="task-row"]').data('row-id');
                getProject(rowId);

            }
        });

        // Initialisation des select pickers pour la nouvelle ligne ajoutée
        getProject(taskRowIdCounter);

        taskRowIdCounter++;
    });

    // Initialize the delete button to remove rows
    $(document).on('click', '.btn-delete', function () {
        var rowId = $(this).closest('[class^="task-row"]').data('row-id'); // Récupère l'ID de la ligne à supprimer
        $('.task-container').find('[data-row-id="' + rowId + '"]').remove(); // Supprime l'élément parent avec classe et data-row-id
        // Ajoutez ici d'autres logiques après la suppression si nécessaire
    });

    //$(document).on('click', '.btn-delete', function () {
    //    $(this).closest(`.task-row${taskRowIdCounter}`).remove();
    //    checkTaskRows();
    //});

    // Check the number of task rows and toggle visibility of the date input and save button
    //function checkTaskRows() {
    //    if ($(`.task-container .task-row${taskRowIdCounter}`).length === 0) {
    //        $('#date').closest('.form-group').hide();
    //        $('button[type="submit"]').hide();
    //    } else {
    //        $('#date').closest('.form-group').show();
    //        $('button[type="submit"]').show();
    //    }
    //}

    // Initial check

    // Fonction pour initialiser les select pickers dans une nouvelle ligne

    // Fonction pour récupérer les projets pour une nouvelle ligne
    

});