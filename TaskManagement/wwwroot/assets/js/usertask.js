

/***************************generer la date aujourdhui et moins une semaine *******/
 
    $('#excelButton').attr('disabled', 'disabled');
$(document).ready(function () {

    $("#userId").selectpicker({
        liveSearch: true,
        actionsBox: true
    });


    var bsRangePickerWeekNum = $('#daterange');

    function formatDate(date) {
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        const year = date.getFullYear();
        return `${year}-${month}-${day}`;
    }

    // Date exacte une semaine avant aujourd'hui
    const today = new Date();
    const exactWeekBefore = new Date();
    exactWeekBefore.setDate(today.getDate() - 7);

    // Insérer les dates dans l'input
    const formattedExactWeekBefore = formatDate(exactWeekBefore);
    const formattedToday = formatDate(today);
    const combinedDates = `${formattedExactWeekBefore} - ${formattedToday}`;

    bsRangePickerWeekNum.val(combinedDates);
    // Mettre à jour le Date Range Picker avec les nouvelles dates
    const startDate = exactWeekBefore;
    const endDate = today;

    $('#startDate').val(formattedExactWeekBefore);
    $('#endDate').val(formattedToday);
    $('#daterange').data('daterangepicker').setStartDate(startDate);
    $('#daterange').data('daterangepicker').setEndDate(endDate);


    AfficheUserTask();


    $('#userId').on('changed.bs.select', function () {


            AfficheUserTask();
 

    });
   




});


/*************************** */


/************configuration input date from to */
$('#daterange').daterangepicker({
    opens: 'left',
    locale: {
        format: 'YYYY-MM-DD'
    }
}, function (start, end, label) {

    $('#startDate').val(start.format('YYYY-MM-DD'));
    $('#endDate').val(end.format('YYYY-MM-DD'));

    

});
/************************************ */

// charger listes des utilisateur

$.ajax({
    url: '/User/GetAllUser',
    type: 'POST',
    dataType :"JSON",
    success: function (data) {

        data.forEach(function (item) {
            
            var option = $('<option>', {
                value: item.userId,
                text: item.username
            });

            $("#userId").append(option);
        });

        $("#userId").selectpicker("refresh");
        

    },
   
});

/******************************* */


/****************suppression usertask ***********/
$("#deleteTimeline").on("click", function (e) {

    $.ajax({
        url: '/UserTask/DeleteUserTask', // Change this URL to your actual endpoint
        type: 'POST',
        dataType: "JSON",
        data: {
            userTaskId: $('#userTaskIdDelete').val()
        },
        success: function (data) {
            toastr["success"]("Successfully deleted")
            $("#modalDelete").modal("hide");
            AfficheUserTask();

        },
        error: function (error) {
            toastr["error"]("Delete failed")
            $("#modalDelete").modal("hide");

        },

    });

});

/***************************** */



// generale Excel
$("#excelButton").on("click", function (event) {

    $.ajax({

        url: '/UserTask/GenerateExcelUserTask',
        type: "POST",
        dataType: "JSON",
        data: {
            startDate: $('#startDate').val(),
            endDate: $('#endDate').val(),
            userId: $('#userId').val()
        },
        success: function (data) {
            toastr["success"]("Successfully update")
            $("#modalEdit").modal("hide");
            AfficheUserTask();

        },
        error: function (error) {
            toastr["error"]("Update failed")
            $("#modalEdit").modal("hide");

        },
    });

    return false;
});


/***************************** filter usertask : submit filter*/

$("#filtreUserTask").on("submit", function (e) {
    e.preventDefault(); 

    AfficheUserTask();

    return false; 
});



/************************ */

