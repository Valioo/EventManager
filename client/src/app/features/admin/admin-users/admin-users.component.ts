import { Component, inject, signal } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, switchMap, startWith, Observable } from 'rxjs';
import { UsersService } from '../../../core/services/users.service';
import { RolesService } from '../../../core/services/roles.service';
import type { UserResponse, UpdateUserRequest, RoleResponse } from '../../../core/models/admin.model';
import type { PaginatedResponse } from '../../../core/models/event.model';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [AsyncPipe, FormsModule],
  templateUrl: './admin-users.component.html',
  styleUrl: './admin-users.component.scss'
})
export class AdminUsersComponent {
  private readonly usersService = inject(UsersService);
  private readonly rolesService = inject(RolesService);
  private readonly refresh$ = new Subject<void>();

  protected users$ = this.refresh$.pipe(
    startWith(undefined),
    switchMap(() => this.usersService.get({ page: 1, pageSize: 20 }))
  );

  protected roles$: Observable<RoleResponse[]> = this.rolesService.getAll();

  protected editingId = signal<number | null>(null);
  protected editForm = { fullName: '', email: '' };
  protected roleActionInProgress = signal<string | null>(null);

  rolesNotAssignedTo(user: UserResponse, allRoles: RoleResponse[]): RoleResponse[] {
    const ids = new Set((user.roles ?? []).map((r) => r.roleId));
    return allRoles.filter((r) => !ids.has(r.roleId));
  }

  assignRole(userId: number, roleId: number): void {
    const key = `assign-${userId}-${roleId}`;
    this.roleActionInProgress.set(key);
    this.rolesService.assign(roleId, userId).subscribe({
      next: () => {
        this.roleActionInProgress.set(null);
        this.refresh$.next();
      },
      error: () => {
        this.roleActionInProgress.set(null);
        alert('Failed to assign role.');
      }
    });
  }

  onAddRole(event: Event, userId: number): void {
    const select = event.target as HTMLSelectElement;
    const value = select.value;
    if (!value) return;
    const roleId = Number(value);
    this.assignRole(userId, roleId);
    select.value = '';
  }

  unassignRole(userId: number, roleId: number): void {
    const key = `unassign-${userId}-${roleId}`;
    this.roleActionInProgress.set(key);
    this.rolesService.unassign(roleId, userId).subscribe({
      next: () => {
        this.roleActionInProgress.set(null);
        this.refresh$.next();
      },
      error: () => {
        this.roleActionInProgress.set(null);
        alert('Failed to remove role.');
      }
    });
  }

  startEdit(user: UserResponse): void {
    this.editForm.fullName = user.fullName;
    this.editForm.email = user.email;
    this.editingId.set(user.id);
  }

  cancelEdit(): void {
    this.editingId.set(null);
  }

  saveUser(user: UserResponse): void {
    const payload: UpdateUserRequest = {
      userId: user.id,
      fullName: this.editForm.fullName.trim() || undefined,
      email: this.editForm.email.trim() || undefined
    };
    this.usersService.update(payload).subscribe({
      next: () => {
        this.editingId.set(null);
        this.refresh$.next();
      },
      error: () => alert('Failed to update user.')
    });
  }

  protected formatRoles(roles: RoleResponse[] | undefined): string {
    if (!roles?.length) return '—';
    return roles.map((r) => r.name).join(', ');
  }

  deleteUser(id: number): void {
    if (!confirm('Delete this user? This is a soft delete.')) return;
    this.usersService.delete(id).subscribe({
      next: () => this.refresh$.next(),
      error: () => alert('Failed to delete user.')
    });
  }
}
