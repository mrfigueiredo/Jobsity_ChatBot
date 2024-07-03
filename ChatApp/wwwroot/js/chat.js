"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("roomSelect").addEventListener("change", function (event) {
    var oldRoom = event.target.dataset.previousValue || '';
    var newRoom = event.target.value;
    connection.invoke("ChangeRoom", oldRoom, newRoom).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userId").value;
    var message = document.getElementById("messageInput").value;
    var room = document.getElementById("roomSelect").value;
    connection.invoke("SendMessage", user, message, room).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});