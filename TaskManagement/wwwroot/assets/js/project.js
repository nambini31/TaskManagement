$(document).ready(function () {
    
    AfficheProjects();

   
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
                AfficheProjects();
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

                AfficheProjects();
            },
            error: function (xhr, status, error) {
                alert('Error: ' + error);
            }
        });
        return false;
    });

    
    // Handle delete project confirmation
    $('#deleteProjectModal').on('click', '#confirmDeleteButton', function () {
        var id = $("#ProjectId").val(); 
        $.ajax({
            url: '/Project/DeleteConfirmed/',
            type: 'POST',
            data: {
                id = id
            },
            success: function () {
                $('#deleteProjectModal').modal('hide');
                AfficheProjects();
            },
            error: function (xhr, status, error) {
                alert('Error: ' + error);
            }
        });
    });
});


function AfficheProjects() {

    $('#table_project').DataTable({
        ajax: {
            url: '/Project/GetAllProjects',
            type: 'GET',
            
            dataType: "JSON",
            dataSrc: function (json) {
               
                return json.data.result;
            }
        },
        columns: [


            { data: 'projectId', title: '#' },
            { data: 'name', title: 'Name' },
            {
                data: null,
                title: 'Action',
                render: function (data, type, row) {
                    return `
                            <a class="btn btn-sm btn-primary editProjectButton" style="color:white"

                            id="project_${row.projectId}"
                            data-id="${row.projectId}"
                            data-name="${row.name}"
                            ><i class="fe-edit"></i></a>

                            <a class="btn btn-sm btn-danger deleteProjectButton" data-id="${row.projectId}" style="color:white"
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
                className: 'add-new btn btn-primary ms-2',
                attr: {
                    'data-bs-toggle': 'modal',
                    'data-bs-target': '#createProjectModal',
                    'id' : "createProjectButton"
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

