document.addEventListener("DOMContentLoaded", async () => {
    //get
    const FOrderDetailIdElement = document.getElementById('FOrderDetailId');
    const FOrderDetailId = parseInt(FOrderDetailIdElement.getAttribute('data-value'));

    try {
        const response = await fetch(`/LessonReview/isValuated?FOrderDetailId=${FOrderDetailId}`);
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

    try {
        const response = await fetch(`/LessonReview/EditEvaluation?FOrderDetailId=${FOrderDetailId}`, {
            method: "GET"
        });

        if (!response.ok) {
            throw new Error(`${response.status}`);
        }

        const data = await response.text();
        document.getElementById("editPartial").innerHTML = data;
    } catch (error) {
        console.error('Error', error);
    }

    //POST部分
    const form = document.querySelector('#EditForm');
    const scoreElement = document.getElementById('editRatingScore');
    const editComment = document.getElementById('editComment');
    const cancelBtns = document.querySelectorAll('#EditCancel');
    const editBtn = document.getElementById('editBtn');

    let originComment = editComment.value;
    let originScore = parseInt(scoreElement.innerText);

    form.addEventListener("submit", async (event) => {
        event.preventDefault();

        const score = parseInt(scoreElement.innerText);
        const comment = editComment.value;

        const url = '/LessonReview/EditEvaluation';
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
                const editModal = document.getElementById('editEvalModal');
                $(editModal).modal('hide');


            } else {
                console.error('Error:', response.statusText);
            }
        } catch (error) {
            console.error('Error:', error);
        }
    });

    $(document).ready(function () {
        var starClicked = false;
        let score = 5;

        $.ajax({
            url: '/LessonReview/GetEvalScore',
            method: 'GET',
            data: { FOrderDetailId: FOrderDetailId },
            success: function (response) {

                score = response;
                console.log('Received score:', score);

                const initialScore = score;
                $('#EditForm .rating .star').each(function () {
                    var value = parseInt($(this).find('.full').data('value'));
                    if (value <= initialScore) {
                        $(this).find('.selected').addClass('is-animated pulse');
                        $(this).addClass('star-colour');
                    }
                    else {
                        $(this).removeClass('star-colour');
                    }
                });
            },
            error: function (xhr, status, error) {
                console.error('Failed to get score:', error);
            }
        });

        $('.star').click(function () {
            $(this).children('.selected').addClass('is-animated pulse');

            var target = this;

            setTimeout(function () {
                $(target).children('.selected').removeClass('is-animated pulse');
            }, 1000);

            starClicked = true;
        });

        $('.full').click(function () {
            if (starClicked) {
                setFullStarState(this);
            }
            $(this).closest('.rating').find('.js-score').text($(this).data('value'));
            $(this).find('.js-average').text(parseInt($(this).data('value')));
            $(this).closest('.rating').data('vote', $(this).data('value'));
            calculateAverage();
        });

        $('.full').hover(function () {
            if (!starClicked) {
                setFullStarState(this);
            }
        });
    });

    function updateStarState(target) {
        $(target).parent().prevAll().addClass('animate');
        $(target).parent().prevAll().children().addClass('star-colour');

        $(target).parent().nextAll().removeClass('animate');
        $(target).parent().nextAll().children().removeClass('star-colour');
    }

    function setFullStarState(target) {
        $('.star').removeClass('star-colour');
        $('.star').removeClass('animate');

        $(target).addClass('star-colour');
        $(target).parent().addClass('animate');
        $(target).siblings('.half').addClass('star-colour');

        updateStarState(target);
    }

    function calculateAverage() {
        var totalScore = 0;

        $('.rating').each(function () {
            totalScore += parseInt($(this).data('vote'));
        });

        var average = totalScore / $('.rating').length;
        $('.js-average').text(average.toFixed(1));
    }

    editBtn.addEventListener('click', function () {
        originScore = parseInt(scoreElement.innerText);
        originComment = editComment.value;
    });

    cancelBtns.forEach(btn => {
        btn.addEventListener('click', function () {

            scoreElement.innerText = originScore;
            editComment.value = originComment;

            $(`#EditForm .star[data-value="${originScore}"]`).trigger('click');

            $(`#EditForm .full[data-value="${originScore}"]`).trigger('click');
        });
    });

});