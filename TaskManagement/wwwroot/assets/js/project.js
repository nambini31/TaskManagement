
$(document).ready(function () {
    $('#table_project').DataTable();

    
    $('#createProjectButton').click(function () {
        $('#createProjectModal').modal('show');
    });

    
    $('.editProjectButton').click(function () {
        var id = $(this).data('id');
        $.get('/Project/Edit/' + id, function (data) {
            $('#editProjectModal .modal-body').html(data);
            $('#editProjectModal').modal('show');
        });
    });

    
    $('.deleteProjectButton').click(function () {
        var id = $(this).data('id');
        $.get('/Project/Delete/' + id, function (data) {
            $('#deleteProjectModal .modal-body').html(data);
            $('#deleteProjectModal').modal('show');
        });
    });
});
