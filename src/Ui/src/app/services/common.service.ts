import { Injectable } from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {ApiPaths} from "../models/api-paths";
import {ApiBaseResponseModel} from "../models/api-base-response.model";

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) {
  }

  getProcessAndState() {
    const url = `${this.baseUrl}/${ApiPaths.GetProcessAndState}`;
    return this.httpClient.get<ApiBaseResponseModel>(url);
  }
}
