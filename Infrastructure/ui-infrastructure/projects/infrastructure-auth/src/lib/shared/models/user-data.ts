export enum UserStatus {
  Normal = 'Normal',
  PendingEmailConfirmation = 'PendingEmailConfirmation',
  Disabled = 'Disabled',
  Blocked = 'Blocked',
}

export interface UserData {
  userId: string;
  fullName: string;
  email: string;
  role: string;
  userStatus: UserStatus;
}
