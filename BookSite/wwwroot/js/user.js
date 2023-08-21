var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/User/GetAll'},
        "columns": [
            { "data": 'name', "width": "15%" },
            { "data": 'email', "width": "15%" },
            { "data": 'phoneNumber', "width": "15%" },
            { "data": 'company.name', "width": "15%" },
            { "data": 'role', "width": "15%" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime(); // Get todays date
                    var lockout = new Date(data.lockoutEnd).getTime(); // Convert to int value
                    if (lockout >= today) {
                        // User currently locked out
                        return `<div class="text-center btn-group">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white m-1" style="cursor:pointer; width:100px">
                                <i class="bi bi-lock-fill"></i> lock
                            </a>
                            <a href="/Admin/User/RoleManagement?userId=${data.id}" class="btn btn - danger text - white m - 1" style="cursor: pointer; width: 150px">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                        </div>`

                    }
                    else {
                        // User currently locked out
                        return `<div class = "text-center btn-group">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white m-1" style="cursor:pointer; width:100px;">
                                <i class="bi bi-unlock-fill"></i> Unlock
                            </a>
                            <a href="/Admin/User/RoleManagement?userId=${data.id}" class="btn btn-danger text-white m-1" style="cursor:pointer; width:150px;">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                        </div>`
                    }
                },
                "width" : "25%"
            }
        ]
    });
}
function LockUnlock(id) {
    $.ajax({
        type: 'POST',
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                dataTable.ajax.reload();
                
            }
        }
    });
}