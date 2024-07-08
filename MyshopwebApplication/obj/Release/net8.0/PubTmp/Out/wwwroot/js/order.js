var dtble;
$(document).ready(function () {

    loadData()
});
function loadData() {
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAllData"
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            { "data": "phoneNumber" },
            { "data": "applicationUser.email" },
            { "data": "orderStatus" },
            { "data": "totalPrice" },
            { "data": "shippingDate" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <a class="btn btn-info" href="/Admin/Order/Detail/${data}">
                                Detail
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

