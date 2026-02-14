import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import type { TagResponse } from '../models/event.model';
import type { TagRequest } from '../models/admin.model';

@Injectable({ providedIn: 'root' })
export class TagsService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getAll(): Observable<TagResponse[]> {
    return this.http.get<TagResponse[]>(`${this.apiUrl}/api/tags`);
  }

  create(payload: TagRequest): Observable<TagResponse> {
    return this.http.post<TagResponse>(`${this.apiUrl}/api/tags`, payload);
  }

  update(id: number, payload: TagRequest): Observable<TagResponse> {
    return this.http.put<TagResponse>(`${this.apiUrl}/api/tags/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/tags/${id}`);
  }
}
