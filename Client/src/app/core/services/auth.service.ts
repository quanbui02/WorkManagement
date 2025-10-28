import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { environment } from '../../../../environments/environment.local';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'access_token';
  private readonly USER_INFO_KEY = 'user_info';

  constructor(private http: HttpClient) {}

  login(username: string, password: string) {
    return this.http.post<any>(`${environment.apiDomain}/Admin/Login`, { username, password });
  }

  saveToken(token: string) {
    localStorage.setItem(this.TOKEN_KEY, token);
    const decoded: any = jwtDecode(token);
    localStorage.setItem(this.USER_INFO_KEY, JSON.stringify(decoded));
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
