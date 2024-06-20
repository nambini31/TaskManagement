if ($.fn.DataTable.isDataTable("table_usertask")) {
    $("#table_usertask").DataTable().destroy();
} else {
}
$('#table_usertask').empty();


$("#table_usertask").DataTable({
    ajax: assetsPath + 'json/ecommerce-category-list.json', // JSON file to add data
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
            // Total products
            targets: 3,
            responsivePriority: 3,
            render: function (data, type, full, meta) {
                var $total_products = full['total_products'];
                return '<div class="text-sm-end">' + $total_products + '</div>';
            }
        },
        {
            // Total Earnings
            targets: 4,
            orderable: false,
            render: function (data, type, full, meta) {
                var $total_earnings = full['total_earnings'];
                return "<div class='h6 mb-0 text-sm-end'>" + $total_earnings + '</div';
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
    dom:
        '<"card-header d-flex flex-wrap pb-2"' +
        '<f>' +
        '<"d-flex justify-content-center justify-content-md-end align-items-baseline"<"dt-action-buttons d-flex justify-content-center flex-md-row mb-3 mb-md-0 ps-1 ms-1 align-items-baseline"lB>>' +
        '>t' +
        '<"row mx-2"' +
        '<"col-sm-12 col-md-6"i>' +
        '<"col-sm-12 col-md-6"p>' +
        '>',
    lengthMenu: [7, 10, 20, 50, 70, 100], //for length of menu
    language: {
        sLengthMenu: '_MENU_',
        search: '',
        searchPlaceholder: 'Search Category'
    },
    // Button for offcanvas
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
    // For responsive popup
    responsive: {
        details: {
            display: $.fn.dataTable.Responsive.display.modal({
                header: function (row) {
                    var data = row.data();
                    return 'Details of ' + data['categories'];
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
                        '<td> ' +
                        col.title +
                        ':' +
                        '</td> ' +
                        '<td class="ps-0">' +
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