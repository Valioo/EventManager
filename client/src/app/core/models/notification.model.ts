export interface EventSubscriptionResponse {
  eventId: number;
  eventTitle: string;
}

export interface EventNotificationResponse {
  notificationId: number;
  daysPrior: number;
  eventIds: number[];
}

export interface CreateNotificationRequest {
  daysPrior: number;
}

export interface UpdateNotificationRequest {
  daysPrior: number;
}

export interface EventNotificationRequest {
  eventId: number;
  notificationId: number;
}
