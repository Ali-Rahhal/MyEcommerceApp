//code needed for datatable to work in order index page
var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else if (url.includes("pending")) {
        loadDataTable("pending");
    }
    else if (url.includes("completed")) {
        loadDataTable("completed");
    }
    else if (url.includes("approved")) {
        loadDataTable("approved");
    }
    else {
        loadDataTable("all");
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Orders/Index?handler=All&status=' + status},//this url will call the OnGetAll method in the page model which returns all the orders in json format.
        "columns": [//defining the columns of the datatable and mapping them to the properties of the order model.
            { data: 'id', "width": "5%" },//dont forget the names should match the property names.
            { data: 'name', "width": "25%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'applicationUser.email', "width": "20%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {//this is to render the edit and delete buttons in the last column.
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/Admin/Orders/Details?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                    </div>`//using `` for multi-line string and ${} for variable interpolation.
                },//anchors only work with Get requests.
                "width": "10%"
            }
        ]
    });
}