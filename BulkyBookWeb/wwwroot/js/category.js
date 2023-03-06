var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblCategoryData').DataTable({
        "ajax": {
            "url": "/Admin/Category/GetAllCategory"
        },
        "columns": [
            { "data": "id", "width": "15%" },
            { "data": "name", "width": "15%" },
            { "data": "displayOrder", "width": "15%" },
            { "data": "createdDateTime", "width": "20%" },
                
            {
                "data": "id",
                "render": function (data) {
                    return `
                       
                            <a href="/Admin/Category/Edit?id=${data}"
                            class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                            <a href="/Admin/Category/Delete?id=${data}"
                            class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					   
                        `
                },
                "width": "20%"
            }
        ]
    });
}
