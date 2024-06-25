$(document).ready(function () {
    // Initialize DataTable
    $('#table_leave').DataTable({
        responsive: true,
        autoWidth: false,
        columnDefs: [
            { orderable: false, targets: -1 }
        ]
    });

    // Show create modal
    $('#createLeaveButton').click(function () {
        $('#createLeaveModal').modal('show');
    });

    // Show edit modal with data
    $('.editLeaveButton').click(function () {
        var id = $(this).data('id');
        $.get('/Leaves/Edit/' + id, function (data) {
            $('#editLeaveModal .modal-body').html(data);
            $('#editLeaveModal').modal('show');
        });


        


    });

    // Show delete modal with data
    $('.deleteLeaveButton').click(function () {
        var id = $(this).data('id');
        $.get('/Leaves/Delete/' + id, function (data) {
            $('#deleteLeaveModal .modal-body').html(data);
            $('#deleteLeaveModal').modal('show');
        });
    });

    // Create leave form submission
    $('#createLeaveForm').submit(function (event) {
        event.preventDefault();
        $.ajax({
            url: '/Leaves/Create',
            type: 'POST',
            data: $(this).serialize(),
            success: function () {
                $('#createLeaveModal').modal('hide');
                location.reload();
            },
            error: function (xhr, status, error) {
                alert('Error: ' + error);
            }
        });
    });

  
    

    
});

function editSubmit() {
   
    $.ajax({
        url: '/Leaves/Edit',
        type: 'POST',
        data: {

            leaveId: $("#LeaveId").val(),
            reason: $("#Reasons").val()
        },
        success: function () {
            $('#editLeaveModal').modal('hide');
            location.reload();
        },
        error: function (xhr, status, error) {
            alert('Error: ' + error);
        }
    });
}
    function deleteLeaves(id) {

        $.post('/Leaves/DeleteConfirmed', { id: id }, function () {
            $('#deleteLeaveModal').modal('hide');
            location.reload();
        }).fail(function (xhr, status, error) {
            alert('Error: ' + error);
        });
    };