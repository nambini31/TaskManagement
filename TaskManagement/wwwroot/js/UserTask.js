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
        { data: 'id' },
        { data: 'categories' },
        { data: 'total_products' },
        { data: 'total_earnings' },
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



var bsRangePickerBasic = $('#bs-rangepicker-basic'),
    bsRangePickerSingle = $('#bs-rangepicker-single'),
    bsRangePickerTime = $('#bs-rangepicker-time'),
    bsRangePickerRange = $('#bs-rangepicker-range'),
    bsRangePickerWeekNum = $('#bs-rangepicker-week-num'),
    bsRangePickerDropdown = $('#bs-rangepicker-dropdown'),
    bsRangePickerCancelBtn = document.getElementsByClassName('cancelBtn');

// Basic
if (bsRangePickerBasic.length) {
    bsRangePickerBasic.daterangepicker({
        opens: isRtl ? 'left' : 'right'
    });
}

// Single
if (bsRangePickerSingle.length) {
    bsRangePickerSingle.daterangepicker({
        singleDatePicker: true,
        opens: isRtl ? 'left' : 'right'
    });
}

// Time & Date
if (bsRangePickerTime.length) {
    bsRangePickerTime.daterangepicker({
        timePicker: true,
        timePickerIncrement: 30,
        locale: {
            format: 'MM/DD/YYYY h:mm A'
        },
        opens: isRtl ? 'left' : 'right'
    });
}

if (bsRangePickerRange.length) {
    bsRangePickerRange.daterangepicker({
        ranges: {
            Today: [moment(), moment()],
            Yesterday: [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        opens: isRtl ? 'left' : 'right'
    });
}

// Week Numbers
if (bsRangePickerWeekNum.length) {
    bsRangePickerWeekNum.daterangepicker({
        showWeekNumbers: true,
        opens: isRtl ? 'left' : 'right'
    });
}
// Dropdown
if (bsRangePickerDropdown.length) {
    bsRangePickerDropdown.daterangepicker({
        showDropdowns: true,
        opens: isRtl ? 'left' : 'right'
    });
}

// Adding btn-label-secondary class in cancel btn
for (var i = 0; i < bsRangePickerCancelBtn.length; i++) {
    bsRangePickerCancelBtn[i].classList.remove('btn-default');
    bsRangePickerCancelBtn[i].classList.add('btn-label-primary');
}