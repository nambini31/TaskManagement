$(document).ready(function () {



    $("#selectTaskId").selectpicker({
        liveSearch: true,
        actionsBox: true
    });

    $("#selectProjectId").selectpicker({
        liveSearch: true,
        actionsBox: true
    });



    // formatPrixImput();
    getProject();
    getTasks();


    $('#checkleave').change(function () {

        if ($(this).is(':checked')) {

            getLeaves();

        } else {

            getProject();

            
        }
    });
});

   


 function getLeaves() {

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

           // $("#selectProjectId").val(id);

            $("#selectProjectId").selectpicker("refresh");
            //$('#selectProjectId').off('change');


        },

    });
} 
function getTasks(projectId) {

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

            //$("#selectTaskId").val(taskId);

            $("#selectTaskId").selectpicker("refresh");

        },

    })
}

function getProject() {

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
            $('#selectTaskId').prop('disabled', false);
            $(`#labelleaveproject`).text("Project");
            $("#selectTaskId").selectpicker("refresh");

            $('#selectProjectId').on('change', function () {
                getTasks($(this).val())
            });

           // $("#selectProjectId").val(id);

            $("#selectProjectId").selectpicker("refresh");
        },

    });
}

