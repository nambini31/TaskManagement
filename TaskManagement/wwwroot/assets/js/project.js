$(document).ready(function () {
    
    $('#table_project').DataTable({
        responsive: true,
        autoWidth: false,
        columnDefs: [
            { orderable: false, targets: -1 }
        ]
    });

   
    $('#createProjectButton').click(function () {
        $('#createProjectModal').modal('show');
    });

   
    $(document).on('click', '.editProjectButton', function () {
        var id = $(this).data('id');
        $.get('/Project/Edit/' + id, function (data) {
            $('#editProjectModal .modal-body').html(data);
            $('#editProjectModal').modal('show');
        });
    });


    $(document).on('click', '.deleteProjectButton', function () {
        var id = $(this).data('id');
        $.get('/Project/Delete/' + id, function (data) {
            $('#deleteProjectModal .modal-body').html(data);
            $('#deleteProjectModal').modal('show');
        });
    });

 
    $('#createProjectForm').submit(function (event) {
        event.preventDefault();
        $.ajax({
            url: '/Project/Create',
            type: 'POST',
            data: $(this).serialize(),
            success: function () {
                $('#createProjectModal').modal('hide');
                location.reload();
            },
            error: function (xhr, status, error) {
                alert('Error: ' + error);
            }
        });
    });

    
    $('#editProjectModal').on('submit', '#editProjectForm', function (event) {
        event.preventDefault();
        $.ajax({
            url: '/Project/EditPost',
            type: 'POST',
            data: $(this).serialize(),
            success: function () {
                $('#editProjectModal').modal('hide');
                location.reload();
            },
            error: function (xhr, status, error) {
                alert('Error: ' + error);
            }
        });
    });

    
    // Handle delete project confirmation
    $('#deleteProjectModal').on('click', '#confirmDeleteButton', function () {
        var id = $("#ProjectId").val(); 
        $.ajax({
            url: '/Project/DeleteConfirmed/' + id,
            type: 'POST',
            success: function () {
                $('#deleteProjectModal').modal('hide');
                location.reload();
            },
            error: function (xhr, status, error) {
                alert('Error: ' + error);
            }
        });
    });
});

