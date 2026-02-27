export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  expiration: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}
