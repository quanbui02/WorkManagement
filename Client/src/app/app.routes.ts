import { Routes } from '@angular/router';
import { GuardService } from './core/guards/guard.service';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadChildren: () =>
      import('./features/client/client-login/client-login.module')
        .then(m => m.ClientLoginModule),
  },
  {
    path: 'app',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./features/client/client.module')
        .then(m => m.ClientModule),
  },
  {
    path: 'admin/login',
    loadChildren: () =>
      import('./features/admin/admin-login/admin-login.module')
        .then(m => m.AdminLoginModule),
  },
  {
    path: 'admin',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./features/admin/admin.module')
        .then(m => m.AdminModule),
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];
