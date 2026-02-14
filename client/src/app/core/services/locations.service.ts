import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import type { LocationResponse } from '../models/event.model';
import type { LocationRequest } from '../models/admin.model';

@Injectable({ providedIn: 'root' })
export class LocationsService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getAll(): Observable<LocationResponse[]> {
    return this.http.get<LocationResponse[]>(`${this.apiUrl}/api/location`);
  }

  create(payload: LocationRequest): Observable<LocationResponse> {
    return this.http.post<LocationResponse>(`${this.apiUrl}/api/location`, payload);
  }

  update(id: number, payload: LocationRequest): Observable<LocationResponse> {
    return this.http.put<LocationResponse>(`${this.apiUrl}/api/location/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/location/${id}`);
  }
}
