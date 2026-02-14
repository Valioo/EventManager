import { Component, inject, signal, ViewChild, ElementRef } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, switchMap, startWith, combineLatest, Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { NotificationsService } from '../../../core/services/notifications.service';
import { EventsService } from '../../../core/services/events.service';
import type { EventNotificationResponse } from '../../../core/models/notification.model';
import type { PaginatedResponse, EventResponse } from '../../../core/models/event.model';

@Component({
  selector: 'app-admin-notifications',
  standalone: true,
  imports: [AsyncPipe, FormsModule],
  templateUrl: './admin-notifications.component.html',
  styleUrl: './admin-notifications.component.scss'
})
export class AdminNotificationsComponent {
  @ViewChild('attachSection') attachSectionRef?: ElementRef<HTMLElement>;

  private readonly notificationsService = inject(NotificationsService);
  private readonly eventsService = inject(EventsService);
  private readonly refresh$ = new Subject<void>();

  protected notifications$ = this.refresh$.pipe(
    startWith(undefined),
    switchMap(() => this.notificationsService.getAll())
  );

  protected events$ = this.refresh$.pipe(
    startWith(undefined),
    switchMap(() => this.eventsService.search({ page: 1, pageSize: 50 }))
  );

  /** Combined data for attach form; emits once notifications and events are loaded. */
  protected attachData$: Observable<{
    notifications: EventNotificationResponse[];
    events: EventResponse[];
    loading: boolean;
  }> = combineLatest([
    this.notifications$.pipe(catchError(() => of<EventNotificationResponse[]>([]))),
    this.events$.pipe(
      map((res) => res?.result ?? []),
      catchError(() => of<EventResponse[]>([]))
    )
  ]).pipe(
    map(([notifications, events]) => ({ notifications, events, loading: false })),
    startWith({ notifications: [], events: [], loading: true })
  );

  protected newDaysPrior = 1;
  protected editingId = signal<number | null>(null);
  protected editDaysPrior = 0;

  protected attachNotificationId: number | null = null;
  protected attachEventId: number | null = null;
  protected attachInProgress = false;

  addNotification(): void {
    this.notificationsService.create({ daysPrior: this.newDaysPrior }).subscribe({
      next: () => {
        this.refresh$.next();
      },
      error: () => alert('Failed to create notification.')
    });
  }

  startEdit(n: EventNotificationResponse): void {
    this.editDaysPrior = n.daysPrior;
    this.editingId.set(n.notificationId);
  }

  cancelEdit(): void {
    this.editingId.set(null);
  }

  saveNotification(n: EventNotificationResponse): void {
    this.notificationsService.update(n.notificationId, { daysPrior: this.editDaysPrior }).subscribe({
      next: () => {
        this.editingId.set(null);
        this.refresh$.next();
      },
      error: () => alert('Failed to update notification.')
    });
  }

  deleteNotification(id: number): void {
    if (!confirm('Delete this notification? It will be removed from all events.')) return;
    this.notificationsService.delete(id).subscribe({
      next: () => this.refresh$.next(),
      error: () => alert('Failed to delete notification.')
    });
  }

  attachToEvent(): void {
    const notificationId = Number(this.attachNotificationId);
    const eventId = Number(this.attachEventId);
    if (Number.isNaN(notificationId) || Number.isNaN(eventId) || notificationId === 0 || eventId === 0) {
      alert('Please select both a notification and an event.');
      return;
    }
    this.attachInProgress = true;
    this.notificationsService
      .attachToEvent({ notificationId, eventId })
      .subscribe({
        next: () => {
          this.attachInProgress = false;
          this.attachNotificationId = null;
          this.attachEventId = null;
          this.refresh$.next();
        },
        error: (err) => {
          this.attachInProgress = false;
          const msg = err?.error?.message ?? err?.message ?? err?.statusText;
          const alreadyAttached = err?.status === 400;
          alert(alreadyAttached
            ? 'Could not attach. This notification may already be attached to this event.'
            : (msg ? `Failed to attach: ${msg}` : 'Failed to attach notification to event.'));
        }
      });
  }

  detachFromEvent(notificationId: number, eventId: number): void {
    this.notificationsService.detachFromEvent({ notificationId, eventId }).subscribe({
      next: () => this.refresh$.next(),
      error: () => alert('Failed to detach notification from event.')
    });
  }

  /** Pre-select this notification and scroll to the attach form. */
  setNotificationForAttach(notificationId: number): void {
    this.attachNotificationId = notificationId;
    this.attachEventId = null;
    this.attachSectionRef?.nativeElement?.scrollIntoView({ behavior: 'smooth', block: 'start' });
  }
}
