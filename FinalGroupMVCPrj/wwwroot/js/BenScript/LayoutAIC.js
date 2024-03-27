const chatButton = document.getElementById('chatButton');
const chatRoom = document.getElementById('chatRoom');
const sendAIButton = document.getElementById('sendAIButton');
const messageInputAI = document.getElementById('messageInputAI');
const chatContainer = document.querySelector('.chat-container');
const demoButton = document.getElementById('demoAIButton');
let hasClicked = false;
chatButton.addEventListener('click', function () {
    chatRoom.style.display = chatRoom.style.display === 'none' ? 'block' : 'none';

    if (hasClicked) return;
    hasClicked = true;
    const messageElement = document.createElement('div');
    messageElement.classList.add('chat-message', 'from-bot');
    messageElement.innerHTML = `
        <div class="message-text">您好，請問需要什麼幫助嗎?</div>
        <div class="message-time">${getCurrentTime()}</div>
    `;
    chatContainer.appendChild(messageElement);
});

async function sendAIMessage() {

    const messageText = messageInputAI.value.trim();
    if (messageText !== '') {
        const messageElement = document.createElement('div');
        messageElement.classList.add('chat-message', 'from-user');
        messageElement.innerHTML = `
                        <div class="message-text">${messageText}</div>
                        <div class="message-time">${getCurrentTime()}</div>
                    `;

        chatContainer.appendChild(messageElement);

        chatContainer.scrollTop = chatContainer.scrollHeight;

        messageInputAI.value = '';
    }

    try {
        const response = await fetch('/Message/GetReplyByGPT', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(messageText)
        });
        if (response.ok) {
            const answer = await response.text();
            handleResponse(answer);
        } else {
            throw new Error('Failed to fetch response from the server');
        }
    } catch (error) {
        console.error('Error sending message to backend:', error);
    }

}

function handleResponse(response) {

    const messageElement = document.createElement('div');
    messageElement.classList.add('chat-message', 'from-bot');
    messageElement.innerHTML = `
                <div class="message-text">${response}</div>
                <div class="message-time">${getCurrentTime()}</div>
            `;
    chatContainer.appendChild(messageElement);
}

function getCurrentTime() {
    const now = new Date();
    const hours = now.getHours().toString().padStart(2, '0');
    const minutes = now.getMinutes().toString().padStart(2, '0');
    return `${hours}:${minutes}`;
}

sendAIButton.addEventListener('click', function () {
    sendAIMessage();
});

messageInputAI.addEventListener('keypress', function (event) {
    if (event.key === 'Enter') {
        sendAIMessage();
    }
});

demoButton.addEventListener('click', function () {
    const inputmsg = document.querySelector('#messageInputAI');
    inputmsg.value = "我想尋找投資理財的相關課程，請問有甚麼可以推薦的嗎?"
});