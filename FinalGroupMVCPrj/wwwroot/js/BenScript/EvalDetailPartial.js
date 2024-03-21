document.addEventListener("DOMContentLoaded", async function () {
    /*    const lessonCourseId = @Model.FLessonCourseId;*/
    const FOrderDetailId = document.querySelector('#FOrderDetailId').getAttribute('data-value');
    const detailPartial = document.querySelector('#evalDetailPartial');

    const url = "/LessonReview/GetEvalDetail?FOrderDetailId=" + FOrderDetailId;

    try {
        const response = await fetch(url, {
            method: "GET",
        });

        if (!response.ok) {
            throw new Error("Network response was not ok");
        }

        const data = await response.text();
        detailPartial.innerHTML = data;
    } catch (error) {
        console.error("There was a problem with the fetch operation:", error);
    }
});