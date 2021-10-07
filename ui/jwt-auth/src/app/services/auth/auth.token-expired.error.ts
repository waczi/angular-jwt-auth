export class RefreshTokenExpiredError extends Error {
    constructor() {
        super();

        Object.setPrototypeOf(this, RefreshTokenExpiredError.prototype);
    }
}
