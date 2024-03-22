let dataTable; // 這段要記得加，先宣告datatable的變數
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    // html 欄位跟 ajax 的欄位務必相同
    dataTable = $('#pushMsgListTable').DataTable({
        "ajax": { url: '/Message/ListDataJson' },
        "columns": [
            { data: 'FPushMessageId', "width": "7%" },
            { data: 'FPushType', "width": "12%" },
            { data: 'FPushStartDate', "width": "8%" },
            { data: 'FPushEndDate', "width": "10%" },
            { data: 'FPushContent', "width": "15%" },
            { data: 'FPushCreatedTime', "width": "7%" },
            { data: 'FPushLastUpdatedTime', "width": "8%" },
            {
                // 這段是新增及刪除按鈕 ， 刪除用到onclick 事件，觸發下方的Delete
                data: 'FPushMessageId',
                "render": function (data) {
                    return `<div class="" role="">
                        <button  FPushMessageId=${data}" class="btn btn-primary mx-2" data-bs-toggle="modal" data-bs-target="#staticBackdrop"><i class="fa-solid fa-gear"></i></button>
                    </div>`
                },
                "width": "5%"
            },
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        },
        order: [[0, 'dsc']]


    });
}