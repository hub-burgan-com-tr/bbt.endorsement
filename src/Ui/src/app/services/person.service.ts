import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {ApiPaths} from "../models/api-paths";
import {ApiBaseResponseModel} from "../models/api-base-response.model";

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) {
  }

  PersonSearch(name: string) {
    const url = `${this.baseUrl}/${ApiPaths.PersonSearch}`;
    let params = new HttpParams();
    params = params.append('name', name);
    return this.httpClient.get<ApiBaseResponseModel>(url, {params: params});
  }

}
