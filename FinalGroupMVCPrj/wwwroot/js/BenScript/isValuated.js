document.addEventListener("DOMContentLoaded", async () => {
    const FOrderDetailIdElement = document.getElementById('FOrderDetailId');
    const FOrderDetailId = parseInt(FOrderDetailIdElement.getAttribute('data-value'));

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
                } else {
                    document.getElementById('editBtn').style.display = 'none';
                }
            } catch (error) {
                console.error('Error', error);
            }
        }
    } catch (error) {
        console.error('Error', error);
    }
});