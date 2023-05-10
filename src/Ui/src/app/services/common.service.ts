import { Injectable } from '@angular/core';
import { environment } from "../../environments/environment";
import { HttpClient, HttpParams, HttpResponse } from "@angular/common/http";
import { ApiPaths } from "../models/api-paths";
import { ApiBaseResponseModel } from "../models/api-base-response.model";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) {
  }

  getTags() {
    const url = `${this.baseUrl}/${ApiPaths.GetTags}`;
    return this.httpClient.get<ApiBaseResponseModel>(url);
  }

  getTagsFormName(tags: any) {
    const url = `${this.baseUrl}/${ApiPaths.GetTagsFormName}`;
    return this.httpClient.post<ApiBaseResponseModel>(url, { formDefinitionTagsId: tags });
  }
  getDocumentPdf(orderId, documentId): Observable<HttpResponse<Blob>> {
    const url = `${this.baseUrl}/${ApiPaths.GetDocumentPdf}?orderId=${orderId}&documentId=${documentId}`;
    return this.httpClient.get<Blob>(url, { observe: 'response', responseType: 'blob' as 'json' });
  }
}
