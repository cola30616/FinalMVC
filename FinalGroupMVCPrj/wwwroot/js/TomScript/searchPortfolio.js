//(async () => {
//    const c = await fetch('@Url.Content("~/Portfolio/Field")');
//    const data = await c.json();
//    console.log(data);
//})();
//const TotalPage = document.getElementById('course-total-page');
//const totalClasses = document.getElementById('totalClasses');
////更揭祘戈
//const FilterSortData = {
//    page: 1,
//    pageSize: 6,
//    keyword: '',
//    fieldId: undefined,
//    subjectName: undefined,
//    minPrice: 100,
//    maxPrice: 2000,
//    sortBy: 'desc',
//    sortType: 'enrollment'
//};

//const handleSubjects = (filterData) => {
//    FilterSortData.fieldId = filterData.fieldId;
//    FilterSortData.subjectName = filterData.subjectName;
//    loadCourses(FilterSortData);
//}
//const handlePriceInput = () => {
//    // 莉程基㎝程蔼基
//    const minPrice = document.getElementById('min').value;
//    const maxPrice = document.getElementById('max').value;

//    // 穝縵匡兵ン穝更揭祘
//    FilterSortData.minPrice = minPrice;
//    FilterSortData.maxPrice = maxPrice;
//    loadCourses(FilterSortData);
//}

//const handleSorting = (value) => {
//    if (value === 'enrollment') {
//        FilterSortData.sortBy = 'enrollment';
//        FilterSortData.sortType = 'desc'; //
//    } else if (value === 'rate') {
//        FilterSortData.sortBy = 'rate';
//        FilterSortData.sortType = 'desc';
//    } else if (value === 'newest') {
//        FilterSortData.sortBy = 'newest';
//        FilterSortData.sortType = 'desc';
//    } else if (value === 'Hot') {
//        FilterSortData.sortBy = 'Hot';
//        FilterSortData.sortType = 'desc';
//    }

//    // 秸ノ loadCourses ㄧ计穝更揭祘
//    loadCourses(FilterSortData);
//}

//const loadCourses = async (FilterSortData) => {
//    const { page = 1, pageSize = 6, keyword, fieldId, subjectName, minPrice = 100, maxPrice = 2000, sortBy, sortType } = FilterSortData;
//    // ネΘurl ㄣ
//    const urlParams = new URLSearchParams();
//    urlParams.append('Page', page);
//    urlParams.append('PageSize', pageSize);
//    if (keyword) urlParams.append('keyword', keyword);
//    if (fieldId) urlParams.append('FieldId', fieldId);
//    if (subjectName) urlParams.append('subjectName', subjectName);
//    if (minPrice) urlParams.append('minPrice', minPrice);
//    if (maxPrice) urlParams.append('maxPrice', maxPrice);
//    if (sortBy) urlParams.append('sortBy', sortBy);
//    if (sortType) urlParams.append('sortType', sortType);

//    let url = `/Lesson/CourseList?${urlParams.toString()}`;
//    console.log(url)

//    const response = await fetch(url);
//    if (!response.ok) {
//        throw new Error('Network response was not ok');
//    }
//    const data = await response.json();
//    console.log(data)
//    addPaging(data)
//    createCourseHtml(data)
//    totalClasses.textContent = data.totalCount


//};

//const addPaging = (data) => {
//    TotalPage.innerHTML = '';
//    for (let i = 1; i <= data.totalPages; i++) {
//        const itemHTML = `<li class="page-item"><button class="page-link" onclick="pagingHandler(${i})">${i}</button></li>`;
//        TotalPage.insertAdjacentHTML('beforeend', itemHTML);
//    }
//}
//// 矪瞶だ
//const pagingHandler = page => {
//    FilterSortData.page = page
//    loadCourses(FilterSortData);
//}

//const createCourseHtml = data => {
//    const createCourseHtml = document.getElementById('createCourseHtml');
//    createCourseHtml.innerHTML = '';
//    for (const item of data.courses) {
//        /* <img class="" src="data:image;base64,@(Convert.ToBase64String(item.imageData))" alt="Image" />*/
//        let html = ''
//        html = `<div class="col-xl-4 col-sm-6">
//    <div class="course-default border radius-md mb-25">
//        <figure class="course-img">
//            <a href="/Lesson/Details/${item.lessonCourse.fLessonCourseId}" title="" target="_self" class="lazy-container ratio ratio-2-3">
//                <img src="data:image/gif;base64,${item.imageData} alt="Image"">                    
//            </a>
//            <div class="hover-show">
//                <a href="/Lesson/Details/${item.lessonCourse.fLessonCourseId}" class="btn btn-md btn-primary rounded-pill" title="Enroll Now" target="_self">ミ箇</a>
//            </div>
//        </figure>
//        <div class="course-details">
//            <div class="p-3">
//                <a href="/Lesson/Details/${item.lessonCourse.fLessonCourseId}" target="_self" title="${item.fieldName}" class="tag font-sm color-primary mb-1">${item.fieldName}/${item.subjectName}</a>
//                <h6 class="course-title lc-2 mb-0">
//                    <a href="/Lesson/Details/${item.lessonCourse.fLessonCourseId}" target="_self" title="Link">
//                        ${item.lessonCourse.fName}
//                    </a>
//                </h6>
//                <div class="authors mt-15">
//                    <div class="author">
//                        <img class="radius-sm" src="/images/avatar-1.jpg" alt="Image">
//                        <span class="font-sm">

//                            <a href="/Teacher/Info/${item.id}" target="_self" title=" ${item.lessonCourse.teacherName}">
//                                 ${item.teacherName}
//                            </a>
//                        </span>
//                    </div>                   
//                </div>
//            </div>
//            <div class="course-bottom-info px-3 py-2">                
//                <span class="font-sm"><i class="fas fa-usd-circle"></i>NT ${item.lessonCourse.fPrice}</span>                               
//            </div>
//        </div>
//    </div>
//</div>`
//        createCourseHtml.insertAdjacentHTML('beforeend', html)
//    }

//}

//// 更抖獽更JS file
//loadCourses(FilterSortData);