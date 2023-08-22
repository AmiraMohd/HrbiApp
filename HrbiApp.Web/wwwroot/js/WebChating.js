var firebaseConfig = {
    apiKey: "AIzaSyA2YsIm-TojzVpwaJm8lk-f5gRGyLYeios",
    authDomain: "tazweed-a71ed.firebaseapp.com",
    projectId: "tazweed-a71ed",
    storageBucket: "tazweed-a71ed.appspot.com",
    messagingSenderId: "578926858761",
    appId: "1:578926858761:web:0ea4b38f501a0b6a175ed8",
    measurementId: "G-9PP10YJW8T"
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
            wMessaging.getToken({ vapidKey: "BMcYp1p70UOGOd03gF7KIiV-026_aDnD-FO-83onQj_9tq6_CUfriJNRuafs-NG_WQIJVgV2Amgaq_AUh-73Lgw", serviceWorkerRegistration: registration })
                .then((currentToken) => {
                    if (currentToken) {
                        console.log('current token for client: ', currentToken);
                        $.ajax({
                            url: '/users/SaveInstanceID?instanceID=' + currentToken,
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