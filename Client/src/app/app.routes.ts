import { Routes } from '@angular/router';
import { GuardService } from './core/guards/guard.service';

export const routes: Routes = [
{
    path: 'admin/login',
    loadChildren: () =>
      import('./features/admin/admin-login/admin-login.module').then(m => m.AdminLoginModule),
  },
  {
    path: 'admin',
    loadComponent: () =>
      import('./features/admin/admin.component').then(c => c.AdminComponent),
    canActivate: [GuardService]
  },
];
