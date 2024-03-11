﻿let HtmlAdded = false; // 確認分頁建立過了
let url = '';
//載入課程資料
const FilterSortData = {
    page: 1,
    pageSize: 9,
    keyword: '',
    fieldId: undefined,
    subjectName: undefined,
    minPrice: undefined,
    maxPrice: undefined,
    sortBy: 'desc',
    sortType: 'enrollment'
};

const handleSubjects = (filterData) => {
    FilterSortData.fieldId = filterData.fieldId;
    FilterSortData.subjectName = filterData.subjectName;
    loadCourses(FilterSortData);
}
const handlePriceInput = () => {
    // 獲取最低價格和最高價格
    const minPrice = document.getElementById('min').value;
    const maxPrice = document.getElementById('max').value;

    // 更新篩選條件並重新加載課程列表
    FilterSortData.minPrice = minPrice;
    FilterSortData.maxPrice = maxPrice;
    loadCourses(FilterSortData);
}

const handleSorting = (value) => {
    if (value === 'enrollment') {
        FilterSortData.sortBy = 'enrollment';
        FilterSortData.sortType = 'desc'; //
    } else if (value === 'rate') {
        FilterSortData.sortBy = 'rate';
        FilterSortData.sortType = 'desc'; 
    } else if (value === 'newest') {
        FilterSortData.sortBy = 'newest';
        FilterSortData.sortType = 'desc'; 
    } else if (value === 'Hot') {
        FilterSortData.sortBy = 'Hot';
        FilterSortData.sortType = 'desc'; 
    }

    // 調用 loadCourses 函數重新加載課程列表
    loadCourses(FilterSortData);
}

const loadCourses = async (FilterSortData) => {
    const { page = 1, pageSize = 9 , keyword, fieldId, subjectName, minPrice = 0, maxPrice = 1000, sortBy, sortType } = FilterSortData;
    const urlParams = new URLSearchParams();
    urlParams.append('Page', page);
    urlParams.append('PageSize', pageSize);
    if (keyword) urlParams.append('keyword', keyword);
    if (fieldId) urlParams.append('FieldId', fieldId);    
    if (subjectName) urlParams.append('subjectName', subjectName);
    if (minPrice) urlParams.append('minPrice', minPrice);
    if (maxPrice) urlParams.append('maxPrice', maxPrice);
    if (sortBy) urlParams.append('sortBy', sortBy);
    if (sortType) urlParams.append('sortType', sortType);
    
    url = `/Lesson/CourseList?${urlParams.toString()}`; 
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

const pagingHandler = page => {
    FilterSortData.page = page
    loadCourses(FilterSortData);   
}


// 頁面載入時，順便載入JS file
loadCourses(FilterSortData);

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
