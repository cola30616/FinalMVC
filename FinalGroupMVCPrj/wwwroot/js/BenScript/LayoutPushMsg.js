let connectionPsuh;

document.addEventListener('DOMContentLoaded', async () => {
    let responseM;
    let memberIdString = '0';

     try {
          responseM = await fetch('https://localhost:7031/UserInfo/APICurrentMemberId', {
             method: "GET",
         });
     }
     catch (error) {
         console.error('Error:', error);
         return;
    }

     try {
         memberIdString = await responseM.text();
     }
     catch {
         const notificationsItem = document.querySelector('.notification-item');
         if (notificationsItem) {
             notificationsItem.style.display = 'none';
         }
         return;
     }

     if (memberIdString === "0") {
         const notificationsItem = document.querySelector('#bellicon');
         if (notificationsItem) {
             notificationsItem.style.display = 'none';
         }
         return;
     }

    connectionPsuh = new signalR.HubConnectionBuilder().withUrl("/pushMsgHub").build();

    connectionPsuh.start().then(function () {

        connectionPsuh.invoke("SendPushStudentId", memberIdString.toString());
    }).then(function () {
        console.log("Student ID sent to server");
    }).catch(function (error) {
        console.error("Error sending student ID: ", error);
    });


    connectionPsuh.on("ConnectionEstablished", function (message) {
        console.log(message);
    });

    connectionPsuh.on("UploadNF", async function () {
        uploadPushMsg(memberIdString);
    });

    uploadPushMsg(memberIdString);
});
async function uploadPushMsg(memberIdString) {
    try {
        const urlNF = `/Message/GetPushJson?memberId=${memberIdString.toString()}`;
        const responseNF = await fetch(urlNF, {
            method: 'GET',
        });

        if (responseNF.ok) {
            const dataNF = await responseNF.json();
            const notifications = dataNF.data;

            const notificationsMenu = document.querySelector('.dropdown-menu.notifications');
            notificationsMenu.innerHTML = '';

            const header = document.createElement('li');
            header.classList.add('dropdown-header');
            header.style.width = '340px';
            header.style.height = '42px';
            header.innerHTML = `
                    你的通知
                    <i class="fa-solid fa-bell" style="color: #74C0FC;"></i>`;
            notificationsMenu.appendChild(header);

            // 插入分隔線
            const divider1 = document.createElement('li');
            divider1.innerHTML = '<hr class="dropdown-divider">';
            notificationsMenu.appendChild(divider1);

            if (notifications.length === 0) {
                // 如果通知數據為空，插入一行 "尚無推播通知"
                const emptyMessage = document.createElement('li');
                emptyMessage.classList.add('notification-item');
                emptyMessage.textContent = '尚無推播通知';
                notificationsMenu.appendChild(emptyMessage);
            } else {
                // 添加通知項目
                notifications.forEach(notification => {
                    let imageUrl = '0';
                    if (notification.fPushImagePath !== null) {
                        imageUrl = 'data:image/jpeg;base64,' + notification.fPushImagePath;
                    }
                    else {
                        imageUrl = '/images/no-image.jpg';
                    }

                    // 計算時間差
                    const pushTime = new Date(notification.fPushCreatedTime);
                    const currentTime = new Date();
                    const timeDifference = Math.abs(currentTime - pushTime);

                    // 如果在一天以內
                    if (timeDifference < 24 * 60 * 60 * 1000) {
                        const hoursDifference = Math.floor(timeDifference / (1000 * 60 * 60));
                        const minutesDifference = Math.floor((timeDifference % (1000 * 60 * 60)) / (1000 * 60));
                        const secondsDifference = Math.floor((timeDifference % (1000 * 60)) / 1000);

                        let timeAgo = '';
                        if (hoursDifference > 0) {
                            timeAgo = `${hoursDifference} 小時前`;
                        } else if (minutesDifference > 0) {
                            timeAgo = `${minutesDifference} 分鐘前`;
                        } else {
                            timeAgo = `${secondsDifference} 秒前`;
                        }

                        const notificationItem = document.createElement('li');
                        notificationItem.classList.add('notification-item');
                        notificationItem.innerHTML = `
            <div class="notification-container" style="overflow: auto; background-color: transparent;" onmouseover="this.style.backgroundColor='#f0f0f0';" onmouseout="this.style.backgroundColor='transparent';">
                <img src="${imageUrl}" style="float: left; margin-right: 10px;" width="90" height="90" />
                <div>
                    <h6>${notification.fPushContent}</h6>
                    <p>${timeAgo}</p>
                </div>
            </div>`;
                        notificationsMenu.appendChild(notificationItem);
                    } else {
                        // 超過一天，顯示天數
                        const daysDifference = Math.ceil(timeDifference / (1000 * 60 * 60 * 24));

                        const notificationItem = document.createElement('li');
                        notificationItem.classList.add('notification-item');
                        notificationItem.innerHTML = `
            <div class="notification-container" style="overflow: auto; background-color: transparent;" onmouseover="this.style.backgroundColor='#f0f0f0';" onmouseout="this.style.backgroundColor='transparent';">
                <img src="${imageUrl}" style="float: left; margin-right: 10px;" width="90" height="90" />
                <div>
                    <h5>${notification.fPushContent}</h5>
                    <p>${daysDifference} 天前</p>
                </div>
            </div>`;
                        notificationsMenu.appendChild(notificationItem);
                    }
                });



                //更新通知數量標籤
                const badgeElement = document.querySelector('.badge-number');
                badgeElement.textContent = notifications.length;

                if (notifications.length > 0) {
                    badgeElement.style.visibility = 'visible';
                } else {
                    
                    badgeElement.style.visibility = 'hidden';
                }
            }

            // 插入分隔線
            const divider2 = document.createElement('li');
            divider2.innerHTML = '<hr class="dropdown-divider">';
            notificationsMenu.appendChild(divider2);

            // 添加菜單 footer
            const footer = document.createElement('li');
            footer.classList.add('dropdown-footer');
            notificationsMenu.appendChild(footer);

            const circle = document.querySelector('#nfcircle');

            circle.style.visibility = "visible";
        } else {
            console.error('Error:', responseNF.statusText);
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

