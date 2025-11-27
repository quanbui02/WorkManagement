import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'access_token';
  private readonly USER_INFO_KEY = 'user_info';

  constructor(private http: HttpClient) {}

login(username: string, password: string) {
  return this.http.post<any>(
    `${environment.apiDomain.workmanagementEndPoint}/Auth/Login`,
    { username, password }
  );
}

saveToken(token: string, expires?: string) {
  const decoded: any = jwtDecode(token);
  const now = new Date();

  const expiresAt = expires
    ? new Date(expires).getTime()
    : decoded.exp
      ? decoded.exp * 1000
      : now.getTime() + 3600 * 1000;

  localStorage.setItem('access_token', token);
  localStorage.setItem('access_token_stored_at', now.getTime().toString());
  localStorage.setItem('expires_at', expiresAt.toString());
  localStorage.setItem('granted_scopes', JSON.stringify(['openid', 'profile', 'email']));
  localStorage.setItem('id_token', token);
  localStorage.setItem('id_token_expires_at', expiresAt.toString());
  localStorage.setItem('id_token_claims_obj', JSON.stringify(decoded));
  localStorage.setItem('nonce', btoa(Math.random().toString()));
  localStorage.setItem('session_state', crypto.randomUUID());
  localStorage.setItem('user_info', JSON.stringify({
    displayName: decoded.fullname || decoded.username,
    email: decoded.email,
    issuperuser: decoded.issuperuser,
    permissions: decoded.permissions,
    userid: decoded.userId || decoded.userid,
    avatar: decoded.avatar,
  }));
  localStorage.setItem('is_authenticated', 'true');

  const sessionKey = crypto.randomUUID();
  sessionStorage.setItem('log_session_key', sessionKey);

  const sessionPermissions = {
    supperUser: decoded.issuperuser === true || String(decoded.issuperuser).toLowerCase() === 'true',
    permissions: decoded.permissions || {}
  };
  sessionStorage.setItem('permissions', JSON.stringify(sessionPermissions));

  console.log('Session values set:', {
    log_session_key: sessionKey,
    permissions: sessionPermissions
  });
}


  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  getUserInfo(): any {
    const json = localStorage.getItem(this.USER_INFO_KEY);
    return json ? JSON.parse(json) : null;
  }

  logout() {
    localStorage.clear();
    sessionStorage.clear();
    window.location.href = '/login';
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;

    try {
      const decoded: any = jwtDecode(token);
      const exp = decoded.exp ? decoded.exp * 1000 : 0;
      return Date.now() < exp;
    } catch {
      return false;
    }
  }
}
