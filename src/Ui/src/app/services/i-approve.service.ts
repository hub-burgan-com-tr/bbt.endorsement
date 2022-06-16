import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {ApiPaths} from "../models/api-paths";
import {ApiBaseResponseModel} from "../models/api-base-response.model";

@Injectable({
  providedIn: 'root'
})
export class IApproveService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) {
  }

  getMyApproval(data: any) {
    const url = `${this.baseUrl}/${ApiPaths.MyApproval}`;
    let params = new HttpParams();
    params = params.append('pageNumber', data.pageNumber);
    params = params.append('pageSize', data.pageSize);
    return this.httpClient.get<ApiBaseResponseModel>(url, {params: params});
  }

  getMyApprovalDetail(orderId) {
    const url = `${this.baseUrl}/${ApiPaths.MyApprovalDetail}`;
    let params = new HttpParams();
    params = params.append('orderId', orderId);
    return this.httpClient.get<ApiBaseResponseModel>(url, {params: params});
  }
}
