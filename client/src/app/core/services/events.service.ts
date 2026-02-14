import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import type {
  EventResponse,
  PaginatedResponse,
  EventSearchParams,
  CreateEventRequest,
  UpdateEventRequest
} from '../models/event.model';

@Injectable({ providedIn: 'root' })
export class EventsService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  search(params: EventSearchParams): Observable<PaginatedResponse<EventResponse>> {
    let httpParams = new HttpParams();
    if (params.page != null) httpParams = httpParams.set('pages.PageNumber', params.page);
    if (params.pageSize != null) httpParams = httpParams.set('pages.PageSize', params.pageSize);
    if (params.categoryId != null) httpParams = httpParams.set('request.CategoryId', params.categoryId);
    if (params.locationId != null) httpParams = httpParams.set('request.LocationId', params.locationId);
    if (params.tagIds?.length) {
      params.tagIds.forEach((id, i) => {
        httpParams = httpParams.set(`request.TagIds[${i}]`, id);
      });
    }
    return this.http.get<PaginatedResponse<EventResponse>>(`${this.apiUrl}/api/event`, {
      params: httpParams
    });
  }

  getById(id: number): Observable<EventResponse> {
    return this.http.get<EventResponse>(`${this.apiUrl}/api/event/${id}`);
  }

  create(payload: CreateEventRequest): Observable<EventResponse> {
    return this.http.post<EventResponse>(`${this.apiUrl}/api/event`, payload);
  }

  update(id: number, payload: UpdateEventRequest): Observable<EventResponse> {
    return this.http.put<EventResponse>(`${this.apiUrl}/api/event/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/event/${id}`);
  }

  attachTag(eventId: number, tagId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/api/event/${eventId}/tags/${tagId}`, {});
  }

  removeTag(eventId: number, tagId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/event/${eventId}/tags/${tagId}`);
  }
}
