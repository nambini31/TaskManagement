$(document).ready(function () {
    // Initialisation des select pickers
    $("#selectTaskId").selectpicker({
        liveSearch: true,
        actionsBox: true
    });

    $("#selectProjectIdCreate").selectpicker({
        liveSearch: true,
        actionsBox: true
    });

    // Récupération des projets au chargement de la page
    getProject();

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

    function getLeaves() {
        $.ajax({
            url: '/Leaves/GetAllLeaves',
            type: 'GET',
            dataType: "JSON",
            success: function (res) {
                $("#selectProjectIdCreate").empty();
                res.data.forEach(function (item) {
                    var option = $('<option>', {
                        value: item.leaveId,
                        text: item.reason
                    });
                    $("#selectProjectIdCreate").append(option);
                });
                $('#selectProjectIdCreate').attr('name', "leaveId");
                $('#selectProjectIdCreate').removeAttr("required");
                $('#selectTaskId').removeAttr("required");
                $("#selectTaskId").empty();
                $('#labelleaveproject').text("Leave");
                $('#selectTaskId').prop('disabled', true);
                $("#selectTaskId").selectpicker("refresh");
                $("#selectProjectIdCreate").selectpicker("refresh");
            },
            error: function (xhr, status, error) {
                console.error('Error fetching leaves:', error);
            }
        });
    }

    function getTasks(projectId) {
        $.ajax({
            url: '/Tasks/GetTaskByIdProject',
            type: 'POST',
            dataType: "JSON",
            data: { projectId: projectId },
            success: function (res) {
                $("#selectTaskId").empty();
                res.forEach(function (item) {
                    var option = $('<option>', {
                        value: item.taskId,
                        text: item.name
                    });
                    $("#selectTaskId").append(option);
                });
                $("#selectTaskId").selectpicker("refresh");
            },
            error: function (xhr, status, error) {
                console.error('Error fetching tasks:', error);
            }
        });
    }

    function getProject() {
        $.ajax({
            url: '/Project/GetAllProjects',
            type: 'GET',
            dataType: "JSON",
            success: function (res) {
                $("#selectProjectIdCreate").empty();
                res.data.result.forEach(function (item) {
                    var option = $('<option>', {
                        value: item.projectId,
                        text: item.name
                    });
                    $("#selectProjectIdCreate").append(option);
                });
                $('#selectProjectIdCreate').attr('name', "projectId");
                $('#selectProjectIdCreate').attr('required', "required");
                $('#selectTaskId').attr('required', "required");
                $('#selectTaskId').prop('disabled', false);
                $('#labelleaveproject').text("Project");
                $("#selectTaskId").selectpicker("refresh");
                $('#selectProjectIdCreate').on('change', function () {
                    getTasks($(this).val());
                });
                $("#selectProjectIdCreate").selectpicker("refresh");
                if ($("#selectProjectIdCreate").find('option').length > 0) {
                    $("#selectProjectIdCreate").val($("#selectProjectIdCreate").find('option:first').val());
                    getTasks($("#selectProjectIdCreate").find('option:first').val());
                    $("#selectProjectIdCreate").selectpicker('refresh');
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching projects:', error);
            }
        });
    }

    // Gestion de la soumission du formulaire avec AJAX
    $('#formUserTaskHome').submit(function (e) {
        e.preventDefault();
        var formData = {
            date: $('input[asp-for="date"]').val(),
            isLeave: $('input[asp-for="isLeave"]').is(':checked'),
            projectId: $('#selectProjectIdCreate').val(),
            taskId: $('#selectTaskId').val(),
            hours: $('input[asp-for="hours"]').val()
        };

        $.ajax({
            url: '/UserTask/Create',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (data) {
                if (data.success) {
                    alert('Task created successfully');
                    location.reload();
                } else {
                    alert('Error while creating task: ' + data.error);
                }
            },
            error: function (xhr, status, error) {
                console.error('An error occurred:', error);
                alert('An error occurred: ' + error);
            }
        });
    });

    $('.btn-plus-small').click(function () {
        var newRow = $('<div class="task-row row align-items-end mb-3">' +
            '<div class="col-12 col-md-3">' +
            '<input asp-for="isLeave" type="checkbox" id="checkleave" />' +
            '</div>' +
            '<div class="col-12 col-md-3">' +
            '<select placeholder="Select project" name="projectId" data-search="true" data-silent-initial-value-set="true" class="form-control w-100" id="selectProjectIdCreate"></select>' +
            '</div>' +
            '<div class="col-12 col-md-3">' +
            '<select placeholder="Select task" name="taskId" data-search="true" data-silent-initial-value-set="true" class="form-control w-100" id="selectTaskId"></select>' +
            '</div>' +
            '<div class="col-12 col-md-2">' +
            '<input asp-for="hours" type="text" class="form-control hours-input" />' +
            '</div>' +
            '<div class="col-12 col-md-1 btn-container">' +
            '<button type="button" class="btn btn-delete btn-sm mt-auto">' +
            '<i class="fa fa-trash"></i>' +
            '</button>' +
            '</div>' +
            '</div>');
        $('.task-container').append(newRow);
        checkTaskRows();
    });

    // Initialize the delete button to remove rows
    $(document).on('click', '.btn-delete', function () {
        $(this).closest('.task-row').remove();
        checkTaskRows();
    });

    // Check the number of task rows and toggle visibility of the date input and save button
    function checkTaskRows() {
        if ($('.task-container .task-row').length === 0) {
            $('#date').closest('.form-group').hide();
            $('button[type="submit"]').hide();
        } else {
            $('#date').closest('.form-group').show();
            $('button[type="submit"]').show();
        }
    }

    // Initial check
    checkTaskRows();
});
    
