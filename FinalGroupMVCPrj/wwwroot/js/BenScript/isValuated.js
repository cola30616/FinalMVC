document.addEventListener("DOMContentLoaded", async () => {
    const FOrderDetailIdElement = document.getElementById('FOrderDetailId');
    const FOrderDetailId = parseInt(FOrderDetailIdElement.getAttribute('data-value'));

    try {
        const response = await fetch(`/LessonReview/isValuated?FOrderDetailId=${FOrderDetailId}`);
        const data = await response.json();

        if (data.isExisting) {
            document.getElementById('createBtn').style.display = 'none';
            document.getElementById('editBtn').style.display = 'block';
        }
        else {
            document.getElementById('createBtn').style.display = 'block';
            document.getElementById('editBtn').style.display = 'none';
        }
    } catch (error) {
        console.error('Error', error);
    }
});