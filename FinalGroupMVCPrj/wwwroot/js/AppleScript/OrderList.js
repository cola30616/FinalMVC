let dataTable; // 這段要記得加，先宣告datatable的變數

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#OrderListTable').DataTable({
        "ajax": { url: '/TeacherAdmin/ListDataJson' },
        "columns": [
            { data: 'orderID', "width": "5%" },
            { data: 'realName', "width": "5%" },
            { data: 'email', "width": "7%" },
            { data: 'orderDate', "width": "7%" },
            { data: 'name', "width": "7%" },
            { data: 'price', "width": "5%" },
            { data: 'orderValid', "width": "5%" },
            { data: 'modificationDescription', "width": "10%" },
            {
                // 這段是新增及刪除按鈕 ， 刪除用到onclick 事件，觸發下方的Delete
                data: 'orderID',
                "render": function (data) {
                    return `<div class="" role="">
                        <button  orderId=${data}" class="btn btn-primary mx-2" data-bs-toggle="modal" data-bs-target="#staticBackdrop"><i class="fa-solid fa-gear"></i></button>
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