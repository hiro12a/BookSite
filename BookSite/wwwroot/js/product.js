var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Product/Getall'},
        "columns": [
            { data: 'title', "width": "25%" },
            { data: 'description', "width": "15%" },
            { data: 'author', "width": "15%" },
            { data: 'category.name', "width": "15%" },
            {
                data: 'productId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                    <a onClick=Delete("/admin/product/delete/${data}") class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i >Delete</a>
                    </div>`
                },
                "width": "25%"
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