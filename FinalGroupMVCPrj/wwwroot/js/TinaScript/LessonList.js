let dataTable; // 這段要記得加，先宣告datatable的變數

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    // html 欄位跟 ajax 的欄位務必相同
    dataTable = $('#lessonListTable').DataTable({
        "ajax": { url: '/TeacherAdmin/ListDataJson' },
        "columns": [
            { data: 'code', "width": "5%", className:'text-center' },
            { data: 'name', "width": "15%", className: 'text-center' },
            { data: 'filed', "width": "5%", className: 'text-center' },
            {
                data: 'price', "width": "5%", className: 'text-center',
                "render": function (data) {
                    // 將字串轉換為數字，然後格式化為帶有千分號的字串
                    var formattedPrice = parseFloat(data).toLocaleString();
                    // 改日期格式，所有符合條件的 "-" 符號都替換為 "/" 符號，而不僅僅是第一個 "-" 符號，g->global match(js) 
                    return formattedPrice.substring(0, 10).replace(/-/g, '/');
                }
            },
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
                data: null,
                "render": function (data, type, row) {
                    var disableButton = row.status === "開放報名" ? '' : 'disabled';
                    return `<div class="d-flex justify-content-between" role="">
    <a href="/TeacherAdmin/LessonEdit/${row.lessonid}" class="btn btn-primary mx-2 flex-grow-1" lessonid=${row.lessonid} ><i class="fa-solid fa-ellipsis-vertical"></i> 檢視</a>
    <button onclick="calloffCourse(${row.lessonid})" lessonid=${row.lessonid} id="calloff" ${disableButton} class="btn btn-primary mx-2 flex-grow-1"  data-bs-toggle="tooltip" data-bs-placement="bottom" title="取消"><i class="fa-regular fa-rectangle-xmark"></i> 取消</button>
</div>`
                },
                "width": "10%"
            },
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        }
        ,
        order: [[4, 'dsc']]
    });
}