import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {
  GetApprovalDetailRequestModel,
  GetApprovalPhysicallyDocumentDetailRequestModel,
  GetEndorsementListRequestModel
} from '../models/my-approval';
import {ApiPaths} from "../models/api-paths";
import {environment} from "../../environments/environment";
import {ApiBaseResponseModel} from "../models/api-base-response.model";

@Injectable({
  providedIn: 'root'
})

export class MyApprovalService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) {
  }

  getEndorsementList(data: GetEndorsementListRequestModel) {
    const url = `${this.baseUrl}/${ApiPaths.Endorsement}`;

    let params = new HttpParams();
    params = params.append('pageNumber', data.pageNumber);
    params = params.append('pageSize', data.pageSize);

    return this.httpClient.get<ApiBaseResponseModel>(url, {params: params});
  }

  getApprovalDetail(data: GetApprovalDetailRequestModel) {
    const url = `${this.baseUrl}/${ApiPaths.ApprovalDetail}`;

    let params = new HttpParams();
    params = params.append('orderId', data.orderId);

    return this.httpClient.get<ApiBaseResponseModel>(url, {params: params});
  }

  getApprovalPhysicallyDocumentDetail(data: GetApprovalPhysicallyDocumentDetailRequestModel) {
    const url = `${this.baseUrl}/${ApiPaths.ApprovalPhysicallyDocumentDetail}`;

    let params = new HttpParams();
    params = params.append('orderId', data.orderId);

    return this.httpClient.get<ApiBaseResponseModel>(url, {params: params});
  }

  saveApproveOrderDocument(data: any) {
    const url = `${this.baseUrl}/${ApiPaths.ApproveOrderDocument}`;
    return this.httpClient.post<ApiBaseResponseModel>(url, data);
  }
}
