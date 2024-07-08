var dtble;
$(document).ready(function () {

    loadData()
});
function loadData() {
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAllData"
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            { "data": "price" },
            { "data": "beforePrice" },
            { "data": "description" },
            { "data": "catigory.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <a class="btn btn-info btn-sm" href="/Admin/Product/Edit/${data}">
                                <i class="fas fa-pencil-alt">
                                </i>
                                Edit
                            </a>
                            <a  onClick=DeleteItem("/Admin/Product/DeleteData/${data}") class="btn btn-danger btn-sm">
                                <i class="fas fa-trash">
                                </i>
                                Delete
                            </a>
                           `
                }
            }
        ]

    });
}
function DeleteItem(url) {

    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {

        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        dtble.ajax.reload();
                        toastr["success"](data.message)

                    } else {
                        toastr["error"](data.message)
                    }
                }

            });
        }
    });
}

