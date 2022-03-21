import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {ApiPaths} from "../models/api-paths";
import {Observable} from "rxjs";
import NewApprovalOrderForm from "../models/new-approval-order-form";

@Injectable({
  providedIn: 'root'
})
export class NewOrderFormService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) {
  }

  getForm(): Observable<any> {
    const url = `${this.baseUrl}/${ApiPaths.GetForm}`;
    return this.httpClient.get(url);
  }

  getFormContent(id): Observable<any> {
    const url = `${this.baseUrl}/${ApiPaths.GetFormContent}?formDefinitionId=${id}`;
    return this.httpClient.get(url);
  }

  save(data: NewApprovalOrderForm) {
    const url = `${this.baseUrl}/${ApiPaths.CreateOrUpdateForm}`;
    return this.httpClient.post<JSON>(url, data);
  }
}
