import { Component, inject, signal } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, switchMap, startWith } from 'rxjs';
import { CategoriesService } from '../../../core/services/categories.service';
import type { CategoryResponse } from '../../../core/models/event.model';

@Component({
  selector: 'app-admin-categories',
  standalone: true,
  imports: [AsyncPipe, FormsModule],
  templateUrl: './admin-categories.component.html',
  styleUrl: './admin-categories.component.scss'
})
export class AdminCategoriesComponent {
  private readonly categoriesService = inject(CategoriesService);
  private readonly refresh$ = new Subject<void>();

  protected categories$ = this.refresh$.pipe(
    startWith(undefined),
    switchMap(() => this.categoriesService.getAll())
  );

  protected newName = '';
  protected editingId = signal<number | null>(null);
  protected editName = '';

  addCategory(): void {
    const name = this.newName.trim();
    if (!name) return;
    this.categoriesService.create({ name }).subscribe({
      next: () => {
        this.newName = '';
        this.refresh$.next();
      },
      error: () => alert('Failed to add category.')
    });
  }

  startEdit(cat: CategoryResponse): void {
    this.editName = cat.name;
    this.editingId.set(cat.categoryId);
  }

  cancelEdit(): void {
    this.editingId.set(null);
  }

  saveCategory(cat: CategoryResponse): void {
    const name = this.editName.trim();
    if (!name) return;
    this.categoriesService.update(cat.categoryId, { name }).subscribe({
      next: () => {
        this.editingId.set(null);
        this.refresh$.next();
      },
      error: () => alert('Failed to update category.')
    });
  }

  deleteCategory(id: number): void {
    if (!confirm('Delete this category?')) return;
    this.categoriesService.delete(id).subscribe({
      next: () => this.refresh$.next(),
      error: () => alert('Failed to delete category.')
    });
  }
}
