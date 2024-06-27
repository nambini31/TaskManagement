$(document).ready(function () {
    //$('#table_task').DataTable();
    AfficheTasks();

    $('#createTaskButton').click(function () {
        $('#createTaskModal').modal('show');
    });

   
    $(document).on('click', '.editTaskButton', function () {
        var taskId = $(this).data('id');
        $.get('/Tasks/Get/' + taskId, function (data) {
            var editFormHtml = `
                <form id="editTaskForm">
                    <input type="hidden" id="taskId" name="taskId" value="${data.taskId}" />
                    <div class="form-group">
                        <label for="Title">Title</label>
                        <input type="text" class="form-control" id="TitleForm" name="Title" value="${data.name}" required />
                    </div>
                    <div class="form-group">
                        <label for="ProjectId">Project</label>
                        <select class="form-control" id="ProjectIdForm" name="ProjectId" required>
                            ${data.projects.map(project => `<option value="${project.projectId}" ${project.projectId == data.projectId ? "selected" : ""}>${project.name}</option>`).join('')}
                        </select>
                    </div>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button type="submit" class="btn btn-primary">Save</button>
                </form>
            `;
            $('#editTaskModal .modal-body').html(editFormHtml);
            $('#editTaskModal').modal('show');
        });
    }); 

    $(document).on('click', '.deleteTaskButton', function () {
        var taskId = $(this).data('id');
        $.get('/Tasks/Get/' + taskId, function (data) {
            var deleteFormHtml = `
                <p>Are you sure you want to delete this task?</p>
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                <button type="button" id="deleteTaskConfirmButton" class="btn btn-danger" data-id="${data.taskId}">Delete</button>
            `;
            $('#deleteTaskModal .modal-body').html(deleteFormHtml);
            $('#deleteTaskModal').modal('show');
        });
    });

    $('#createTaskForm').submit(function (e) {
        e.preventDefault();
        var formData = {
            name: $('#Title').val(),
            projectId: $('#ProjectId').val()
        };

        $.ajax({
            url: '/Tasks/Create',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (data) {
                if (data.success) {
                    toastr["success"]("Successfuly !!");
                    $('#createTaskModal').modal('hide');
                    AfficheTasks();
                } else {
                    alert('Error while creating task: ' + data.message);
                }
            },
            error: function (xhr, status, error) {
                alert('An error occurred: ' + error);
            }
        });
    });

    $(document).on('submit', '#editTaskForm', function (e) {
        e.preventDefault();

        var formData = {
            taskId: $('#taskId').val(),
            name: $('#TitleForm').val(),
            projectId: $('#ProjectIdForm').val(),
            projectName: $('#ProjectIdForm option:selected').text() // Si nécessaire
        };

        $.ajax({
            url: '/Tasks/Edit',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (data) {
                if (data.success) {
                    toastr["success"]("Successfuly !!");
                    $('#editTaskModal').modal('hide');
                    AfficheTasks(); // Recharger la page pour afficher les changements
                } else {
                    alert('Error while updating task: ' + data.message);
                }
            },
            error: function (xhr, status, error) {
                alert('An error occurred: ' + error);
            }
        });
    });

    $(document).on('click', '#deleteTaskConfirmButton', function () {
        var taskId = $(this).data('id');
        $.ajax({
            url: '/Tasks/Delete/' + taskId,
            type: 'POST',
            success: function (data) {
                if (data.success) {
                    toastr["success"]("Successfuly !!");
                    $('#deleteTaskModal').modal('hide');
                    AfficheTasks();
                } else {
                    alert('Error while deleting task: ' + data.message);
                }
            },
            error: function (xhr, status, error) {
                alert('An error occurred: ' + error);
            }
        });
    });
});

function AfficheTasks() {

    $('#table_task').DataTable({
        ajax: {
            url: '/Tasks/GetAllTasks',
            type: 'GET',
            dataType: "JSON",
            dataSrc: function (json) {
                return json.data;
            }
        },
        columns: [
            { data: 'taskId', title: '#' }, 
            { data: 'name', title: 'Name' },
            { data: 'projectName', title: 'Project' },
            {
                data: null,
                title: 'Action',
                render: function (data, type, row) {
                    return `
                                <a class="btn btn-sm btn-primary editTaskButton" style="color:white"

                                id="tasks_${row.taskId}"
                                data-id="${row.taskId}"
                                data-name="${row.name}"
                                ><i class="fe-edit"></i></a>

                                <a class="btn btn-sm btn-danger deleteTaskButton" data-id="${row.taskId}" style="color:white"
                                data-toggle="modal" >
                                <i class="fas fa-trash"></i></a>
                            `;
                },
                orderable: false,
                searchable: false
            }

        ],
        destroy: true,
        ordering: true,
        order: [[0, "desc"]],
        "lengthChange": false,
        "paging": true,
        "info": false,
        "filter": true,
        pageLength: 7,
        "initComplete": function (settings, json) {
            $('div.dataTables_wrapper div.dataTables_filter input')
                .attr('placeholder', 'Recherche')
                .attr('class', 'form-control');

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
                text: '<i class="ti ti-plus ti-xs me-0 me-sm-2"></i><span class="d-none d-sm-inline-block">Add</span>',
                className: 'add-new btn btn-info ms-2',
                attr: {
                    'data-toggle': 'modal',
                    'data-target': '#createTaskModal',
                    'id': "createTaskButton"
                }
            }


        ],


        dom:
            '<"card-header d-flex flex-wrap pb-2"' +
            '<f>' +
            '<"d-flex justify-content-center justify-content-md-end align-items-baseline"<"dt-action-buttons d-flex justify-content-center flex-md-row mb-3 mb-md-0 ps-1 ms-1 align-items-baseline"lB>>' +
            '>t' +
            '<"row mx-2"' +
            '<"col-sm-12 col-md-6"i>' +
            '<"col-sm-12 col-md-6"p>' +
            '>',

    });

}
