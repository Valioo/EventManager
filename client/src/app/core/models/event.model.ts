export interface CategoryResponse {
  categoryId: number;
  name: string;
}

export interface LocationResponse {
  id: number;
  address: string;
  city: string;
  venueName: string;
}

export interface TagResponse {
  id: number;
  name: string;
}

export interface EventResponse {
  eventId: number;
  title: string;
  description: string;
  startDate: string;
  endDate: string;
  category: CategoryResponse;
  location: LocationResponse;
  tags: TagResponse[];
}

export interface PaginatedResponse<T> {
  result: T[];
  pageSize: number;
  pageNumber: number;
  maximumPages: number;
}

export interface EventSearchParams {
  page?: number;
  pageSize?: number;
  categoryId?: number;
  locationId?: number;
  tagIds?: number[];
}

/** Payload for creating an event (Admin/Organizer) */
export interface CreateEventRequest {
  title: string;
  description: string;
  startDate: string;
  endDate: string;
  categoryId: number;
  locationId: number;
}

/** Payload for updating an event (Admin/Organizer) */
export interface UpdateEventRequest extends CreateEventRequest {}
