var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Company/Getall'},
        "columns": [
            { data: 'name', "width": "25%" },
            { data: 'streetAddress', "width": "15%" },
            { data: 'city', "width": "10%" },
            { data: 'state', "width": "10%" },
            { data: 'phoneNumber', "width": "10%" },
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
function Delete(url) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success m-3',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, Cancel it!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            swalWithBootstrapButtons.fire(
                'File has been deleted',
                $.ajax({
                    url: url,
                    type: 'DELETE',
                    success: function (data) {
                        dataTable.ajax.reload();
                    }
                })
            )
        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'The file is not deleted',
                'error'
            )
        }
    })
}