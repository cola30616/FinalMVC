let HtmlAdded = false; // 確認分頁建立過了
//載入課程資料
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
    
    // 如果分頁和篩選功能尚未新增過，就會新增一次
    if (!HtmlAdded) {
        // 新增篩選功能
       
        const TotalPage = document.getElementById('course-total-page');
        for (let i = 1; i <= data.totalPages; i++) {
            const itemHTML = `<li class="page-item"><button class="page-link" onclick="pagingHandler(${i})">${i}</button></li>`;
            TotalPage.insertAdjacentHTML('beforeend', itemHTML);
        }
        HtmlAdded = true; // 將標記設置為 true，表示已經新增過分頁
    }
   
   
};



// 模板的bug 暫時棄用
//const createFilterHtml = (data) => {
//    const fieldsSubjects = document.getElementById('fieldsSubjects');
//    for (const item of data.fieldWithSubjects) {
//        const { fieldId, fieldName, subjectNames } = item;
//        let subjectHTML = ''; // 初始化空的 HTML 字符串
//        // 使用 map 方法遍歷 subjectNames 陣列並生成對應的 HTML
//        subjectHTML = subjectNames.map(subjectName => {
//            return `<li><a class="icon-start" href="courses.html" title="link" target="_self"><i class="fal fa-angle-right"></i>${subjectName}<span class="qty"></span></a></li>`;
//        }).join(''); // 將生成的 HTML 字符串使用 join 方法合併成一個字符串
//        const itemHTML = `<li class="list-item list-dropdown">
//            <a class="category-toggle" href="#${fieldId}">${fieldName}<span class="qty"></span></a>
//            <ul class="menu-collapse">
//                ${subjectHTML}  
//            </ul>
//        </li>`;
//        fieldsSubjects.insertAdjacentHTML('beforeend', itemHTML);
//        console.log(itemHTML)
           
//    }
//}

// 分頁預設
const pagingHandler = page => {    
    loadCourses(page);   
}


// 頁面載入時，順便載入JS file
loadCourses();

