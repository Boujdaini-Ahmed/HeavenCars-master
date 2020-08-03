class Message {
    constructor(username, text, when) {
        this.userName = username;
        this.text = text;
        this.when = when;
    }
}

// userName is declared in razor page.
const username = userName;
const textInput = document.getElementById('messageText');
//const whenInput = document.getElementById('when');
const chat = document.getElementById('chat');
const messagesQueue = [];

document.getElementById('submitButton').addEventListener('click', () => {
    var currentdate = new Date();
    //messageText.innerHTML = // message que j'ai definis, contenu du msg deviens (format du date)
    //    (currentdate.getMonth() + 1) + "/"
    //    + currentdate.getDate() + "/"
    //    + currentdate.getFullYear() + " "
    //    + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })
});


function clearInputField() {
    messagesQueue.push(textInput.value);
    textInput.value = "";
    console.log(this.userName);
    console.log(this.username);
}

function sendMessage() {
    let text = messagesQueue.shift() || "";
    if (text.trim() === "") return;
    
    let when = new Date();
    let message = new Message(username, text, when);
    sendMessageToHub(message);
}

function addMessageToChat(message) {
    let isCurrentUserMessage = message.userName === username;

    let container = document.createElement('div');
    container.className = isCurrentUserMessage ? "container darker" : "container";

    let sender = document.createElement('p');
    sender.className = "sender";
    sender.innerHTML = message.userName;
    let text = document.createElement('p');
    text.innerHTML = message.text;

    let when = document.createElement('span');
    when.className = isCurrentUserMessage ? "time-left" : "time-right";
    var currentdate = new Date();
    //when.innerHTML = 
    //    (currentdate.getMonth() + 1) + "/"
    //    + currentdate.getDate() + "/"
    //    + currentdate.getFullYear() + " "
    //    + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })

    container.appendChild(sender);
    container.appendChild(text);
    container.appendChild(when);
    chat.appendChild(container);
}
