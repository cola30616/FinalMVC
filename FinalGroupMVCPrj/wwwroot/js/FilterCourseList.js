let paginationAdded = false; // 確認分頁建立過了
const loadCourses = async ( page = 1, pageSize = 9, keyword, fieldId, subjectId, minPrice, maxPrice, sortBy, sortType ) => {
    const urlParams = new URLSearchParams();
    urlParams.append('Page', page);
    urlParams.append('PageSize', pageSize);
    if (keyword) urlParams.append('keyword', keyword);
    if (fieldId) urlParams.append('FieldId', fieldId);
    if (subjectId) urlParams.append('SubjectId', subjectId);
    if (minPrice) urlParams.append('minPrice', minPrice);
    if (maxPrice) urlParams.append('maxPrice', maxPrice);
    if (sortBy) urlParams.append('sortBy', sortBy);
    if (sortType) urlParams.append('sortType', sortType);
    
    const url = `/Lesson/CourseList?${urlParams.toString()}`; 
    console.log(url) 
    
    const response = await fetch(url);
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    const data = await response.json();
    console.log(data)

     
    // 如果分頁尚未新增過，則新增分頁
    if (!paginationAdded) {
        const TotalPage = document.getElementById('course-total-page');
        for (let i = 1; i <= data.totalPages; i++) {
            const itemHTML = `<li class="page-item"><button class="page-link" onclick="pagingHandler(${i})">${i}</button></li>`;
            TotalPage.insertAdjacentHTML('beforeend', itemHTML);
        }
        paginationAdded = true; // 將標記設置為 true，表示已經新增過分頁
    }
   
   
};
const pagingHandler = page => {    
    loadCourses(page);
    console.log('hi')
}


// 頁面載入時，順便載入JS file
loadCourses();

