if ($.fn.DataTable.isDataTable("table_usertask")) {
    $("#table_usertask").DataTable().destroy();
} else {
}
$('#table_usertask').empty();

$("#table_usertask").DataTable({
    ajax: {
        url: '/UserTask/UserTaskList', // URL du contrôleur pour récupérer les données JSON
        dataSrc: '', // Si les données directes JSON
    },
    columns: [
        // columns according to JSON
        { data: '' },
        { data: 'userTaskId', title: '#' },
        { data: 'projectName', title: 'Project' },
        { data: 'taskName', title: 'Task' },
        { data: 'hours', title: 'Hours' },
        { data: 'datetime', title: 'date' },
        { data: '' }
    ],
    columnDefs: [
        {
            // For Responsive
            className: 'control',
            searchable: false,
            orderable: false,
            targets: 0,
            render: function (data, type, full, meta) {
                return '';
            }
        },
        {
            // Actions
            targets: -1,
            title: 'Actions',
            searchable: false,
            orderable: false,
            render: function (data, type, full, meta) {
                return (
                    '<div class="d-flex align-items-sm-center justify-content-sm-center">' +
                    '<button class="btn btn-sm btn-icon delete-record me-2"><i class="ti ti-trash"></i></button>' +
                    '<button class="btn btn-sm btn-icon"><i class="ti ti-edit"></i></button>' +
                    '</div>'
                );
            }
        }
    ],
    order: [2, 'desc'], //set any columns order asc/desc
    
    info: false,
    lengthChange: false,
    language: {
        search: '',
        searchPlaceholder: 'Search Category'
    },
    buttons: [
        {
            text: '<i class="ti ti-plus ti-xs me-0 me-sm-2"></i><span class="d-none d-sm-inline-block">Add Category</span>',
            className: 'add-new btn btn-primary ms-2 btn-sm',
            attr: {
                'data-bs-toggle': 'offcanvas',
                'data-bs-target': '#offcanvasEcommerceCategoryList'
            }
        }
    ],
    // Button for offcanvas
    dom:
        '<"card-header d-flex flex-wrap pb-2"' +
        '<f>' +
        '<"d-flex justify-content-center justify-content-md-end align-items-baseline"<"dt-action-buttons d-flex justify-content-center flex-md-row mb-3 mb-md-0 ps-1 ms-1 align-items-baseline"lB>>' +
        '>t' +
        '<"row mx-2"' +
        '<"col-sm-12 col-md-6"i>' +
        '<"col-sm-12 col-md-6"p>' +
        '>',
    orderCellsTop: true,
    responsive: {
        details: {
            display: $.fn.dataTable.Responsive.display.modal({
                header: function (row) {
                    var data = row.data();
                    return 'Details of ' + data['full_name'];
                }
            }),
            type: 'column',
            renderer: function (api, rowIdx, columns) {
                var data = $.map(columns, function (col, i) {
                    return col.title !== '' // ? Do not show row in modal popup if title is blank (for check box)
                        ? '<tr data-dt-row="' +
                        col.rowIndex +
                        '" data-dt-column="' +
                        col.columnIndex +
                        '">' +
                        '<td>' +
                        col.title +
                        ':' +
                        '</td> ' +
                        '<td>' +
                        col.data +
                        '</td>' +
                        '</tr>'
                        : '';
                }).join('');

                return data ? $('<table class="table"/><tbody />').append(data) : false;
            }
        }
    }
});
$('.dt-action-buttons').addClass('pt-0');
$('.dataTables_filter').addClass('me-3 ps-0');



var bsRangePickerWeekNum = $('#bs-rangepicker-week-num');


// Week Numbers
if (bsRangePickerWeekNum.length) {
    bsRangePickerWeekNum.daterangepicker({
        showWeekNumbers: true,
        opens: isRtl ? 'left' : 'right'
    });
}

function formatDate(date) {
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const year = date.getFullYear();
    return `${month}/${day}/${year}`;
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