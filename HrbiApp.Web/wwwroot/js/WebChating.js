var firebaseConfig = {
    apiKey: "AIzaSyA6u0xr2tRY-ocNDjq1glOBrZ5kX8fRj9k",
    authDomain: "hrbiapp.firebaseapp.com",
    projectId: "hrbiapp",
    storageBucket: "hrbiapp.appspot.com",
    messagingSenderId: "740259280118",
    appId: "1:740259280118:web:56f1faaa02d25a132968d7",
    measurementId: "G-QGPNY84B5M"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);
messaging = firebase.messaging();
messaging.onMessage((payload) => {
    console.log('Message received. ', payload);
    
});
navigator.serviceWorker.addEventListener('message', function (event) {
    //ShowRecivedMessage(event.data.notification.body)
    console.log("Got reply from service worker2: " + JSON.stringify(event.data));
    sweetAlert('Info', 'يوحد تنبيه رجاءً تفده','info');
    var li = document.createElement("li");

    li.innerHTML = ` <li><a class="app-notification__item" href="javascript:;"><span class="app-notification__icon"><span class="fa-stack fa-lg"></span></span>
                  <div>
                    <p class="app-notification__message">`+ event.data.notification.title + `</p>
                    <p class="app-notification__meta">`+ event.data.notification.body + `</p>
                  </div></a></li>`;
    
    //document.getElementById('notificationsList').appendChild(li);
    //var childGuest = document.getElementById('seeAll');
    $('#seeAll').before(li);
    //document.getElementById('notificationsList').insertBefore(childGuest, parentGuest.nextSibling);
});
if ("serviceWorker" in navigator) {
    const wMessaging = firebase.messaging();
    navigator.serviceWorker
        .register("../js/firebase-messaging-sw.js")
        .then(function (registration) {
            console.log("Registration successful, scope is:", registration.scope);
            wMessaging.getToken({ vapidKey: "BLH8UWj5yOPM1pc-_a_aDtSUNwECSm5MYOsE_LNE-ctRa6oSW5GnOvt3HS65NvQPbAsHwadhzvP6_bqXXV6ffb0", serviceWorkerRegistration: registration })
                .then((currentToken) => {
                    if (currentToken) {
                        console.log('current token for client: ', currentToken);
                        $.ajax({
                            url: '/Home/SaveAdminInstanceID?instanceID=' + currentToken,
                            type: 'POST',
                            dataType: 'application/json',
                            crossDomain: true,
                            success: function () { },
                            error: function (xhr) {
                                //sweetAlert("خطأ", "الرجاء اعادة تحميل الصفحة", "error"); 
                            }
                        });
                    } else {
                        console.log('No registration token available. Request permission to generate one.');

                        // shows on the UI that permission is required
                    }
                }).catch((err) => {
                    console.log('An error occurred while retrieving token. ', err);
                    // catch error while creating client token
                });
        })
        .catch(function (err) {
            console.log("Service worker registration failed, error:", err);
        });

    navigator.serviceWorker.register('../js/firebase-messaging-sw.js');
}