$(document).ready(function () {
    $('#table_task').DataTable();

    $('#createTaskButton').click(function () {
        $('#createTaskModal').modal('show');
    });

    $('.editTaskButton').click(function () {
        var taskId = $(this).data('id');
        // Charger les données de la tâche par AJAX
        $.get('/Tasks/Get/' + taskId, function (data) {
            $('#editTaskForm #taskId').val(data.taskId);
            $('#editTaskForm #Title').val(data.name);
            $('#editTaskForm #ProjectId').val(data.projectId);
            $('#editTaskModal').modal('show');
        });
    });

    $('.deleteTaskButton').click(function () {
        var taskId = $(this).data('id');
        // Charger les données de la tâche par AJAX
        $.get('/Tasks/Get/' + taskId, function (data) {
            $('#deleteTaskModal .modal-body').html('<p>Are you sure you want to delete this task?</p><p><strong>Title:</strong> ' + data.name + '<br /><strong>Project:</strong> ' + data.project.name + '</p>');
            $('#deleteTaskConfirmButton').data('id', taskId);
            $('#deleteTaskModal').modal('show');
        });
    });

    $('#createTaskForm').submit(function (e) {
        e.preventDefault();
        $.post('/Tasks/Create', $(this).serialize(), function (data) {
            location.reload();
        });
    });

    $('#editTaskForm').submit(function (e) {
        e.preventDefault();
        $.post('/Tasks/Edit', $(this).serialize(), function (data) {
            location.reload();
        });
    });

    $('#deleteTaskConfirmButton').click(function () {
        var taskId = $(this).data('id');
        $.post('/Tasks/Delete/' + taskId, function (data) {
            location.reload();
        });
    });
});
