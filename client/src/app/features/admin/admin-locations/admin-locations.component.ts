import { Component, inject, signal } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, switchMap, startWith } from 'rxjs';
import { LocationsService } from '../../../core/services/locations.service';
import type { LocationResponse } from '../../../core/models/event.model';
import type { LocationRequest } from '../../../core/models/admin.model';

@Component({
  selector: 'app-admin-locations',
  standalone: true,
  imports: [AsyncPipe, FormsModule],
  templateUrl: './admin-locations.component.html',
  styleUrl: './admin-locations.component.scss'
})
export class AdminLocationsComponent {
  private readonly locationsService = inject(LocationsService);
  private readonly refresh$ = new Subject<void>();

  protected locations$ = this.refresh$.pipe(
    startWith(undefined),
    switchMap(() => this.locationsService.getAll())
  );

  protected newLocation: LocationRequest = { venueName: '', address: '', city: '' };
  protected editingId = signal<number | null>(null);
  protected editForm: LocationRequest = { venueName: '', address: '', city: '' };

  addLocation(): void {
    this.locationsService.create(this.newLocation).subscribe({
      next: () => {
        this.newLocation = { venueName: '', address: '', city: '' };
        this.refresh$.next();
      },
      error: () => alert('Failed to add location.')
    });
  }

  startEdit(loc: LocationResponse): void {
    this.editForm = {
      venueName: loc.venueName ?? '',
      address: loc.address ?? '',
      city: loc.city ?? ''
    };
    this.editingId.set(loc.id);
  }

  cancelEdit(): void {
    this.editingId.set(null);
  }

  saveLocation(loc: LocationResponse): void {
    this.locationsService.update(loc.id, this.editForm).subscribe({
      next: () => {
        this.editingId.set(null);
        this.refresh$.next();
      },
      error: () => alert('Failed to update location.')
    });
  }

  deleteLocation(id: number): void {
    if (!confirm('Delete this location?')) return;
    this.locationsService.delete(id).subscribe({
      next: () => this.refresh$.next(),
      error: () => alert('Failed to delete location.')
    });
  }
}
