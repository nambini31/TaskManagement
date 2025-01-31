$(document).ready(function () {
    projecProcccess()
    var projectId = $("#selectedProject").val()
    taskPrecessByProject(projectId)

    $("#selectedProject").on("change", function () {
        var projectId = $(this).val();
        taskPrecessByProject(projectId)
    });
});

function projecProcccess(){
    $.ajax({
        type: "POST",
        url: "/Home/ChartProjectProcess",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            const labels = data.map(task => task.projectName);
            const timeTotalData = data.map(task => task.timeTotal);
            const statusData = data.map(task => task.status);
            const ctx = document.getElementById('projectProccess').getContext('2d');
            const tasksChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: 'Estimated Time(hours)',
                            data: timeTotalData,
                            backgroundColor: 'rgba(54, 162, 235, 0.2)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 1
                        },
                        {
                            label: 'Status (%)',
                            data: statusData,
                            backgroundColor: 'rgba(255, 99, 132, 0.2)',
                            borderColor: 'rgba(255, 99, 132, 1)',
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }

            });            
        },
        error: function (err) {
            console.error("Erreur lors de la récupération des données :", err);
        }
    });
}

function taskPrecessByProject(projectId) {
    var chartInstance = null;
    $.ajax({
        type: "POST",
        url: "/Home/ChartTaskProcessByProject",
        dataType: "JSON",
        data: {
            projectId: projectId
        },
        success: function (data) {        
            if (chartInstance) {
                chartInstance.destroy();
            }
            $("#taskProcessByProject").remove(); // Supprime l'ancien canvas
            $("#afficheCanva").append('<canvas id="taskProcessByProject" style="width:100%; height:200px"></canvas>');

            const ctx = document.getElementById('taskProcessByProject').getContext('2d');
            const statusData = data.map(task => task.status);
            const nomTache = data.map(task => task.name);
            chartInstance = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: nomTache,
                    datasets: [{
                        label: data.projectName,
                        data: statusData,
                        backgroundColor: [
                            'rgb(255, 99, 132)',
                            'rgb(54, 162, 235)',
                            'rgb(255, 205, 86)'
                        ],
                        hoverOffset: 4
                    }]
                },
            });
        },
        error: function (err) {
            console.error("Erreur lors de la récupération des données :", err);
        }
    });    
}

