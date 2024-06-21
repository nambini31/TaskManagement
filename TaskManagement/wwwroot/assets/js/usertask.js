

$(document).ready(function () {
  
    $('#table_usertask').DataTable({
        destroy: true,
        ordering: true,
        order: [[0, "desc"]],
        responsive: true,
        info: false,
        paging: true,
        deferRender: true,
        pageLength: 7,
        "initComplete": function (settings, json) {
            $('div.dataTables_wrapper div.dataTables_filter input').attr('placeholder', 'Recherche').attr('class', 'form-control');
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
                className: "btn btn-sm btn-secondary btn-min-width ",
                text: '<i class="ft-refresh"> Actualiser</i>',
                action: function () {

                    liste_membre();


                },
            },
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
});