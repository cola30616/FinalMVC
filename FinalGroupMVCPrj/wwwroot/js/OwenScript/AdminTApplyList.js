let dataTable; // 這段要記得加，先宣告datatable的變數

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    // html 欄位跟 ajax 的欄位務必相同
    dataTable = $('#tApplyListTable').DataTable({
        "ajax": { url: '/AdminTeacher/ListDataJson' },
        "columns": [
            { data: 'applyDatetime', "width": "12%" },
            { data: 'realName', "width": "8%" },
            { data: 'teacherName', "width": "10%" },
            { data: 'reviewDatetime', "width": "12%" },
            { data: 'progressStatus', "width": "7%" },
            { data: 'note', "width": "15%" },
            {
                // 這段是新增及刪除按鈕 ， 刪除用到onclick 事件，觸發下方的Delete
                data: 'applyLogId',
                "render": function (data) {
                    return `<div class="" role="">
                        <button  memberId=${data}" class="btn btn-primary mx-2" data-bs-toggle="modal" data-bs-target="#staticBackdrop"><i class="fa-solid fa-gear"></i></button>
                    </div>`
                },
                "width": "5%"
            },
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        },
        order: [[1, 'dsc']]


    });
}