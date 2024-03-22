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
});