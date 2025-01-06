import { Injectable } from '@angular/core';
import { environment } from "../../environments/environment";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { ApiPaths } from "../models/api-paths";
import { BehaviorSubject, map, Observable } from "rxjs";
import { User } from "../models/user";
import { GatewayPaths } from "../models/gateway-paths";
import { Components } from "formiojs";
import password = Components.components.password;

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.baseUrl;
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


  logout() {
    localStorage.clear();
    localStorage.removeItem('currentUser');
    localStorage.removeItem('token');

    this.currentUserSubject.next(null);
    this.tokenSubject.next(null);

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Request-Method': 'OPTIONS'
      })
    };
    const revokeUrl = this.baseUrl + "/token/revoke";
    this.httpClient.options(revokeUrl, httpOptions).subscribe({
      next: () => {
        console.log('Token revoke request sent successfully.');
      },
      error: (err) => {
        console.error('Error revoking token:', err);
      },
      complete: () => {
        location.reload();
      }
    });
  }

}
