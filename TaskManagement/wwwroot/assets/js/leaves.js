$(document).ready(function () {
    // Initialize DataTable
    

    AfficheLeaves();

    // Show create modal
   

    

    // Create leave form submission
    $('#createLeaveForm').submit(function (event) {
        event.preventDefault();
        $.ajax({
            url: '/Leaves/Create',
            type: 'POST',
            data: $(this).serialize(),
            success: function () {
                toastr["success"]("Successfuly !!");
                $('#createLeaveModal').modal('hide');
                AfficheLeaves();
            },
            error: function (xhr, status, error) {
                toastr["error"]("Leaves's Name already exist !!");

            }
        });
    });

  
    

    
});


function AfficheLeaves() {

    $('#table_leave').DataTable({
        ajax: {
            url: '/Leaves/GetAllLeaves',
            type: 'GET',

            dataType: "JSON",
            dataSrc: function (json) {

                return json.data;
            }
        },
        columns: [


            { data: 'leaveId', title: '#' },
            { data: 'reason', title: 'Name' },
            {
                data: null,
                title: 'Action',
                render: function (data, type, row) {
                    return `
                            <a class="btn btn-sm btn-primary editLeaveButton" style="color:white"

                            id="leave_${row.leaveId}"
                            data-id="${row.leaveId}"
                            data-name="${row.reason}"
                            ><i class="fe-edit"></i></a>

                            <a class="btn btn-sm btn-danger deleteLeaveButton" data-id="${row.leaveId}" style="color:white"
                            " >
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
                className: 'add-new btn btn-primary ms-2',
                attr: {
                    //'data-bs-toggle': 'modal',
                    //'data-bs-target': '#createProjectModal',
                    'id': "createLeaveButton"
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

        rowCallback: function (row, data) {
            $('#createLeaveButton').click(function () {
                $('#createLeaveModal').modal('show');
            });
            if (data.leaveId > 0) {
                // Show edit modal with data
                $(document).on('click', '.editLeaveButton', function () {
                    var id = $(this).data('id');
                    $.get('/Leaves/Edit/' + id, function (data) {
                        $('#editLeaveModal .modal-body').html(data);
                        $('#editLeaveModal').modal('show');
                    });





                });

                // Show delete modal with data
                $(document).on('click', '.deleteLeaveButton', function () {
                    var id = $(this).data('id');
                    $.get('/Leaves/Delete/' + id, function (data) {
                        $('#deleteLeaveModal .modal-body').html(data);
                        $('#deleteLeaveModal').modal('show');
                    });
                });
            }
        }

    });

    

}

function editSubmit() {
   
    $('#editLeaveModal').modal('hide');
    $.ajax({
        url: '/Leaves/Edit',
        type: 'POST',
        data: {

            leaveId: $("#LeaveId").val(),
            reason: $("#Reasons").val()
        },
        success: function () {
            toastr["success"]("Successfuly !!");
            AfficheLeaves();
        },
        error: function (xhr, status, error) {
            toastr["error"]("Leaves's Name already exist !!");
        }
    });
}
    function deleteLeaves(id) {

            $('#deleteLeaveModal').modal('hide');
        $.post('/Leaves/DeleteConfirmed', { id: id }, function () {
            toastr["success"]("Successfuly");
            AfficheLeaves();
        }).fail(function (xhr, status, error) {
            alert('Error: ' + error);
        });
    };