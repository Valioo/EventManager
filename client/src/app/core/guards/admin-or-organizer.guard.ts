import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const adminOrOrganizerGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);
  if (!auth.isAuthenticated()) {
    router.navigate(['/login']);
    return false;
  }
  const roles = auth.getRoles();
  const canManage = roles.includes('Administrator') || roles.includes('Organizer');
  if (!canManage) {
    router.navigate(['/events']);
    return false;
  }
  return true;
};
