import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {ApiPaths} from "../models/api-paths";
import {ApiBaseResponseModel} from "../models/api-base-response.model";

@Injectable({
  providedIn: 'root'
})
export class TracingService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) {
  }

  getWatchApproval(data: any) {
    const url = `${this.baseUrl}/${ApiPaths.WatchApproval}`;
    let params = new HttpParams();
    params = params.append('customer', data.customer);
    params = params.append('approver', data.approver);
    params = params.append('process', data.process);
    params = params.append('state', data.state);
    params = params.append('processNo', data.processNo);
    params = params.append('pageNumber', data.pageNumber);
    params = params.append('pageSize', data.pageSize);
    return this.httpClient.get<ApiBaseResponseModel>(url, {params: params});
  }

  getWatchApprovalDetail(orderId) {
    const url = `${this.baseUrl}/${ApiPaths.WatchApprovalDetail}`;
    let params = new HttpParams();
    params = params.append('orderId', orderId);
    return this.httpClient.get<ApiBaseResponseModel>(url, {params: params});
  }
}
