


var datatable;


$(document).ready(function () {
    loadDataTabel();


});

function loadDataTabel() {

    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Products/GetAll' },
        "columns": [
            { data: 'author', "width": "10%" },
            { data: 'isbn', "width": "25%" },
            { data: 'title', "width": "10%" },
            { data: 'price', "width": "10%" },
            { data: 'category.name', "width": "10%" },
            {

                data: 'id',
                "render": function (data) {

                    return `<td class="text-center">
                                    <a href="/admin/products/Upsert?id= ${data} " class="btn btn-warning btn-sm btn-fancy">
                                        <i class="fas fa-edit"></i> edit
                                    </a>
                                    <a href="/admin/products/Details?id=${data} " class="btn btn-info btn-sm btn-fancy">
                                        <i class="fas fa-info-circle"></i> details
                                    </a>
                                    <a onClick= Delete('/admin/products/delete/${data}') class="btn btn-danger btn-sm btn-fancy">
                                        <i class="fas fa-trash-alt"></i> delete
                                    </a>
                                </td>`

                                    
                }, "width": "35%"



            }
        ]
    });

}


function Delete (url) {

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
                url: url ,  
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })

        }
    })
}
