/** User list/detail from API (admin) */
export interface UserResponse {
  id: number;
  fullName: string;
  email: string;
  roles: RoleResponse[];
}

export interface RoleResponse {
  roleId: number;
  name: string;
}

/** Update user (admin) */
export interface UpdateUserRequest {
  userId: number;
  email?: string;
  fullName?: string;
}

export interface PaginationParams {
  page?: number;
  pageSize?: number;
}

/** Category create/update */
export interface CategoryRequest {
  name: string;
}

/** Location create/update - API uses same shape for create and update */
export interface LocationRequest {
  address?: string;
  city?: string;
  venueName?: string;
}

/** Tag create/update */
export interface TagRequest {
  name: string;
}
