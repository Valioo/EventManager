import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import type { EventSubscriptionResponse } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class EventSubscriptionsService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getAll(): Observable<EventSubscriptionResponse[]> {
    return this.http.get<EventSubscriptionResponse[]>(`${this.apiUrl}/api/EventSubscriptions`);
  }

  subscribe(eventId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/api/EventSubscriptions`, eventId, {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  unsubscribe(eventId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/EventSubscriptions`, {
      body: eventId,
      headers: { 'Content-Type': 'application/json' }
    });
  }
}
