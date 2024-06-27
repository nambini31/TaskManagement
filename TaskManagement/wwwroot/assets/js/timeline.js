$(document).ready(function () {
    // Initialisation des select pickers

    var taskRowIdCounter = 2;

    // Récupération des projets au chargement de la page
    getProject($('.task-container').find('.task-row1'));

    // Formatage de l'input prix
    formatPrixInput();

    $('#checkleave').change(function () {
        if ($(this).is(':checked')) {
            getLeaves();
        } else {
            getProject();
        }
    });

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

    function getLeaves(rowtask) {
        var newRowSelect = rowtask.find('.selectProjectIdCreate');

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
                var newRowTask = rowtask.find('.selectTaskId');
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

    function getTasks(rowtask, projectId) {
        var newRowTask = rowtask.find('.selectTaskId');

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

    function getProject(rowtask) {
        var newRowSelect = rowtask.find('.selectProjectIdCreate');
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
                var newRowTask = rowtask.find('.selectTaskId');
                newRowTask.attr('required', "required");
                newRowTask.prop('disabled', false);
                $('#labelleaveproject').text("Project");
                newRowSelect.on('change', function () {
                    getTasks(rowtask, newRowSelect.val());
                });
                newRowSelect.selectpicker("refresh");

                if (newRowSelect.find('option').length > 0) {
                    
                    newRowSelect.val(newRowSelect.find('option:first').val());

                    getTasks(rowtask, newRowSelect.val());
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

        var newRow = $(`<div class="task-row${taskRowIdCounter} row align-items-end mb-3">` +
                            '<div class="col-12 col-md-1 mt-3">' +
                                '<input asp-for="isLeave" type="checkbox" class="checkleave" />' +
                            '</div>' +
                            '<div class="col-12 col-md-3 mt-3">' +
                                '<select placeholder="Select project" name="projectId" data-search="true" data-silent-initial-value-set="true" class="form-control w-100 selectProjectIdCreate"></select>' +
                            '</div>' +
                            '<div class="col-12 col-md-4 mt-3">' +
                                '<select placeholder="Select task" name="taskId" data-search="true" data-silent-initial-value-set="true" class="form-control w-100 selectTaskId"></select>' +
                            '</div>' +
                            '<div class="col-12 col-md-1 mt-3">' +
                                '<input asp-for="hours" type="text" class="form-control hours-input" />' +
                            '</div>' +
                            '<div class="col-12 col-md-1 mt-3 btn-container">' +
                                '<button type="button" class="btn btn-delete btn-sm mt-auto">' +
                                '<i class="fa fa-trash"></i>' +
                                '</button>' +
                            '</div>' +
            '</div>');
        $('.task-container').append(newRow);

        var container = $('.task-container').find(`.task-row${taskRowIdCounter}`);


        container.find(`.checkleave`) .change(function () {
            if ($(this).is(':checked')) {
                getLeaves(container);
            } else {
                getProject(container);
            }
        });

        // Initialisation des select pickers pour la nouvelle ligne ajoutée
        getProject(container);

        taskRowIdCounter++;
    });

    // Initialize the delete button to remove rows
    $(document).on('click', '.btn-delete', function () {
        $(`.task-row${taskRowIdCounter}`).remove();
    });

    // Check the number of task rows and toggle visibility of the date input and save button
    function checkTaskRows() {
        if ($(`.task-container .task-row${taskRowIdCounter}`).length === 0) {
            $('#date').closest('.form-group').hide();
            $('button[type="submit"]').hide();
        } else {
            $('#date').closest('.form-group').show();
            $('button[type="submit"]').show();
        }
    }

    // Initial check
    checkTaskRows();

    // Fonction pour initialiser les select pickers dans une nouvelle ligne

    // Fonction pour récupérer les projets pour une nouvelle ligne
    

});