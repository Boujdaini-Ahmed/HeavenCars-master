var connection = new signalR.HubConnectionBuilder()
    .withUrl('/Chat')
    .build();

connection.on('receiveMessage', addMessageToChat);

connection.start()
    .catch(error => {
        console.error(error.message);
    });

function sendMessageToHub(message) {
    connection.invoke('sendMessage', message);
}

//(function () {
//    var connection = new signalR.HubConnectionBuilder().withUrl("/Chat").build();

//    connection.start().then(function () {
//        console.log("connected");

//        connection.invoke('getConnectionId')
//            .then(function (connectionId) {
//                sessionStorage.setItem('conectionId', connectionId);
//                // Send the connectionId to controller
//            }).catch(err => console.error(err.toString()));;


//    });

//    $("#sendmessage").click(function () {
//        var connectionId = sessionStorage.getItem('conectionId');
//        connection.invoke("sendMessage", connectionId);
//    });

//    connection.on("ReceiveMessage", function (message) {
//        console.log(message);
//    });

//})();