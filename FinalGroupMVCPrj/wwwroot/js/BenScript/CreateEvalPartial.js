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

        try {
            const url = '/LessonReview/CreateEvaluation';
            const data = {
                FOrderDetailId: FOrderDetailId,
                FScore: score,
                FComment: comment
            };
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

                // 使用 SweetAlert 提示评价提交成功
                Swal.fire({
                    icon: 'success',
                    title: '評價提交成功',
                    text: '您的評價已成功提交！',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: '確認'
                });

            } else {
                console.error('Error:', response.statusText);
                
                Swal.fire({
                    icon: 'error',
                    title: '評價提交失敗',
                    text: '評價提交失敗，請稍後再試。',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: '確認'
                });
            }
        } catch (error) {
            console.error('Error:', error);
            
            Swal.fire({
                icon: 'error',
                title: '評價提交失敗',
                text: '評價提交失敗，請稍後再試。',
                confirmButtonColor: '#3085d6',
                confirmButtonText: '確認'
            });
        }

        // 更新評價顯示
        try {
            const FOrderDetailId = document.querySelector('#FOrderDetailId').getAttribute('data-value');
            const detailPartial = document.querySelector('#evalDetailPartial');
            const url = "/LessonReview/GetEvalDetail?FOrderDetailId=" + FOrderDetailId;

            const response = await fetch(url, {
                method: "GET",
            });

            if (!response.ok) {
                throw new Error("Network response was not ok");
            }

            const data = await response.text();
            detailPartial.innerHTML = data;
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

    const demo = document.querySelector('#createdemo');
    demo.addEventListener('click', function () {
        let message = "參加這個課程是我投資生涯中最正確的決定之一。課程內容豐富而實用，讓我徹底理解了ETF的運作原理和投資策略。專業的教學團隊不僅給予了我們深入的理論知識，還通過案例分析和實戰演練幫助我們更好地應用所學知識。";
        document.getElementById("evalContent").value = message;
    });
});