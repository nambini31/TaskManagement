$(document).ready(function () {
    
    $("#selectTaskId").selectpicker({
        liveSearch: true,
        actionsBox: true
    });

    $("#selectProjectIdCreate").selectpicker({
        liveSearch: true,
        actionsBox: true
    });

    getProject();

   
    formatPrixImput();
});

$('#checkleave').change(function () {

    if ($(this).is(':checked')) {
        getLeaves();
    } else {
        getProject();
    }
});

function formatPrixImput() {

    var inputPrix = $("#hoursEditUsrTask");

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

function getLeaves() {
    $.ajax({
        url: '/Leaves/GetAllLeaves',
        type: 'GET',
        dataType: "JSON",
        success: function (res) {
            $("#selectProjectIdCreate").empty();

            res.data.forEach(function (item) {
                var option = $('<option>', {
                    value: item.leaveId,
                    text: item.reason
                });
                $("#selectProjectIdCreate").append(option);
            });

            $('#selectProjectIdCreate').attr('name', "leaveId");//huu
            $('#selectProjectIdCreate').removeAttr("required");
            $('#selectTaskId').removeAttr("required");
            $("#selectTaskId").empty();
            $('#labelleaveproject').text("Leave");
            $('#selectTaskId').prop('disabled', true);
            $("#selectTaskId").selectpicker("refresh");
            $("#selectProjectIdCreate").selectpicker("refresh");
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
            
            $("#selectTaskId").selectpicker("refresh");
        },
    });
}

function getProject() {
    $.ajax({
        url: '/Project/GetAllProjects',
        type: 'GET',
        dataType: "JSON",
        success: function (res) {
            $("#selectProjectIdCreate").empty();
            res.data.result.forEach(function (item) {
                var option = $('<option>', {
                    value: item.projectId,
                    text: item.name
                });
                $("#selectProjectIdCreate").append(option);
            });

            $('#selectProjectIdCreate').attr('name', "projectId");
            $('#selectProjectIdCreate').attr('required', "required");
            $('#selectTaskId').attr('required', "required");
            $('#selectTaskId').prop('disabled', false);
            $('#labelleaveproject').text("Project");
            $("#selectTaskId").selectpicker("refresh");

            $('#selectProjectIdCreate').on('change', function () {
                getTasks($(this).val());
            });

            $("#selectProjectIdCreate").selectpicker("refresh");

            if ($("#selectProjectIdCreate").find('option').length > 0) {
                // Select the first option
                $("#selectProjectIdCreate").val($("#selectProjectIdCreate").find('option:first').val());
                getTasks($("#selectProjectIdCreate").find('option:first').val());
                // Refresh the selectpicker to reflect the change
                $("#selectProjectIdCreate").selectpicker('refresh');
            }
        },
    });
}
