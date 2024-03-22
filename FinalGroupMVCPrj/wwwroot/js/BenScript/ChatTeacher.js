let connection;

document.addEventListener("DOMContentLoaded", async () => {

    connection = new signalR.HubConnectionBuilder().withUrl("/teacherMsgHub").build();

    const teacherId = document.getElementById("teacherId").getAttribute("teacherId");


    //與Server建立連線
    connection.start().then(function () {
        connection.invoke("SendTeacherId", teacherId)
            .then(function () {
                console.log("Teacher ID sent to server");
            }).catch(function (error) {
                console.error("Error sending teacher ID: ", error);
            });
        // console.log("Hub 連線完成");
    }).catch(function (err) {
        alert('連線錯誤: ' + err.toString());
    });

    connection.on("ConnectionEstablished", function (message) {
        console.log(message);
        //建立連線後向後端請求聊天訊息
        connection.invoke("GetTeacherChatMessages", teacherId)
            .then(function (messages) {
                console.log("Received chat messages:", messages);
                //獲得後將其展示
                displayChatMessages(messages);
            }).catch(function (error) {
                console.error("Error retrieving chat messages: ", error);
            });
    });

    connection.on("UpdContent", function (msg) {
        connection.invoke("GetTeacherChatMessages", teacherId)
            .then(function (messages) {
                console.log("Received chat messages:", messages);
                //獲得後將其展示
                displayChatMessages(messages);
            }).catch(function (error) {
                console.error("Error retrieving chat messages: ", error);
            });
    });
});

function handleKeyPress(event) {
    if (event.keyCode === 13) {
        sendMessage();
        event.preventDefault();
    }
}

function sendMessage() {
    const messageInput = document.getElementById("messageInput");
    const message = messageInput.value.trim();
    if (message === "") {
        return;
    }
    const date = new Date();
    const time = getCurrentTime(date);

    const teacherId = document.getElementById("teacherId").getAttribute("teacherId");
    const memberId = "1";

    connection.invoke("SendMessage", teacherId, memberId, message, date)
        .catch(function (err) {
            alert('錯誤: ' + err.toString());
        });
    messageInput.value = "";

}

function displayChatMessages(messages) {
    const chatBox = document.getElementById("chatBox");
    chatBox.innerHTML = "";
    messages.forEach(function (message) {
        const time = getDisplayTime(new Date(message.fMessageTime));
        let messageDiv = document.createElement("div");
        const ImgSrc = "@Url.Action("GetTeacherPicture", "Message", new { FTeacherId = Model.FTeacherId })"
        if (message.fIsTeacherMsg) {
            messageDiv.className = "message my_message";
            messageDiv.innerHTML = `
                                <div class="d-flex">
                                    <div class="flex-shrink-0">
                                        <ul class="list-inline ms-auto mb-0">
                                            <li class="list-inline-item">
                                                <a href="#" class="avtar avtar-xs btn-link-secondary">
                                                    <ion-icon name="trash-outline" class="delete"></ion-icon>
                                                </a>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="flex-grow-1 mx-3">
                                        <div class="msg-content">
                                                <p class="mb-0" id="msgtxt">${message.fMessage}</p>
                                        </div>
                                            <p class="mb-0 text-muted text-sm msg_time" id="time">${time}</p>
                                    </div>
                                    <div class="flex-shrink-0">
                                        <div class="chat-avtar">
                                            <img class="rounded-circle img-fluid wid-40" style="max-width: 40px; max-height: 40px;" src=${ImgSrc} alt="User image">
                                        </div>
                                    </div>
                                </div>
                            `;
            chatBox.appendChild(messageDiv);
        } else {
            messageDiv.className = "message std_message";
            messageDiv.innerHTML = `
                        <div class="d-flex">
                            <div class="flex-shrink-0">
                                <div class="chat-avtar">
                                    <img class="rounded-circle img-fluid wid-40 " style="max-width: 40px; max-height: 40px;" src="/images/chat/img1.jpg" alt="User image">
                                    <span class=" bg-success border border-light rounded-circle chat-badge"></span>
                                </div>
                            </div>
                            <div class="flex-grow-1 mx-3">
                                <div class="msg-content">
                                        <p class="mb-0" id="msgtxt">${message.fMessage}</p>
                                </div>
                                    <p class="mb-0 text-muted text-sm msg_time" id="time">${time}</p>
                            </div>
                        </div>
                                `;
            chatBox.appendChild(messageDiv);
        }

    });
}

function getCurrentTime(now) {
    let hours = now.getHours();
    let minutes = now.getMinutes();
    let ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    let strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}

function getDisplayTime(date) {
    let hours = date.getHours();
    let minutes = date.getMinutes();
    let ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    let strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}
