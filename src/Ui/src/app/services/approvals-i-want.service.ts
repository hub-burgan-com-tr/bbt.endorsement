import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {ApiPaths} from "../models/api-paths";
import {ApiBaseResponseModel} from "../models/api-base-response.model";

@Injectable({
  providedIn: 'root'
})
export class ApprovalsIWantService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) {
  }

  getWantApproval(data: any) {
    const url = `${this.baseUrl}/${ApiPaths.WantApproval}`;
    let params = new HttpParams();
    params = params.append('pageNumber', data.pageNumber);
    params = params.append('pageSize', data.pageSize);
    return this.httpClient.get<ApiBaseResponseModel>(url, {params: params});
  }

  getWantApprovalDetail(orderId: any) {
    const url = `${this.baseUrl}/${ApiPaths.WantApprovalDetail}`;
    let params = new HttpParams();
    params = params.append('orderId', orderId);
    return this.httpClient.get<ApiBaseResponseModel>(url, {params: params});
  }

  cancelOrder(orderId: any) {
    const url = `${this.baseUrl}/${ApiPaths.CancelOrder}`;
    return this.httpClient.post<ApiBaseResponseModel>(url, {orderId});
  }
}
