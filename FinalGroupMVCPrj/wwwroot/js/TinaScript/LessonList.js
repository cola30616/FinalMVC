﻿let dataTable; // 這段要記得加，先宣告datatable的變數

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    // html 欄位跟 ajax 的欄位務必相同
    dataTable = $('#lessonListTable').DataTable({
        "ajax": { url: '/TeacherAdmin/ListDataJson' },
        "columns": [
            { data: 'code', "width": "5%", className:'text-center' },
            { data: 'name', "width": "10%", className: 'text-center' },
            { data: 'filed', "width": "8%", className: 'text-center' },
            { data: 'price', "width": "5%", className: 'text-center' },
            {
                data: 'lessonDate',
                "width": "5%",
                className: 'text-center',
                "render": function (data) {
                    // 改日期格式，所有符合條件的 "-" 符號都替換為 "/" 符號，而不僅僅是第一個 "-" 符號，g->global match(js)
                    return data.substring(0, 10).replace(/-/g, '/');
                }
            },
            { data: 'time', "width": "5%", className: 'text-center' },
            { data: 'maxPeople', "width": "5%", className: 'text-center' },
            { data: 'regPeople', "width": "5%", className: 'text-center' },
            { data: 'status', "width": "5%", className: 'text-center' },
            { data: 'venueType', "width": "5%", className: 'text-center' },
            {
                // 這段是新增及刪除按鈕 ， 刪除用到onclick 事件，觸發下方的Delete
                data: 'lessonid',
                "render": function (data) {
                    return `<div class="d-flex justify-content-between" role="">
    <a href="/TeacherAdmin/LessonEdit" class="btn btn-primary mx-2 flex-grow-1" lessonid=${data} ><i class="fa-solid fa-ellipsis-vertical"></i> 檢視</a>
    <button lessonid=${data} class="btn btn-primary mx-2 flex-grow-1" data-bs-toggle="modal"><i class="fa-regular fa-rectangle-xmark"></i> 取消</button>
</div>`
                },
                "width": "15%"
            },
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        }
        //,
        //order: [[0, 'dsc']]
    });
}