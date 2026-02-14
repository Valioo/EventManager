import { Component, inject, HostListener, ViewChild, ElementRef, signal, computed } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { EventsService } from '../../../core/services/events.service';
import { CategoriesService } from '../../../core/services/categories.service';
import { LocationsService } from '../../../core/services/locations.service';
import { TagsService } from '../../../core/services/tags.service';
import { EventSubscriptionsService } from '../../../core/services/event-subscriptions.service';
import type { EventSearchParams } from '../../../core/models/event.model';

@Component({
  selector: 'app-events-list',
  standalone: true,
  imports: [DatePipe, RouterLink, AsyncPipe, FormsModule],
  templateUrl: './events-list.component.html',
  styleUrl: './events-list.component.scss'
})
export class EventsListComponent {
  @ViewChild('tagsDropdown') tagsDropdownRef?: ElementRef<HTMLElement>;

  private readonly eventsService = inject(EventsService);
  private readonly categoriesService = inject(CategoriesService);
  private readonly locationsService = inject(LocationsService);
  private readonly tagsService = inject(TagsService);
  private readonly subscriptionsService = inject(EventSubscriptionsService);

  private readonly searchParams$ = new BehaviorSubject<EventSearchParams>({ page: 1, pageSize: 3 });
  private readonly subscribedEventIds = signal<Set<number>>(new Set());

  protected categories$ = this.categoriesService.getAll();
  protected locations$ = this.locationsService.getAll();
  protected tags$ = this.tagsService.getAll();
  protected events$ = this.searchParams$.pipe(
    switchMap((params) => this.eventsService.search(params))
  );
  protected isSubscribed = computed(() => (eventId: number) => this.subscribedEventIds().has(eventId));
  protected subscriptionInProgress = signal<number | null>(null);

  constructor() {
    this.subscriptionsService.getAll().subscribe({
      next: (list) => this.subscribedEventIds.set(new Set(list.map((s) => s.eventId)))
    });
  }

  protected categoryId: number | null = null;
  protected locationId: number | null = null;
  protected tagIds: number[] = [];
  protected tagsDropdownOpen = false;

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (this.tagsDropdownRef?.nativeElement.contains(event.target as Node)) return;
    this.tagsDropdownOpen = false;
  }

  protected isTagSelected(tagId: number): boolean {
    return this.tagIds.includes(tagId);
  }

  protected toggleTag(tagId: number): void {
    if (this.tagIds.includes(tagId)) {
      this.tagIds = this.tagIds.filter((id) => id !== tagId);
    } else {
      this.tagIds = [...this.tagIds, tagId];
    }
  }

  protected toggleTagsDropdown(): void {
    this.tagsDropdownOpen = !this.tagsDropdownOpen;
  }

  protected applySearch(): void {
    this.searchParams$.next({
      page: 1,
      pageSize: 3,
      ...(this.categoryId != null && { categoryId: this.categoryId }),
      ...(this.locationId != null && { locationId: this.locationId }),
      ...(this.tagIds.length > 0 && { tagIds: [...this.tagIds] })
    });
  }

  protected clearFilters(): void {
    this.categoryId = null;
    this.locationId = null;
    this.tagIds = [];
    this.tagsDropdownOpen = false;
    this.searchParams$.next({ page: 1, pageSize: 3 });
  }

  protected setPage(page: number): void {
    const current = this.searchParams$.value;
    this.searchParams$.next({ ...current, page });
  }

  protected toggleSubscription(eventId: number): void {
    if (this.subscriptionInProgress() !== null) return;
    const subscribed = this.isSubscribed()(eventId);
    this.subscriptionInProgress.set(eventId);
    const req = subscribed
      ? this.subscriptionsService.unsubscribe(eventId)
      : this.subscriptionsService.subscribe(eventId);
    req.subscribe({
      next: () => {
        this.subscribedEventIds.update((set) => {
          const next = new Set(set);
          if (subscribed) next.delete(eventId);
          else next.add(eventId);
          return next;
        });
        this.subscriptionInProgress.set(null);
      },
      error: () => {
        this.subscriptionInProgress.set(null);
        alert(subscribed ? 'Failed to unsubscribe.' : 'Failed to subscribe.');
      }
    });
  }
}
