export class BackendErrorResponse{
    public status! : ErrorStatus
}

export enum ErrorStatus {
    UserNotFound = 'UserNotFound' as any,
    RefreshTokenExpired = 'RefreshTokenExpired' as any,
    InvalidRefreshToken = 'InvalidRefreshToken' as any,
    InvalidUserCredentials = 'InvalidUserCredentials' as any
}