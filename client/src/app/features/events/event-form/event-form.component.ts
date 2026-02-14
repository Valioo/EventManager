import { Component, inject, signal, computed } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { forkJoin } from 'rxjs';
import { EventsService } from '../../../core/services/events.service';
import { CategoriesService } from '../../../core/services/categories.service';
import { LocationsService } from '../../../core/services/locations.service';
import { TagsService } from '../../../core/services/tags.service';
import type { EventResponse, CreateEventRequest } from '../../../core/models/event.model';
import type { CategoryResponse, LocationResponse, TagResponse } from '../../../core/models/event.model';

@Component({
  selector: 'app-event-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, AsyncPipe],
  templateUrl: './event-form.component.html',
  styleUrl: './event-form.component.scss'
})
export class EventFormComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly eventsService = inject(EventsService);
  private readonly categoriesService = inject(CategoriesService);
  private readonly locationsService = inject(LocationsService);
  private readonly tagsService = inject(TagsService);

  protected readonly eventId = signal<number | null>(null);
  protected readonly isEdit = computed(() => this.eventId() !== null);
  protected readonly categories$ = this.categoriesService.getAll();
  protected readonly locations$ = this.locationsService.getAll();
  protected readonly tags$ = this.tagsService.getAll();

  /** In edit mode: current tags on the event (updated when we add/remove). */
  protected readonly eventTags = signal<TagResponse[]>([]);
  /** In create mode: tag IDs to attach after creating the event. */
  protected readonly selectedTagIds = signal<number[]>([]);
  protected tagActionInProgress = signal<boolean>(false);

  protected readonly form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', [Validators.required]],
    startDate: ['', Validators.required],
    endDate: ['', Validators.required],
    categoryId: [0, [Validators.required, Validators.min(1)]],
    locationId: [0, [Validators.required, Validators.min(1)]]
  });

  protected error = '';
  protected saving = false;

  constructor() {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        const numId = Number(id);
        this.eventId.set(numId);
        this.eventsService.getById(numId).subscribe({
          next: (evt) => this.patchForm(evt),
          error: () => this.router.navigate(['/events'])
        });
      }
    });
  }

  private patchForm(evt: EventResponse): void {
    const startDate = evt.startDate ? new Date(evt.startDate).toISOString().slice(0, 16) : '';
    const endDate = evt.endDate ? new Date(evt.endDate).toISOString().slice(0, 16) : '';
    this.form.patchValue({
      title: evt.title,
      description: evt.description ?? '',
      startDate,
      endDate,
      categoryId: evt.category?.categoryId ?? 0,
      locationId: evt.location?.id ?? 0
    });
    this.eventTags.set(evt.tags ?? []);
  }

  /** Tags that are not yet on the event (for "Add tag" dropdown in edit mode). */
  protected tagsNotOnEvent(allTags: TagResponse[]): TagResponse[] {
    const onEvent = new Set(this.eventTags().map((t) => t.id));
    return allTags.filter((t) => !onEvent.has(t.id));
  }

  protected onAddTagToEvent(event: Event, availableTags: TagResponse[]): void {
    const select = event.target as HTMLSelectElement;
    const value = select.value;
    if (!value) return;
    const tagId = Number(value);
    const tag = availableTags.find((t) => t.id === tagId);
    if (tag) this.addTagToEvent(tag);
    select.value = '';
  }

  protected addTagToEvent(tag: TagResponse): void {
    const id = this.eventId();
    if (id == null) return;
    this.tagActionInProgress.set(true);
    this.eventsService.attachTag(id, tag.id).subscribe({
      next: () => {
        this.eventTags.update((tags) => [...tags, tag]);
        this.tagActionInProgress.set(false);
      },
      error: () => {
        this.tagActionInProgress.set(false);
        alert('Failed to add tag to event.');
      }
    });
  }

  protected removeTagFromEvent(tagId: number): void {
    const id = this.eventId();
    if (id == null) return;
    this.tagActionInProgress.set(true);
    this.eventsService.removeTag(id, tagId).subscribe({
      next: () => {
        this.eventTags.update((tags) => tags.filter((t) => t.id !== tagId));
        this.tagActionInProgress.set(false);
      },
      error: () => {
        this.tagActionInProgress.set(false);
        alert('Failed to remove tag from event.');
      }
    });
  }

  protected toggleCreateTag(tagId: number, checked: boolean): void {
    this.selectedTagIds.update((ids) =>
      checked ? [...ids, tagId] : ids.filter((id) => id !== tagId)
    );
  }

  protected onSubmit(): void {
    this.error = '';
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const raw = this.form.getRawValue();
    const payload: CreateEventRequest = {
      title: raw.title,
      description: raw.description,
      startDate: new Date(raw.startDate).toISOString(),
      endDate: new Date(raw.endDate).toISOString(),
      categoryId: raw.categoryId,
      locationId: raw.locationId
    };
    this.saving = true;
    const id = this.eventId();
    const req = id
      ? this.eventsService.update(id, payload)
      : this.eventsService.create(payload);
    req.subscribe({
      next: (event) => {
        const tagIds = id == null ? this.selectedTagIds() : [];
        if (tagIds.length === 0) {
          this.saving = false;
          this.router.navigate(['/events', event.eventId]);
          return;
        }
        forkJoin(tagIds.map((tagId) => this.eventsService.attachTag(event.eventId, tagId))).subscribe({
          next: () => {
            this.saving = false;
            this.router.navigate(['/events', event.eventId]);
          },
          error: () => {
            this.saving = false;
            this.error = 'Event created but some tags could not be attached.';
          }
        });
      },
      error: () => {
        this.saving = false;
        this.error = id ? 'Failed to update event.' : 'Failed to create event.';
      }
    });
  }
}
