

    const searchResults = document.querySelector('#searchResults');

    const search = async () => { // 將 search 函數綁定到全局作用域
        try {
            const searchInput = document.querySelector('#inputSearch');
            console.log(searchInput);
            console.log(searchInput.value);
            const response = await fetch(`@Url.Content("~/Lesson/Search")?searchText=${searchInput.value}`);
            console.log(response)
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();
            // 清空搜尋結果區域
            console.log(data)
            searchResults.innerHTML = "";

            // 將搜索結果動態添加到頁面上
            for (const item of data) {
                const itemHTML = `<a class="list-group-item list-group-item-action" href="/Lesson/Details/${item.fLessonCourseId}">${item.fName}</a>`;
                searchResults.insertAdjacentHTML('beforeend', itemHTML);
            }
        } catch (error) {
            console.error('Error:', error);
        }
    };

