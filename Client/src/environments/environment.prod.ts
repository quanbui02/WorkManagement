export const environment = {
    production: true,
    hmr: false,
    appMetadata: {
        appDomain: {
            title: 'workmanagement.COM',
            owner: 'workmanagement.COM',
            logo: 'assets/images/mypha.png',
        }
    },
    apiDomain: {
        gateway: 'https://faapi.friends.vn',
        authenticationEndpoint: 'https://faaccount.friends.vn',
        authorizationEndpoint: 'https://fa.friends.vn/authorization.api',
        fileEndpoint: 'https://faapi.friends.vn/file.api',
        notificationEndpoint: 'https://notification.faapi.friends.vn',
        workmanagementEndPoint: 'https://wmdev.id.vn/api',
        dapEInvoiceEndPoint: 'https://localhost:44319',
        logEndPoint: 'https://logs.mypha.vn',
        documentServer: 'https://documentserver.friends.vn',
    },
    clientDomain: {
        appDomain: 'https://fa.friends.vn',
        qthtDomain: 'https://admin.fa.friends.vn',
        idPhanhe: 2,
    },
    authenticationSettings: {
        clientId: 'workmanagement',
        issuer: 'https://faaccount.friends.vn'
    },
    systemLogSetting: {
        enabled: false
    },
    signalr: {
        clientKey: 'workmanagement',
        endpoint: '',
        linkDownloadClientApp: ''
    },
    signalrConfig: {
        hub: {
            notification: 'NotificationHub'
        },
        action: {
            notificationCreated: '',
            viewUpdated: ''
        }
    },
    firebaseConfig: {
        apiKey: 'AIzaSyBSkn5jUqD0N4PiiDEhsWyfPIws6TyNUho',
        authDomain: 'workmanagement-firebase.firebaseapp.com',
        databaseURL: 'https://workmanagement-firebase.firebaseio.com',
        projectId: 'workmanagement-firebase',
        storageBucket: 'workmanagement-firebase.appspot.com',
        messagingSenderId: '1088787441716',
        appId: '1:1088787441716:web:329c0dfd4356195f98c261',
        measurementId: 'G-RVEWV87V1P'
    }
}