let dataTable; // 這段要記得加，先宣告datatable的變數
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    // html 欄位跟 ajax 的欄位務必相同
    dataTable = $('#memberListTable').DataTable({
        "ajax": { url: '/AdminMember/ListDataJson' },
        "columns": [
            { data: 'memberId', "width": "7%" },
            { data: 'registerDateTime', "width": "12%" },
            { data: 'realName', "width": "8%" },
            { data: 'showName', "width": "10%" },
            { data: 'email', "width": "15%" },
            { data: 'emailVerification', "width": "7%" },
            { data: 'getCampInfo', "width": "8%" },
            { data: 'status', "width": "7%" },
            {
                // 這段是新增及刪除按鈕 ， 刪除用到onclick 事件，觸發下方的Delete
                data: 'memberId',
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
        order: [[0, 'dsc']]


    });
}