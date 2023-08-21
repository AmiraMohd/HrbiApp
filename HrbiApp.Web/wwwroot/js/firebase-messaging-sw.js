importScripts('firebase-app.js');
importScripts('firebase-messaging.js');
// Initialize Firebase
var firebaseConfig = {
    apiKey: "AIzaSyA2YsIm-TojzVpwaJm8lk-f5gRGyLYeios",
    authDomain: "tazweed-a71ed.firebaseapp.com",
    projectId: "tazweed-a71ed",
    storageBucket: "tazweed-a71ed.appspot.com",
    messagingSenderId: "578926858761",
    appId: "1:578926858761:web:0ea4b38f501a0b6a175ed8",
    measurementId: "G-9PP10YJW8T"
};
firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();
var myEvent = null;
addEventListener('fetch', event => {
    myEvent = event;
    event.waitUntil(async function () {
        // Exit early if we don't have access to the client.
        // Eg, if it's cross-origin.
        if (!event.clientId) return;

       

    }());
});

//onBackgroundMessage(messagings, (payload) => {
//    console.log(' Received background message ', payload);
//    // Customize notification here
//    const notificationTitle = payload.notification.title;
//    const notificationOptions = {
//        body: payload.notification.body,
//        icon: '/Sada Logo.png'
//    };

//    self.registration.showNotification(notificationTitle,
//        notificationOptions);
//});
//messaging.onBackgroundMessage((payload) => {

//    console.log('Message received.onBackgroundMessage ', payload);
    
//    const client = await clients.get(myEvent.clientId);
//    // Exit early if we don't get the client.
//    // Eg, if it closed.
//    if (!client) return;

//    // Send a message to the client.
//    client.postMessage({
//        msg: "Hey I just got a fetch from you!",
//        url: event.request.url
//    });

//});



