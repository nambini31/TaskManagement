$(document).ready(function () {
    $('#table_task').DataTable();

    $('#createTaskButton').click(function () {
        $('#createTaskModal').modal('show');
    });

    $('.editTaskButton').click(function () {
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
                    <button type="submit" class="btn btn-primary">Save</button>
                </form>
            `;
            $('#editTaskModal .modal-body').html(editFormHtml);
            $('#editTaskModal').modal('show');
        });
    });

    $('.deleteTaskButton').click(function () {
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
                    $('#createTaskModal').modal('hide');
                    location.reload();
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
                    $('#editTaskModal').modal('hide');
                    location.reload(); // Recharger la page pour afficher les changements
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
                    $('#deleteTaskModal').modal('hide');
                    location.reload();
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
