export class CustomerListResponseBody {
    public customers!: Customer[];
}

export class Customer {
    public firstName! : string;
    public lastName! : string;
    public email! : string;
}
