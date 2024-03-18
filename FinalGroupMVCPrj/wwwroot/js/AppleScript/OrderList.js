let dataTable; // 這段要記得加，先宣告datatable的變數

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#OrderListTable').DataTable({
        "ajax": { url: '/TeacherAdmin/ListDataJson2' },
        "columns": [
            { data: 'fOrderId', "width": "5%", className: 'text-center' },
            { data: 'fRealName', "width": "5%", className: 'text-center' },
            { data: 'fPhone', "width": "6%", className: 'text-center' },
            { data: 'fEmail', "width": "6%", className: 'text-center' },
            {
                data: 'fOrderDate',
                "width": "7%",
                className: 'text-center',
                 "render": function (data) {
                    return data.substring(0, 10).replace(/-/g, '/');
                }
            },
            { data: 'fName', "width": "10%", className: 'text-center' },
            { data: 'fLessonPrice', "width": "5%", className: 'text-center' },
            {
                data: 'fOrderValid',
                "width": "5%",
                className: 'text-center',
                "render": function (data) {
                    return data==true ? "是" : "否";
                }
            },
            { data: 'fModificationDescription', "width": "10%", className: 'text-center' },
            //{
            //    // 這段是新增及刪除按鈕 ， 刪除用到onclick 事件，觸發下方的Delete
            //    data: 'orderID',
            //    "render": function (data) {
            //        return `<div class="" role="">
            //            <button  orderId=${data}" class="btn btn-primary mx-2" data-bs-toggle="modal" data-bs-target="#staticBackdrop"><i class="fa-solid fa-gear"></i></button>
            //        </div>`
            //    },
            //    "width": "5%"
            //},
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        },
        order: [[0, 'dsc']]


    });
}