//********************************* liste usertask***************
function AfficheUserTask() {

    $('#table_usertask').DataTable({
        ajax: {
            url: '/UserTask/UserTaskList',
            type: 'POST',
            data: {
                startDate: $('#startDate').val(),
                endDate: $('#endDate').val(),
                userId: $('#userId').val()
            }, 
            dataType: "JSON",
            dataSrc: function (json) {
                
                if (json && json.length > 0) {
                    $('#excelButton').removeAttr('disabled');
                } else {
                    $('#excelButton').attr('disabled', 'disabled');
                }
                return json;
            }
        },
        columns: [
           
            
            { data: 'userTaskId', title: '#' },
            { data: 'projectName', title: 'Project' },
            { data: 'taskName', title: 'Task' },
            { data: 'hours', title: 'Hours' },
            { data: 'userName', title: 'Username' },
            {
                data: 'datetime',
                title: 'Date',
                render: function (data, type, row) {
                    
                    return moment(data).format('YYYY-MM-DD HH:mm:ss');
                }
            },
            {
                data: null,
                title: 'Action',
                render: function (data, type, row) {
                    return `
                            <a class="btn btn-sm btn-primary" style="color:white"

                            id="usertask_${row.userTaskId}"
                            data-projectId="${row.projectId}"
                            data-leaveId="${row.leaveId}"
                            data-taskId="${row.taskId}"
                            onclick="ModalEdit(${row.userTaskId})"><i class="fe-edit"></i></a>

                            <a class="btn btn-sm btn-danger" style="color:white"
                            data-toggle="modal" onclick="ModalDelete(${row.userTaskId})">
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
                    'data-bs-toggle': 'offcanvas',
                    'data-bs-target': '#offcanvasEcommerceCategoryList'
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
            if (data.leaveId > 0) {
                $(row).addClass('leave-row'); // Ajoutez une classe CSS pour marquer la ligne en rouge
            }
        }

    });

}
/**
 * *********************************
 */

/*******affiche model delete */
function ModalDelete(id) {

    $('#userTaskIdDelete').val(id)
    $('#textDelete').html(`Do you really want to delete this task ( ${id} ) ?`)
    $("#modalDelete").modal("show");

}

/*******affiche model edit */
function ModalEdit(id) {

       
       
       $.ajax({
           url: '/UserTask/ModalUserTaskEdit',
           type: 'POST',
           data: {
               userTaskId: id
           },
        success: function (data) {
            $('#editUserTaskContainer').html(data);
            $("#modalEdit").modal("show");

            var leave = $(`#usertask_${id}`).data("leaveid")
            var projectId = $(`#usertask_${id}`).data("projectid");
            var taskId = $(`#usertask_${id}`).data("taskid");

            $('#checkleave').change(function () {

                if ($(this).is(':checked')) {

                    $("#selectTaskId").empty();
                    getLeaves(projectId);


                    $(`#labelleaveproject`).text("Leave");
                    $(`#selectTaskId`).attr("disabled", "disabled");

                } else {
                    $(`#selectTaskId`).removeAttr("disabled");

                    $(`#labelleaveproject`).text("Project");

                    getProject(projectId);

                    getTasks(projectId, taskId)
                }
            });

            if (leave > 0 && leave != null) {

                
                getLeaves(projectId);


                $(`#labelleaveproject`).text("Leave");
                $(`#selectTaskId`).attr("disabled", "disabled");

            } else {
                $(`#labelleaveproject`).text("Project");

                getProject(projectId, taskId);

                getTasks(projectId, taskId) 

            }

           

            /* save edit *****/
            $("#formUserTask").on("submit", function (e) {
                e.preventDefault();

                let data = new FormData(this);
                data.delete('isLeave');

                $.ajax({

                    url: '/UserTask/UpdateUserTask',
                    type: "POST",
                    processData: false,
                    contentType: false,
                    cache: false,
                    dataType: "JSON",
                    data: data,
                    success: function (data) {
                        toastr["success"]("Successfully update")
                        $("#modalEdit").modal("hide");
                        AfficheUserTask();

                    },
                    error: function (error) {
                        toastr["error"]("Update failed")
                        $("#modalEdit").modal("hide");

                    },
                });

                return false;
            });



        },
            error: function (xhr, status, error) {
                alert('Failed to load edit form');
            }
       });
    


}



// charger listes des projets et leaves

function getProject(id , idTask) {

    $.ajax({
        url: '/Project/GetAllProjects',
        type: 'GET',
        dataType: "JSON",
        success: function (res) {

            $("#selectProjectId").empty();
            res.data.forEach(function (item) {

                var option = $('<option>', {
                    value: item.projectId,
                    text: item.name
                });

                $("#selectProjectId").append(option);
            });

            $('#selectProjectId').attr('name', "projectId");

            $('#selectProjectId').attr('required', "required");
            $('#selectTaskId').attr('required', "required");

            $('#selectProjectId').on('change', function () {
                getTasks($(this).val(), idTask)
            });

            $("#selectProjectId").val(id); 
        },

    });
}


/** charger leaves depuis id */
function getLeaves(id) {

    //$(`#selectTaskId`).attr("disabled", "disabled");

    $.ajax({
        url: '/Leaves/GetAllLeaves',
        type: 'GET',
        dataType: "JSON",
        success: function (res) {

            

            $("#selectProjectId").empty();
            res.data.forEach(function (item) {

                var option = $('<option>', {
                    value: item.leaveId,
                    text: item.reason
                });

                $("#selectProjectId").append(option);
            });

            $('#selectProjectId').attr('name', "leaveId");
            $('#selectProjectId').removeAttr("required");
            $('#selectTaskId').removeAttr("required");

            $('#selectProjectId').off('change');

            $("#selectProjectId").val(id);

          
        },

    });
}


/******************************* */

/** charger tasks depuis id */
function getTasks(projectId , taskId) {

    $.ajax({
        url: '/Tasks/GetTaskByIdProject',
        type: 'POST',
        dataType: "JSON",
        data: {

            projectId: projectId
        },
        success: function (res) {
            $("#selectTaskId").empty();
            res.forEach(function (item) {

                var option = $('<option>', {
                    value: item.taskId,
                    text: item.name
                });

                $("#selectTaskId").append(option);
            });

            //VirtualSelect.init({
            //    ele: '#selectTaskId'
            //});

            $("#selectTaskId").val(taskId);
        },

    });
}

/******************************* */
