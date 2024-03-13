document.addEventListener("DOMContentLoaded", async () => {

    const FOrderDetailIdElement = document.getElementById('FOrderDetailId');
    const FOrderDetailId = parseInt(FOrderDetailIdElement.getAttribute('data-value'));

    //GET部分
    try {
        const response = await fetch(`/LessonReview/CreateEvaluation?FOrderDetailId=${FOrderDetailId}`, {
            method: "GET"
        });

        if (!response.ok) {
            throw new Error(`${response.status}`);
        }

        const data = await response.text();
        document.getElementById("createPartial").innerHTML = data;
    } catch (error) {
        console.error('Error', error);
    }


    //POST部分
    const form = document.querySelector('#CreateForm');
    const scoreElement = document.getElementById('ratingScore');
    const evalComment = document.getElementById('evalContent');

    const cancelBtns = document.querySelectorAll('#CreateCancel');
    const originComment = evalComment.value;
    const originScore = parseInt(scoreElement.innerText);

    form.addEventListener("submit", async (event) => {
        event.preventDefault();

        const score = parseInt(scoreElement.innerText);
        const comment = evalComment.value;

        const url = '/LessonReview/CreateEvaluation';
        const data = {
            FOrderDetailId: FOrderDetailId,
            FScore: score,
            FComment: comment
        };

        try {
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                console.log('Evaluation submitted successfully.');
                const createModal = document.getElementById('evalModal');
                $(createModal).modal('hide');
                document.getElementById('createBtn').style.display = 'none';
                document.getElementById('editBtn').style.display = 'block';

                $(`#EditForm .star[data-value="${score}"]`).trigger('click');

                $(`#EditForm .full[data-value="${score}"]`).trigger('click');

                const comment = evalComment.value;
                document.getElementById('editComment').value = comment;

            } else {
                console.error('Error:', response.statusText);
            }
        } catch (error) {
            console.error('Error:', error);
        }
    });

    cancelBtns.forEach(btn => {
        btn.addEventListener('click', function () {

            scoreElement.innerHTML = originScore;
            evalComment.value = originComment;

            $(`#CreateForm .star[data-value="${originScore}"]`).trigger('click');

            $(`#CreateForm .full[data-value="${originScore}"]`).trigger('click');
        });
    });

});