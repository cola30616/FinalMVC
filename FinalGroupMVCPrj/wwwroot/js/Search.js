const searchResults = document.querySelector('#searchResults');
const searchInput = document.querySelector('#searchInput');

const search = async (searchText) => {
    try {
        const response = await fetch(`/Lesson/Search?searchText=${encodeURIComponent(searchText)}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const data = await response.json();
        searchResults.innerHTML = "";

        for (const item of data) {
            const itemHTML = `<a onclick="clickHandler('${item.fName}')" class="list-group-item list-group-item-action" style="z-index: 100;margin-top: 0;" href="/Lesson/Details/${item.fLessonCourseId}">${item.fName}</a>`;
            console.log(itemHTML)
            searchResults.insertAdjacentHTML('beforeend', itemHTML);
        }        
    } catch (error) {
        console.error('Error:', error);
    }
};

const clickHandler = (searchText) => {
    searchInput.value = searchText;
    searchResults.innerHTML = "";
}