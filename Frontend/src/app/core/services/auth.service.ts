import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../enviroments/enviroment';
import { LoginRequest, LoginResponse, ChangePasswordRequest } from '../interfaces/auth.interfaces';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.apiUrl;
  private http = inject(HttpClient);
  private router = inject(Router);

  private readonly TOKEN_KEY = 'auth_token';
  private readonly EXPIRATION_KEY = 'auth_expiration';

  private _isAuthenticated = signal(this.checkAuth());
  readonly isAuthenticated = this._isAuthenticated.asReadonly();

  private checkAuth(): boolean {
    const token = localStorage.getItem(this.TOKEN_KEY);
    const expiration = localStorage.getItem(this.EXPIRATION_KEY);
    if (!token || !expiration) return false;
    return new Date(expiration) > new Date();
  }

  login(username: string, password: string): Observable<LoginResponse> {
    const body: LoginRequest = { username, password };
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, body).pipe(
      tap((response) => {
        localStorage.setItem(this.TOKEN_KEY, response.token);
        localStorage.setItem(this.EXPIRATION_KEY, response.expiration);
        this._isAuthenticated.set(true);
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.EXPIRATION_KEY);
    this._isAuthenticated.set(false);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  changePassword(currentPassword: string, newPassword: string): Observable<any> {
    const body: ChangePasswordRequest = { currentPassword, newPassword };
    return this.http.put(`${this.apiUrl}/auth/change-password`, body);
  }
}
