const searchResults = document.querySelector('#searchResults');
const searchInput = document.querySelector('#searchInput');

const search = async (searchText) => {
    try {
        const response = await fetch(`/Lesson/Search?searchText=${encodeURIComponent(searchText)}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        
        const data = await response.json();
        const teacherNames = []
        searchResults.innerHTML = "";
        console.log(data)
        const lowerSearchText = searchText.toLowerCase(); // 將搜索文字轉換成小寫
        for (const item of data) {
            const lowerItemName = item.name.toLowerCase(); // 將item.name轉換成小寫
            if (lowerItemName.includes(lowerSearchText)) {
                const date = new Date(item.date);
                const dateFormatter = new Intl.DateTimeFormat('zh-TW', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit'
                });
                const formattedDate = dateFormatter.format(date);
                const itemHTML = `<a onclick="clickHandler('${item.name}')" class="list-group-item list-group-item-action" style="z-index: 100;margin-top: 0;" href="/Lesson/Details/${item.id}">(課程)${item.name}    <i class="fa-solid fa-calendar-days color-primary"></i>${formattedDate}</a>`;
                searchResults.insertAdjacentHTML('beforeend', itemHTML);
            }          
        }
        for (const item of data) {
            const lowerItemName = item.teacherName.toLowerCase(); // 將item.name轉換成小寫
            // 檢查老師名稱是否已經存在於陣列中
            if (!teacherNames.includes(item.teacherName) && lowerItemName.includes(lowerSearchText)) {
                const itemHTML = `<a onclick="clickHandler('${item.teacherName}')" class="list-group-item list-group-item-action" style="z-index: 100;margin-top: 0;" href="/Teacher/Info/${item.teacherId}">(老師)${item.teacherName}</a>`;
                searchResults.insertAdjacentHTML('beforeend', itemHTML);
                // 將老師名稱加入到陣列中
                teacherNames.push(item.teacherName);
            }
        }         
    } catch (error) {
        console.error('Error:', error);
    }
};

const clickHandler = (searchText) => {
    searchInput.value = searchText;
    searchResults.innerHTML = "";
}