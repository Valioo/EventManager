import { Component, inject, signal } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, switchMap, startWith } from 'rxjs';
import { TagsService } from '../../../core/services/tags.service';
import type { TagResponse } from '../../../core/models/event.model';

@Component({
  selector: 'app-admin-tags',
  standalone: true,
  imports: [AsyncPipe, FormsModule],
  templateUrl: './admin-tags.component.html',
  styleUrl: './admin-tags.component.scss'
})
export class AdminTagsComponent {
  private readonly tagsService = inject(TagsService);
  private readonly refresh$ = new Subject<void>();

  protected tags$ = this.refresh$.pipe(
    startWith(undefined),
    switchMap(() => this.tagsService.getAll())
  );

  protected newName = '';
  protected editingId = signal<number | null>(null);
  protected editName = '';

  addTag(): void {
    const name = this.newName.trim();
    if (!name) return;
    this.tagsService.create({ name }).subscribe({
      next: () => {
        this.newName = '';
        this.refresh$.next();
      },
      error: () => alert('Failed to add tag.')
    });
  }

  startEdit(tag: TagResponse): void {
    this.editName = tag.name;
    this.editingId.set(tag.id);
  }

  cancelEdit(): void {
    this.editingId.set(null);
  }

  saveTag(tag: TagResponse): void {
    const name = this.editName.trim();
    if (!name) return;
    this.tagsService.update(tag.id, { name }).subscribe({
      next: () => {
        this.editingId.set(null);
        this.refresh$.next();
      },
      error: () => alert('Failed to update tag.')
    });
  }

  deleteTag(id: number): void {
    if (!confirm('Delete this tag?')) return;
    this.tagsService.delete(id).subscribe({
      next: () => this.refresh$.next(),
      error: () => alert('Failed to delete tag.')
    });
  }
}
