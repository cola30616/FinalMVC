document.addEventListener("DOMContentLoaded", async function () {
    //const lessonCourseId = @Model.FLessonCourseId;
    var element = document.querySelector('div[FLessonCourseId]');
    var lessonCourseId = element.getAttribute('FLessonCourseId');
    const url = "/LessonReview/GetEvalList?CourseId=" + lessonCourseId;

    try {
        const response = await fetch(url, {
            method: "GET"
        });

        if (!response.ok) {
            throw new Error("Network response was not ok");
        }

        const data = await response.text();
        document.getElementById("tab5").innerHTML = data;
    } catch (error) {
        console.error("There was a problem with the fetch operation:", error);
    }

    const ratingElements = document.querySelectorAll('.col-3.col-sm-2');


    ratingElements.forEach(function (element) {
        element.addEventListener('click', function () {

            const rating = this.getAttribute('data-rating');


            toggleReviews(rating);
        });
    });

    function toggleReviews(rating) {
       
        const reviewLists = document.querySelectorAll('.review-list');

      
        reviewLists.forEach(function (review) {
        
            const reviewRating = review.querySelector('.ratings-total').textContent.trim();
            const ratingValue = reviewRating.match(/\d+/)[0];

    
            if (ratingValue === rating) {
                review.style.display = 'block';
            } else {
                review.style.display = 'none';
            }
        });
    }

    const showAllButton = document.getElementById('showAllButton');

    showAllButton.addEventListener('click', function () {
       
        const reviewElements = document.querySelectorAll('.review-list');

        reviewElements.forEach(function (element) {
            element.style.display = 'block';
        });

    });

});
