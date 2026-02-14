import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import type { RoleResponse } from '../models/admin.model';

@Injectable({ providedIn: 'root' })
export class RolesService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getAll(): Observable<RoleResponse[]> {
    return this.http.get<RoleResponse[]>(`${this.apiUrl}/api/roles`);
  }

  assign(roleId: number, userId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/api/roles/${roleId}/assign/${userId}`, {});
  }

  unassign(roleId: number, userId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/roles/${roleId}/unassign/${userId}`);
  }
}
