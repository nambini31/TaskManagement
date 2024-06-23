
$('#daterange').daterangepicker({
    opens: 'left',
    locale: {
        format: 'YYYY-MM-DD'
    }
}, function (start, end, label) {

    $('#startDate').val(start.format('YYYY-MM-DD'));
    $('#endDate').val(end.format('YYYY-MM-DD'));

});
$(document).ready(function () {
    
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
   



  
});

/***************************** filter usertask : submit filter*/

$("#filtreUserTask").on("submit", function (e) {
    e.preventDefault(); 

    AfficheUserTask();

    return false; // Empêcher la soumission du formulaire
});


//********************************* liste usertask
function AfficheUserTask() {

    $('#table_usertask').DataTable({
        ajax: {
            url: '/UserTask/UserTaskList',
            type: 'POST',
            data: {
                startDate: $('#startDate').val(),
                endDate: $('#endDate').val(),
                userId: $('#userId').val()
            }, // Utiliser le FormData passé en paramètre
            dataType: "JSON",
            dataSrc: ''
        },
        columns: [
            // columns according to JSON
            
            { data: 'userTaskId', title: '#' },
            { data: 'projectName', title: 'Project' },
            { data: 'taskName', title: 'Task' },
            { data: 'hours', title: 'Hours' },
            { data: 'userName', title: 'Username' },
            {
                data: 'datetime',
                title: 'Date',
                render: function (data, type, row) {
                    // Format the date using moment.js
                    return moment(data).format('YYYY-MM-DD HH:mm:ss');
                }
            },
            {
                data: null,
                title: 'Action',
                render: function (data, type, row) {
                    return `
                            <a class="btn btn-sm btn-primary" style="color:white" data-id="${row.UserTaskId}"><i class="fe-edit"></i></a>
                            <a class="btn btn-sm btn-danger" style="color:white" data-id="${row.UserTaskId}"><i class="fas fa-trash"></i></a>
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
                text: '<i class="ti ti-plus ti-xs me-0 me-sm-2"></i><span class="d-none d-sm-inline-block">Add Category</span>',
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

    });

}