document.addEventListener('DOMContentLoaded', function () {
    // 在DOMContentLoaded事件觸發後設置onkeyup事件
    var searchInput = document.getElementById('searchInput');
    searchInput.addEventListener('keydown', searchCourses);
    console.log(searchInput);
    // 定義searchCourses函數
    
});
async function searchCourses() {
    try {
        var searchText = searchInput.value;
        var searchResults = document.getElementById('searchResults');
        console.log(searchText); // 確認是否觸發，檢查searchText的值        
        // 向後端發送搜尋請求
        const response = await fetch(`https://localhost:7031/Lesson/Search?searchText=${searchText}`, {
            method: 'GET'
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const data = await response.json();

        // 清空搜尋結果區域
        searchResults.innerHTML = "";

        // 將搜索結果動態添加到頁面上
        for (const item of data) {
            var itemHTML = `<div><a asp-controller="Lesson" asp-action="Index" asp-route-id="${item.fLessonCourseId}">${item.fName}</a></div>`;
            searchResults.insertAdjacentHTML('beforeend', itemHTML);
        }
    } catch (error) {
        console.error('Error:', error);
    }
}