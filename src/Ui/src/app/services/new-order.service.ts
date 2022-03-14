import {Injectable} from '@angular/core';
import {NewApprovalOrder} from "../models/new-approval-order";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {ApiPaths} from "../models/api-paths";

@Injectable({
  providedIn: 'root'
})
export class NewOrderService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) {
  }

  save(data: NewApprovalOrder) {
    const url = `${this.baseUrl}/${ApiPaths.EndorsementOrders}`;
    return this.httpClient.post<JSON>(url, data);
  }

  setModel(model: NewApprovalOrder) {
    localStorage.setItem('newApprovalOrder', JSON.stringify(model));
  }

  getModel() {
    const model = localStorage.getItem('newApprovalOrder');
    if (model)
      return JSON.parse(model);
    return undefined;
  }
}
