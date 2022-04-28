import {Injectable} from '@angular/core';
import {NewApprovalOrder} from "../models/new-approval-order";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {ApiPaths} from "../models/api-paths";
import {ApiBaseResponseModel} from "../models/api-base-response.model";

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

  getOrderFormParameters() {
    const url = `${this.baseUrl}/${ApiPaths.GetOrderFormParameters}`;
    return this.httpClient.get<ApiBaseResponseModel>(url);
  }

  getOrderFormTag() {
    const url = `${this.baseUrl}/${ApiPaths.GetOrderFormTag}`;
    return this.httpClient.get<ApiBaseResponseModel>(url);
  }
}
