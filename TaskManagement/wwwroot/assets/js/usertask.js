

/***************************generer la date aujourdhui et moins une semaine *******/
 
    $('#excelButton').attr('disabled', 'disabled');
$(document).ready(function () {

    $("#userId").selectpicker({
        liveSearch: true,
        actionsBox: true
    });


    // Mettre à jour le Date Range Picker avec les nouvelles dates
    function updateDateRange() {
        return new Promise((resolve, reject) => {
            try {
                var bsRangePickerWeekNum = $('#daterange');

                function formatDate(date) {
                    const month = String(date.getMonth() + 1).padStart(2, '0');
                    const day = String(date.getDate()).padStart(2, '0');
                    const year = date.getFullYear();
                    return `${year}-${month}-${day}`;
                }

                // Date exacte une semaine avant aujourd'hui
                const today = new Date();
                const dayOfWeek = today.getDay(); // Récupérer le jour de la semaine (0 pour dimanche, 6 pour samedi)

                let exactWeekBeforeStart = new Date();
                let exactWeekBeforeEnd = new Date();

                if (dayOfWeek >= 4 && dayOfWeek <= 6) {
                    // Si aujourd'hui est jeudi, vendredi ou samedi
                    // Date fin est le mercredi de cette semaine + 1 jour
                    exactWeekBeforeEnd.setDate(today.getDate() - (dayOfWeek - 2) + 1);
                    // Date début est le jeudi de la semaine précédente
                    exactWeekBeforeStart.setDate(today.getDate() - (dayOfWeek + 3));
                } else {
                    // Pour les autres jours (lundi, mardi, mercredi)
                    // Date fin est le mercredi de la semaine précédente + 1 jour
                    exactWeekBeforeEnd.setDate(today.getDate() - (dayOfWeek + 5) + 1);
                    // Date début est le jeudi de la 2ème semaine précédente
                    exactWeekBeforeStart.setDate(today.getDate() - (dayOfWeek + 10));
                }

                // Insérer les dates dans l'input
                const formattedExactWeekBeforeStart = formatDate(exactWeekBeforeStart);
                const formattedExactWeekBeforeEnd = formatDate(exactWeekBeforeEnd);
                const combinedDates = `${formattedExactWeekBeforeStart} - ${formattedExactWeekBeforeEnd}`;

                bsRangePickerWeekNum.val(combinedDates);

                // Mettre à jour le Date Range Picker avec les nouvelles dates
                $('#startDate').val(formattedExactWeekBeforeStart);
                $('#endDate').val(formattedExactWeekBeforeEnd);
                $('#daterange').data('daterangepicker').setStartDate(exactWeekBeforeStart);
                $('#daterange').data('daterangepicker').setEndDate(exactWeekBeforeEnd);

                $('#daterange').on('change.daterangepicker', function (ev, picker) {
                    AfficheUserTask();
                });

                // Résoudre la promesse après avoir mis à jour les dates
                resolve();
            } catch (error) {
                // Rejeter la promesse en cas d'erreur
                reject(error);
            }
        });
    }

    // Fonction appelée après la mise à jour des dates
   
    // Appeler la fonction de mise à jour des dates et ensuite exécuter afterSettingDateRange
    updateDateRange()
        .then(AfficheUserTask)
        .catch(error => {
            console.error("Une erreur s'est produite lors de la mise à jour des dates : ", error);
        });


    

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

function formatPrixImput() {

    var inputPrix = $("#hoursEditUsrTask");

    var valeurDepuisBase = inputPrix.val(); 

    if (valeurDepuisBase.includes(',')) {
        valeurDepuisBase = valeurDepuisBase.replace(',', '.'); 
    }

    
    $(this).val(valeurDepuisBase);
    inputPrix.each(function () {
        clave = new Cleave(this, {
            numeral: true,
            numeralDecimalMark: '.',
            numeralDecimalScale: 2,
            numeralPositiveOnly: true, 
            numeralThousandsGroupStyle: 'thousand', 
            delimiter: '', 
            numeralPositiveOnly: true,
            numeralIntegerScale: 4, 
        });
    });

}

/****************suppression usertask ***********/
$("#deleteTimeline").on("click", function (e) {
    $("#modalDelete").modal("hide");

    $.ajax({
        url: '/UserTask/DeleteUserTask', // Change this URL to your actual endpoint
        type: 'POST',
        dataType: "JSON",
        data: {
            userTaskId: $('#userTaskIdDelete').val()
        },
        success: function (data) {
            toastr["success"]("Successfully deleted ")
            AfficheUserTask();

        },
        error: function (error) {
            toastr["error"]("Delete failed")

        },

    });

});

/***************************** */



// generale Excel
$("#excelButton").on("click", function (event) {
    

    $.ajax({

        url: '/UserTask/GenerateExcelUserTask',
        type: "POST",

        data: {
            startDate: $('#startDate').val(),
            endDate: $('#endDate').val(),
            userId: $('#userId').val()
        },
        xhrFields: {
            responseType: 'blob'
        },
        success: function (blob) {

            var link = document.createElement('a');
            var url = window.URL.createObjectURL(blob);
            link.href = url;
            link.download = "UserTask.xlsx";  // Nom du fichier
            document.body.appendChild(link);
            link.click();
            window.URL.revokeObjectURL(url);

            toastr["success"]("Generated successfully")

        },
        error: function (xhr, status, error) {
            console.error('Erreur lors de la requête Ajax : ' + error);
            toastr["error"]("Generated failed")

        },
    });

    return false;
});


/***************************** filter usertask : submit filter*/



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
                    'data-bs-target': '#offcanvasEcommerceCategoryList',
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


            $("#selectTaskId").selectpicker({
                liveSearch: true,
                actionsBox: true
            });

            $("#selectProjectId").selectpicker({
                liveSearch: true,
                actionsBox: true
            });

            var leave = $(`#usertask_${id}`).data("leaveid")
            var projectId = $(`#usertask_${id}`).data("projectid");
            var taskId = $(`#usertask_${id}`).data("taskid");

           
            formatPrixImput();
          

            $('#checkleave').change(function () {

                if ($(this).is(':checked')) {

                    getLeaves(projectId);

                } else {
                   
                    getProject(projectId);

                    getTasks(projectId, taskId)
                }
            });

            if (leave > 0 && leave != null) {

                getLeaves(projectId);

            } else {

                getProject(projectId, taskId);
                getTasks(projectId, taskId) 

            }

           

            /* save edit *****/
            $("#formUserTask").on("submit", function (e) {
                e.preventDefault();
                
                let data = new FormData(this);
                data.delete('isLeave');


                // Convert FormData to JSON object
                const jsonObject = {};
                data.forEach((value, key) => {
                    jsonObject[key] = value;
                });

                // Convert JSON object to JSON string
                const jsonString = JSON.stringify(jsonObject);
                $("#modalEdit").modal("hide");

                $.ajax({

                    url: '/UserTask/UpdateUserTask',
                    type: "POST",
                    processData: false,
                    contentType: false,
                    cache: false,
                    dataType: "JSON",
                    headers: {
                        'Content-Type': 'application/json'  // Assurez-vous que le Content-Type est correct
                    },
                    data: jsonString,
                    success: function (data) {
                        toastr["success"]("Successfully update")
                        AfficheUserTask();

                    },
                    error: function (error) {
                        console.log(error);
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
            res.data.result.forEach(function (item) {

                var option = $('<option>', {
                    value: item.projectId,
                    text: item.name
                });

                $("#selectProjectId").append(option);
            });

            $('#selectProjectId').attr('name', "projectId");
            $('#selectProjectId').attr('required', "required");
            $('#selectTaskId').attr('required', "required");
            $('#selectTaskId').prop('disabled', false);
            $(`#labelleaveproject`).text("Project");
            $("#selectTaskId").selectpicker("refresh");

            $('#selectProjectId').on('change', function () {
                getTasks($(this).val(), idTask)
            });

            $("#selectProjectId").val(id); 

            $("#selectProjectId").selectpicker("refresh");
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
            $("#selectTaskId").empty();
            $(`#labelleaveproject`).text("Leave");
            $('#selectTaskId').prop('disabled', true);
            $("#selectTaskId").selectpicker("refresh");

            $('#selectProjectId').on('change', function () {
               
            });

            $("#selectProjectId").val(id);

            $("#selectProjectId").selectpicker("refresh");
            //$('#selectProjectId').off('change');

          
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

 

            $("#selectTaskId").val(taskId);

            $("#selectTaskId").selectpicker("refresh");

        },

    });
}

/******************************* */
