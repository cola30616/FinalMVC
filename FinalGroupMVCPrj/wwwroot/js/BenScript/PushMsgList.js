        <!---->
        <!--清空建立Modal內的資料-->
            $(document).ready(function () {
                $('#createPMsgModal').on('hidden.bs.modal', function () {
                    // 清除表單中的值
                    $('#createPushMsg').trigger("reset");
                });
            });

function createPushMessage() {
    // 獲取表單數據
    const pushMsgId = document.getElementById('pushMsgId').value;
    const selectedMembers = []; // 獲取選中的會員數據，您需要根據實際情況來獲取

    // 在此處添加其他必要的表單數據

    try {
        // 執行後端操作，提交表單數據
        const url = '/Message/CreatePushMsg';
        const data = {
            pushMsgId: pushMsgId,
            selectedMembers: selectedMembers
        };

        // 使用 fetch 或其他 AJAX 方法發送數據到後端
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                // 成功後顯示 SweetAlert
                Swal.fire({
                    icon: 'success',
                    title: '成功',
                    text: '提交成功！',
                    confirmButtonText: 'OK'
                }).then(() => {
                    // 成功提示後，重定向到指定的 URL
                    window.location.href = '/Message/PushMsgList';
                });
            })
            .catch(error => {
                console.error('Error:', error);
                // 如果出現錯誤，您可以在這裡處理錯誤，例如顯示錯誤消息
            });
    } catch (error) {
        console.error('Error:', error);
        // 如果出現錯誤，您可以在這裡處理錯誤，例如顯示錯誤消息
    }
}


        <!--圖片顯示切換-->

            document.getElementById('FPushImagePath').addEventListener('change', function (event) {
                previewImage(event);
            });

            function previewImage(event) {
                const input = event.target;
                const reader = new FileReader();
                reader.onload = function () {
                    const dataURL = reader.result;
                    const img = document.getElementById('previewImage');
                    img.src = dataURL;
                };
                reader.readAsDataURL(input.files[0]);
            }

            function selectAllCheckboxes() {
                
                const checkboxes = document.querySelectorAll('.memberCheckbox');
                
                checkboxes.forEach(function (checkbox) {
                    checkbox.checked = true;
                });
            }

            function unselectAllCheckboxes() {
                
                const checkboxes = document.querySelectorAll('.memberCheckbox');
               
                checkboxes.forEach(function (checkbox) {
                    checkbox.checked = false;
                });
            }
            
            function searchMembers() {
                const genderSelect = document.getElementById('genderSelect');
                const minAgeInput = document.getElementById('minAgeInput');
                const maxAgeInput = document.getElementById('maxAgeInput');

                const selectedGender = genderSelect.value;
                const minAge = minAgeInput.value;
                const maxAge = maxAgeInput.value;

                const memberRows = document.querySelectorAll('.memberRow');
                memberRows.forEach(function (row) {
                    const gender = row.getAttribute('data-gender');
                    const age = parseInt(row.getAttribute('data-age'));

                    const genderMatch = (selectedGender === 'all' || gender === selectedGender);
                    const ageMatch = (age >= parseInt(minAge) && age <= parseInt(maxAge));

                    if (genderMatch && ageMatch) {
                        row.style.display = 'table-row';
                    } else {
                        row.style.display = 'none';
                    }
                });
            }

            function resetSearch() {
                const genderSelect = document.getElementById('genderSelect');
                const minAgeInput = document.getElementById('minAgeInput');
                const maxAgeInput = document.getElementById('maxAgeInput');

                genderSelect.value = '全部';
                minAgeInput.value = '1';
                maxAgeInput.value = '100';

                const memberRows = document.querySelectorAll('.memberRow');
                memberRows.forEach(function (row) {
                    row.style.display = 'table-row';
                });
            }

        < !--SignalR部分-->

            let connection;
            let adminId = '1';

            document.addEventListener('DOMContentLoaded', async () => {

                connection = new signalR.HubConnectionBuilder().withUrl("/pushMsgHub").build();

                connection.start().then(function () {
                    connection.invoke("SendAdminId", adminId);
                }).then(function () {
                    console.log("Admin ID sent to server");
                }).catch(function (error) {
                    console.error("Error sending teacher ID: ", error);
                });


                connection.on("ConnectionEstablished", function (message) {
                    console.log(message);

                });

            });


            async function sendPushMessage() {
                const checkboxes = document.querySelectorAll('.memberCheckbox:checked');
                const selectedMembers = [];

                const pushMsgIdElement = document.querySelector('div[PutmsgId]');
                const pushMsgId = parseInt(pushMsgIdElement.getAttribute('PutmsgId'));

                const selectElement = document.querySelector('#pushDelaySelect');
                const pushDelay = parseInt(selectElement.value);

                
                if (checkboxes.length === 0) {
                    Swal.fire({
                        icon: 'error',
                        title: '請選擇要推送的會員',
                        text: '請至少選擇一位會員進行推送',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: '確認'
                    });
                    return; 
                }

               
                const minAgeInput = document.querySelector('#minAgeInput').value;
                if (minAgeInput.trim() === '') {
                    Swal.fire({
                        icon: 'error',
                        title: '請輸入最小年齡',
                        text: '請輸入要推送的會員的最小年齡',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: '確認'
                    });
                    return; 
                }

                const maxAgeInput = document.querySelector('#maxAgeInput').value;
                if (maxAgeInput.trim() === '') {
                    Swal.fire({
                        icon: 'error',
                        title: '請輸入最大年齡',
                        text: '請輸入要推送的會員的最大年齡',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: '確認'
                    });
                    return;
                }

                const minAge = parseInt(minAgeInput);
                const maxAge = parseInt(maxAgeInput);

                if (minAge >= maxAge) {
                    Swal.fire({
                        icon: 'error',
                        title: '年齡範圍錯誤',
                        text: '最小年齡必須小於最大年齡',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: '確認'
                    });
                    return;
                }

                checkboxes.forEach(function (checkbox) {
                    selectedMembers.push(parseInt(checkbox.value));
                });

                try {
                    const url = '/Message/CreatePush';
                    const data = {
                        pushMsgId: pushMsgId,
                        selectedMembers: selectedMembers,
                        pushDelay: pushDelay
                    };
                    const jsonData = JSON.stringify(data);
                    const response = await fetch(url, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'X-Requested-With': 'XMLHttpRequest'
                        },
                        body: jsonData
                    });

                    if (response.ok) {
                        Swal.fire({
                            icon: 'success',
                            title: '推播訊息已成功發送',
                            text: '已成功推播給所選會員！',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: '確認'
                        });
                        // 成功存到資料庫後開始處理推播
                        connection.invoke("SendPushMsg", selectedMembers, pushDelay.toString());
                    } else {
                        console.error('Error:', response.statusText);
                    }
                } catch (error) {
                    console.error('Error:', error);
                }
            }