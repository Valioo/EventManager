import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';
import { adminOrOrganizerGuard } from './core/guards/admin-or-organizer.guard';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { AdminLayoutComponent } from './layout/admin-layout/admin-layout.component';
import { EventsListComponent } from './features/events/events-list/events-list.component';
import { EventDetailComponent } from './features/events/event-detail/event-detail.component';
import { EventFormComponent } from './features/events/event-form/event-form.component';
import { AdminDashboardComponent } from './features/admin/admin-dashboard/admin-dashboard.component';
import { AdminEventsComponent } from './features/admin/admin-events/admin-events.component';
import { AdminUsersComponent } from './features/admin/admin-users/admin-users.component';
import { AdminCategoriesComponent } from './features/admin/admin-categories/admin-categories.component';
import { AdminLocationsComponent } from './features/admin/admin-locations/admin-locations.component';
import { AdminTagsComponent } from './features/admin/admin-tags/admin-tags.component';
import { AdminNotificationsComponent } from './features/admin/admin-notifications/admin-notifications.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: 'events', component: EventsListComponent },
      { path: 'events/new', component: EventFormComponent, canActivate: [adminOrOrganizerGuard] },
      { path: 'events/:id', component: EventDetailComponent },
      { path: 'events/:id/edit', component: EventFormComponent, canActivate: [adminOrOrganizerGuard] },
      { path: 'notifications', component: AdminNotificationsComponent, canActivate: [adminOrOrganizerGuard] },
      {
        path: 'admin',
        component: AdminLayoutComponent,
        canActivate: [adminGuard],
        children: [
          { path: '', component: AdminDashboardComponent },
          { path: 'users', component: AdminUsersComponent },
          { path: 'events', component: AdminEventsComponent },
          { path: 'categories', component: AdminCategoriesComponent },
          { path: 'locations', component: AdminLocationsComponent },
          { path: 'tags', component: AdminTagsComponent },
          { path: 'notifications', component: AdminNotificationsComponent }
        ]
      },
      { path: '', pathMatch: 'full', redirectTo: 'events' }
    ]
  },
  { path: '**', redirectTo: 'events' }
];
