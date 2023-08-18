var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/User/Getall'},
        "columns": [
            { data: 'name', "width": "25%" },
            { data: 'applicationUser.email', "width": "15%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'company.name', "width": "10%" },
            { data: '', "width": "10%" },
            {
                data: 'companyId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/Admin/Company/Upsert/${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                    <a onClick=Delete("/admin/company/delete/${data}") class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i >Delete</a>
                    </div>`
                },
                "width" : "25%"
            }
        ]
    });
}