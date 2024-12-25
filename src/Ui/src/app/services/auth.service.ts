import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {ApiPaths} from "../models/api-paths";
import {BehaviorSubject, map, Observable} from "rxjs";
import {User} from "../models/user";
import {GatewayPaths} from "../models/gateway-paths";
import {Components} from "formiojs";
import password = Components.components.password;

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.baseUrl;
  identityServerUrl = environment.identityServerUrl;
  private currentUserSubject: BehaviorSubject<User>;
  private tokenSubject: BehaviorSubject<string>;
  public currentUser: Observable<User>;
  public token: Observable<string>;

  constructor(private httpClient: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
    this.tokenSubject = new BehaviorSubject<string>(JSON.parse(localStorage.getItem('token')));
    this.token = this.tokenSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  public get tokenValue(): any {
    return this.tokenSubject.value;
  }

  login(code: any) {
    const url = `${this.baseUrl}/${ApiPaths.Login}?code=${code}`;
    console.log('Login URL:', url);
    return this.httpClient.get<User>(url).pipe(map(user => {
      localStorage.setItem('currentUser', JSON.stringify(user));
      // localStorage.setItem('token', JSON.stringify(user.token));
      this.tokenSubject.next(user.token);
      this.currentUserSubject.next(user);
      return user;
    }));
  }
  getUserInfo(code: any) {
    const url = `${this.baseUrl}/${ApiPaths.GetUserInfo}`;
    return this.httpClient.get<User>(url).pipe(map(user => {
      localStorage.setItem('currentUser', JSON.stringify(user));
      // localStorage.setItem('token', JSON.stringify(user.token));
      this.tokenSubject.next(user.token);
      this.currentUserSubject.next(user);
      return user;
    }));
  }

  // ssoLogin(code) {
  //   const url = `${this.identityServerUrl}/${GatewayPaths.connectToken}`;
  //   let header = new HttpHeaders({'Content-Type': 'application/x-www-form-urlencoded'});

  //   let p = new HttpParams()
  //     .append('code', code)
  //     .append('client_id', environment.clientId)
  //     .append('grant_type', environment.grantType)
  //     .append('client_secret', environment.clientSecret)
  //     .append('redirect_uri', environment.redirectUri);

  //   let requestBody = p.toString();

  //   return this.httpClient.post<any>(url, requestBody, {headers: header}).pipe(map(data => {
  //     localStorage.setItem('token', JSON.stringify(data.access_token));
  //     this.tokenSubject.next(data.access_token);
  //     return data;
  //   }));
  //   ;
  // }

  logout() {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
    this.tokenSubject.next(null);
    location.reload();
  }
}
