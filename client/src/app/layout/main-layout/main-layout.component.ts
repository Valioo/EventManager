import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss'
})
export class MainLayoutComponent {
  protected readonly auth = inject(AuthService);

  get email(): string | null {
    return this.auth.getEmail();
  }

  get isAdmin(): boolean {
    return this.auth.getRoles().includes('Administrator');
  }

  get canManageEvents(): boolean {
    const roles = this.auth.getRoles();
    return roles.includes('Administrator') || roles.includes('Organizer');
  }

  logout(): void {
    this.auth.logout();
  }
}
