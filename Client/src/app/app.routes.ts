import { Routes } from '@angular/router';
import { GuardService } from './core/guards/guard.service';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
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
  { path: '', redirectTo: 'admin/login', pathMatch: 'full' },
  { path: '**', redirectTo: 'admin/login' },
];
