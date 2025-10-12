export const environment = {
   production: true,
   hmr: false,
   appMetadata: {
      appDomain: {
         title: 'mypha.vn',
         owner: 'mypha.vn',
         logo: 'assets/images/logo-mypha.png',
      }
   },
   apiDomain: {
      gateway: 'https://api.mypha.vn',
      authenticationEndpoint: 'https://account.mypha.vn',
      authorizationEndpoint: 'https://api.mypha.vn/authorization.api',
      fileEndpoint: 'https://api.mypha.vn/file.api',
      notificationEndpoint: 'https://notification.mypha.vn',
      dapFoodEndPoint: 'https://api.mypha.vn/dapfood.api',
      dapEInvoiceEndPoint: 'https://api.mypha.vn/dapeinvoice',
      logEndPoint: 'https://logs.mypha.vn',
      documentServer: 'https://documentserver.friends.vn',
   },
   clientDomain: {
      appDomain: 'https://app.mypha.vn',
      qthtDomain: 'https://admin.mypha.vn',
      idPhanhe: 2,
   },
   authenticationSettings: {
      clientId: 'dapfood',
      issuer: 'https://account.mypha.vn'
   },
   systemLogSetting: {
      enabled: false
   },
   signalr: {
      clientKey: 'dapfood',
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
      authDomain: 'dapfood-firebase.firebaseapp.com',
      databaseURL: 'https://dapfood-firebase.firebaseio.com',
      projectId: 'dapfood-firebase',
      storageBucket: 'dapfood-firebase.appspot.com',
      messagingSenderId: '1088787441716',
      appId: '1:1088787441716:web:329c0dfd4356195f98c261',
      measurementId: 'G-RVEWV87V1P'
   }
}