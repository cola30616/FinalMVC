var dataTable; // 這段要記得加，先宣告datatable的變數

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    // html 欄位跟 ajax 的欄位務必相同
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/TeacherAdmin/GetVideoData' },
        "columns": [
            { data: 'fVideoName', "width": "25%" },
            { data: 'fVideoPath', "width": "25%" },
            { data: 'fUploadTime', "width": "25%" },          
            {
                // 這段是新增及刪除按鈕 ， 刪除用到onclick 事件，觸發下方的Delete
                data: 'fVideoUploadUrlId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/TeacherAdmin/GetVideoData?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i>Edit </a>
                        
                    </div>`
                },
                "width": "25%"
            },
        ]
    });


}
 //這一段是用sweetalert 做是否取消，可參考上方sweeetAlertfile
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
            })
        }
    });
}
