let dataTable; // 這段要記得加，先宣告datatable的變數

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    // html 欄位跟 ajax 的欄位務必相同
    dataTable = $('#lessonListTable').DataTable({
        "ajax": { url: '/TeacherAdmin/ListDataJson' },
        "columns": [
            { data: 'fCode', "width": "7%" },
            { data: 'fName', "width": "7%" },
            { data: 'fFiled', "width": "7%" },
            { data: 'fPrice', "width": "7%" },
            { data: 'fLessonDate', "width": "7%" },
            { data: 'fTime', "width": "7%" },
            { data: 'fMaxPeople', "width": "7%" },
            { data: 'fRegPeople', "width": "7%" },
            { data: 'fStatus', "width": "10%" },
            { data: 'fVenueType ', "width": "7%" },          
            {
                // 這段是新增及刪除按鈕 ， 刪除用到onclick 事件，觸發下方的Delete
                data: 'courseid',
                "render": function () {
                    return `<div class="" role="">
                        <button   class="btn btn-primary mx-2" data-bs-toggle="modal" ><i class="fa-solid fa-gear"></i></button>
                        <button   class="btn btn-primary mx-2" data-bs-toggle="modal" ><i class="fa-solid fa-gear"></i></button>
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