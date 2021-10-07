import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Customer } from 'src/app/dto/customer-list.response.body';
import { RequestStatus } from 'src/app/dto/request-status';
import { BackendApi } from 'src/app/services/http/backend.api';
import { BaseComponent } from "../base.component";

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent extends BaseComponent implements OnInit {

  public customers : Customer[] = []

  constructor(
    router : Router,
    private backendApi : BackendApi) {
    super(router);
  }

  async ngOnInit() {
    try{
      const apiResponse = await this.backendApi.getCustomerList()

      if(apiResponse.requestStatus === RequestStatus.Success){
        this.customers = apiResponse.body.customers
      }
      else{
        alert("Backend servie failed.");
      }
    }
    catch(error){
      this.handleHttpError(error)
    }
  }
}
