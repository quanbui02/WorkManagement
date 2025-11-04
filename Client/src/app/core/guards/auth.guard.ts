import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private _authService: AuthService,
    private _userService: UserService,
    private _router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const isLoggedIn = this._authService.isLoggedIn();
    if (!isLoggedIn) {
      console.warn('Guard: Chưa đăng nhập, chuyển hướng về /admin/login');
      this._router.navigate(['/admin/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }

    const user = this._authService.getUserInfo();
    const permissions = JSON.parse(sessionStorage.getItem('permissions') || '{}');

    const isSuperUser =
      permissions?.supperUser ||
      String(user?.issuperuser || user?.isSuperUser).toLowerCase() === 'true';

    if (isSuperUser) return true;

    const requiredPerm = route.data?.['permissionRequired'];
    if (requiredPerm) {
      const hasPermission =
        Array.isArray(permissions?.permissions) &&
        permissions.permissions.includes(requiredPerm);

      if (!hasPermission) {
        console.warn('Guard: Không đủ quyền', requiredPerm);
        this._router.navigate(['/error/403']);
        return false;
      }
    }

    return true;
  }
}
