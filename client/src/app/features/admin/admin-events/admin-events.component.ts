import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Observable, Subject, switchMap, startWith } from 'rxjs';
import { EventsService } from '../../../core/services/events.service';
import type { PaginatedResponse, EventResponse } from '../../../core/models/event.model';

@Component({
  selector: 'app-admin-events',
  standalone: true,
  imports: [AsyncPipe, DatePipe, RouterLink],
  templateUrl: './admin-events.component.html',
  styleUrl: './admin-events.component.scss'
})
export class AdminEventsComponent {
  private readonly eventsService = inject(EventsService);
  private readonly refresh$ = new Subject<void>();

  protected events$: Observable<PaginatedResponse<EventResponse>> = this.refresh$.pipe(
    startWith(undefined),
    switchMap(() => this.eventsService.search({ page: 1, pageSize: 20 }))
  );

  deleteEvent(id: number): void {
    if (!confirm('Delete this event?')) return;
    this.eventsService.delete(id).subscribe({
      next: () => this.refresh$.next(),
      error: () => alert('Failed to delete event.')
    });
  }
}
