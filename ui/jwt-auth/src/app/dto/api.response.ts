import { RequestStatus } from "./request-status";

export class ApiResponse {
    public requestStatus: RequestStatus;
    public body: any;

    constructor(requestStatus: RequestStatus, body: any) {
        this.requestStatus = requestStatus;
        this.body = body;
    }
}
