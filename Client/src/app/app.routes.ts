import { Routes } from '@angular/router';

export const routes: Routes = [
{
    path: 'admin/login',
    loadChildren: () =>
      import('./features/admin/admin-login/admin-login.module').then(m => m.AdminLoginModule),
  },
];
