let dataTable; // 這段要記得加，先宣告datatable的變數

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#OrderListTable').DataTable({
        "ajax": { url: '/TeacherAdmin/ListDataJson2' },
        "columns": [
            { data: 'fOrderNumber', "width": "5%", className: 'text-center' },
            { data: 'fRealName', "width": "5%", className: 'text-center' },
            { data: 'fPhone', "width": "5%", className: 'text-center' },
            { data: 'fEmail', "width": "4%", },       
            {
                data: 'fOrderDate',
                "width": "10%",
                className: 'text-center',
                 "render": function (data) {
                     const date = new Date(`${data}`);
                     const formatter = new Intl.DateTimeFormat('zh-TW', {
                         year: 'numeric',
                         month: '2-digit',
                         day: '2-digit',
                         hour: '2-digit',
                         minute: '2-digit',
                         second: '2-digit',
                     });
                     const formattedDate = formatter.format(date);
                     return formattedDate;
                }
            },
            { data: 'fName', "width": "11%",  },
            { data: 'fLessonPrice', "width": "5%", className: 'text-center' },
            {
                data: 'fOrderValid',
                "width": "5%",
                className: 'text-center',
                "render": function (data) {
                    return data==true ? "是" : "否";
                }
            },
            { data: 'fModificationDescription', "width": "5%",  },
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