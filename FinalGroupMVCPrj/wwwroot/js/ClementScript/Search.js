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
        for (const item of data) {
            const itemHTML = `<a onclick="clickHandler('${item.name}')" class="list-group-item list-group-item-action" style="z-index: 100;margin-top: 0;" href="/Lesson/Details/${item.id}">${item.name}</a>`;             
            searchResults.insertAdjacentHTML('beforeend', itemHTML);
        }
        for (const item of data) {
            // 檢查老師名稱是否已經存在於陣列中
            if (!teacherNames.includes(item.teacherName)) {
                const itemHTML = `<a onclick="clickHandler('${item.teacherName}')" class="list-group-item list-group-item-action" style="z-index: 100;margin-top: 0;" href="/Teacher/Info/${item.teacherId}">${item.teacherName}</a>`;
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