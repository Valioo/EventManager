import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import type { UserResponse, UpdateUserRequest, PaginationParams } from '../models/admin.model';
import type { PaginatedResponse } from '../models/event.model';

@Injectable({ providedIn: 'root' })
export class UsersService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  get(params: PaginationParams = {}): Observable<PaginatedResponse<UserResponse>> {
    let httpParams = new HttpParams();
    if (params.page != null) httpParams = httpParams.set('PageNumber', params.page);
    if (params.pageSize != null) httpParams = httpParams.set('PageSize', params.pageSize);
    return this.http.get<PaginatedResponse<UserResponse>>(`${this.apiUrl}/api/users`, {
      params: httpParams
    });
  }

  getById(id: number): Observable<UserResponse> {
    return this.http.get<UserResponse>(`${this.apiUrl}/api/users/${id}`);
  }

  update(payload: UpdateUserRequest): Observable<UserResponse> {
    return this.http.put<UserResponse>(`${this.apiUrl}/api/users`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/users/${id}`);
  }
}
