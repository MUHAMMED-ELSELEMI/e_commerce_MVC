var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Companies/GetAll' },
        "columns": [
            { data: 'companyName', "width": "15%" },
            { data: 'companyPhoneNumber', "width": "15%" },
            { data: 'companyAddress', "width": "30%" },
            { data: 'companyPostalCode', "width": "15%" },
            {
                data: 'companyId',
                "render": function (data) {
                    return `<td class="text-center">
                                <a href="/admin/Companies/Upsert?id=${data}" class="btn btn-warning btn-sm btn-fancy">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a href="/admin/Companies/Details?id=${data}" class="btn btn-info btn-sm btn-fancy">
                                    <i class="fas fa-info-circle"></i> 
                                </a>
                                <a onClick=Delete('/admin/Companies/delete/${data}') class="btn btn-danger btn-sm btn-fancy">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </td>`;
                }, "width": "15%"
            }
        ]
    });
}

function Delete(url) {
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
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}
