$(document).ready(function () {
    $('#searchInput').on('keyup', function () {
        var value = $(this).val().toLowerCase();
        $('#leaveTable tbody tr').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    $('#updateLeaveModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var reason = button.data('reason');

        var modal = $(this);
        modal.find('#updateLeaveId').val(id);
        modal.find('#updateReason').val(reason);
    });

    $('#leaveTable').DataTable({
        responsive: true
    });
});

function addLeave() {
    var reason = $('#addLeaveForm #reason').val();

    $.ajax({
        url: '/api/leaves',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Reason: reason }),
        success: function (response) {
            if (response.success) {
                $('#addLeaveModal').modal('hide');
                location.reload();
            } else {
                alert('Error adding leave: ' + response.message);
            }
        },
        error: function () {
            alert('Error adding leave');
        }
    });
}

function updateLeave() {
    var id = $('#updateLeaveForm #updateLeaveId').val();
    var reason = $('#updateLeaveForm #updateReason').val();

    $.ajax({
        url: '/api/leaves',
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify({ LeaveId: id, Reason: reason }),
        success: function (response) {
            if (response.success) {
                $('#updateLeaveModal').modal('hide');
                location.reload();
            } else {
                alert('Error updating leave: ' + response.message);
            }
        },
        error: function () {
            alert('Error updating leave');
        }
    });
}

function deleteLeave(id) {
    $.ajax({
        url: '/api/leaves/' + id,
        type: 'DELETE',
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert('Error deleting leave: ' + response.message);
            }
        },
        error: function () {
            alert('Error deleting leave');
        }
    });
}
