import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import type {
  EventNotificationResponse,
  CreateNotificationRequest,
  UpdateNotificationRequest,
  EventNotificationRequest
} from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class NotificationsService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getAll(): Observable<EventNotificationResponse[]> {
    return this.http.get<EventNotificationResponse[]>(`${this.apiUrl}/api/Notifications`);
  }

  create(payload: CreateNotificationRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/api/Notifications`, payload);
  }

  update(id: number, payload: UpdateNotificationRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/api/Notifications/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/Notifications/${id}`);
  }

  attachToEvent(payload: EventNotificationRequest): Observable<void> {
    const body = {
      eventId: Number(payload.eventId),
      notificationId: Number(payload.notificationId)
    };
    return this.http.post<void>(`${this.apiUrl}/api/Notifications/events`, body, {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  detachFromEvent(payload: EventNotificationRequest): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/Notifications/events`, {
      body: payload
    });
  }
}
