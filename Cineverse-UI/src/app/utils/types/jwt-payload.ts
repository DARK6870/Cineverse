export enum UserStatus {
  Normal = 'Normal',
  PendingEmailConfirmation = 'PendingEmailConfirmation',
  Disabled = 'Disabled',
  Blocked = 'Blocked',
}

export interface JwtPayload {
  userId: string;
  fullName: string;
  email: string;
  role: string;
  userStatus: UserStatus;
}
