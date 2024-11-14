$(document).ready(function () {
    projecProcccess()
    taskPrecessByProject()
});

function projecProcccess(){
    $.ajax({
        type: "POST",
        url: "/Home/ChartProjectProcess",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            // Préparer les données pour Chart.js
            const labels = data.map(task => task.projectName);
            const timeTotalData = data.map(task => task.timeTotal);
            const statusData = data.map(task => task.status);

            // Créer le graphique
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

function taskPrecessByProject() {
    const ctx = document.getElementById('taskProcessByProject').getContext('2d');
    const config = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: [
                'Red',
                'Blue',
                'Yellow'
            ],
            datasets: [{
                label: 'My First Dataset',
                data: [300, 50, 100],
                backgroundColor: [
                    'rgb(255, 99, 132)',
                    'rgb(54, 162, 235)',
                    'rgb(255, 205, 86)'
                ],
                hoverOffset: 4
            }]
        },
    });
}