import { Component, inject, signal, computed } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Observable, switchMap, shareReplay } from 'rxjs';
import { EventsService } from '../../../core/services/events.service';
import { AuthService } from '../../../core/services/auth.service';
import { EventSubscriptionsService } from '../../../core/services/event-subscriptions.service';
import { EventResponse } from '../../../core/models/event.model';

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [DatePipe, RouterLink, AsyncPipe],
  templateUrl: './event-detail.component.html',
  styleUrl: './event-detail.component.scss'
})
export class EventDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly eventsService = inject(EventsService);
  private readonly auth = inject(AuthService);
  private readonly subscriptionsService = inject(EventSubscriptionsService);

  protected readonly subscribed = signal<boolean>(false);
  protected readonly subscriptionInProgress = signal<boolean>(false);

  protected event$: Observable<EventResponse> = this.route.paramMap.pipe(
    switchMap((params) => this.eventsService.getById(Number(params.get('id')))),
    shareReplay(1)
  );

  constructor() {
    this.event$.subscribe((evt) => this.loadSubscriptionState(evt.eventId));
  }

  protected get canEdit(): boolean {
    const roles = this.auth.getRoles();
    return roles.includes('Administrator') || roles.includes('Organizer');
  }

  protected formatTags(tags: { name: string }[]): string {
    return tags?.map((t) => t.name).join(', ') ?? '';
  }

  protected loadSubscriptionState(eventId: number): void {
    this.subscriptionsService.getAll().subscribe({
      next: (list) => this.subscribed.set(list.some((s) => s.eventId === eventId))
    });
  }

  protected toggleSubscription(eventId: number): void {
    if (this.subscriptionInProgress()) return;
    const currentlySubscribed = this.subscribed();
    this.subscriptionInProgress.set(true);
    const req = currentlySubscribed
      ? this.subscriptionsService.unsubscribe(eventId)
      : this.subscriptionsService.subscribe(eventId);
    req.subscribe({
      next: () => {
        this.subscribed.set(!currentlySubscribed);
        this.subscriptionInProgress.set(false);
      },
      error: () => {
        this.subscriptionInProgress.set(false);
        alert(currentlySubscribed ? 'Failed to unsubscribe.' : 'Failed to subscribe.');
      }
    });
  }
}
