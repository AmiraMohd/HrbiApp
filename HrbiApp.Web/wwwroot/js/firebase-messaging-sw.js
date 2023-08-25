importScripts('firebase-app.js');
importScripts('firebase-messaging.js');
// Initialize Firebase
var firebaseConfig = {
    apiKey: "AIzaSyA6u0xr2tRY-ocNDjq1glOBrZ5kX8fRj9k",
    authDomain: "hrbiapp.firebaseapp.com",
    projectId: "hrbiapp",
    storageBucket: "hrbiapp.appspot.com",
    messagingSenderId: "740259280118",
    appId: "1:740259280118:web:56f1faaa02d25a132968d7",
    measurementId: "G-QGPNY84B5M"
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



