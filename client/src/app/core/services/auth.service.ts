import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import type { LoginRequest, RegisterRequest, AuthResult } from '../models/auth.model';

const TOKEN_KEY = 'eventmanager_token';
const EMAIL_KEY = 'eventmanager_email';
const ROLES_KEY = 'eventmanager_roles';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly apiUrl = environment.apiUrl;

  login(credentials: LoginRequest): Observable<AuthResult> {
    return this.http
      .post<AuthResult>(`${this.apiUrl}/api/auth/login`, credentials)
      .pipe(tap((res) => this.storeAuth(res)));
  }

  register(dto: RegisterRequest): Observable<AuthResult> {
    return this.http
      .post<AuthResult>(`${this.apiUrl}/api/auth/register`, dto)
      .pipe(tap((res) => this.storeAuth(res)));
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(EMAIL_KEY);
    localStorage.removeItem(ROLES_KEY);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  getEmail(): string | null {
    return localStorage.getItem(EMAIL_KEY);
  }

  getRoles(): string[] {
    const raw = localStorage.getItem(ROLES_KEY);
    if (!raw) return [];
    try {
      return JSON.parse(raw) as string[];
    } catch {
      return [];
    }
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  private storeAuth(result: AuthResult): void {
    localStorage.setItem(TOKEN_KEY, result.token);
    localStorage.setItem(EMAIL_KEY, result.email);
    localStorage.setItem(ROLES_KEY, JSON.stringify(result.roles ?? []));
  }
}
