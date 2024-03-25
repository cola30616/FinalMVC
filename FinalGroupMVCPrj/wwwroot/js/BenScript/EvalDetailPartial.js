document.addEventListener("DOMContentLoaded", async function () {
    /*    const lessonCourseId = @Model.FLessonCourseId;*/
    const FOrderDetailId = document.querySelector('#FOrderDetailId').getAttribute('data-value');
    const detailPartial = document.querySelector('#evalDetailPartial');

    //先檢查是否可評價
    try {
        const response = await fetch(`/LessonReview/canEvaluated?FOrderDetailId=${FOrderDetailId}`);
        const data = await response.json();

        if (!data.isValid) {
            document.getElementById('createBtn').style.display = 'none';
            document.getElementById('editBtn').style.display = 'none';
        }
        else {
            try {
                const response = await fetch(`/LessonReview/isEvaluated?FOrderDetailId=${FOrderDetailId}`);
                const data = await response.json();

                if (data.isExisting) {
                    document.getElementById('createBtn').style.display = 'none';
                }
                else {
                    document.getElementById('editBtn').style.display = 'none';
                }
            } catch (error) {
                console.error('Error', error);
            }
        }
    } catch (error) {
        console.error('Error', error);
    }

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