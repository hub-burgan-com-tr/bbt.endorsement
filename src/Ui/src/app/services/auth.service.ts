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

    const revokeUrl = this.baseUrl + "/ebanking/revoke";
    this.httpClient.put(revokeUrl, {}).subscribe({
      next: () => console.log('Token revoked successfully.'),
      error: (err) => console.error('Error revoking token:', err),
      complete: () => {
        this.clearAllCookies();
        location.reload();
      }
    });

  }
  clearAllCookies() {
    const cookieName = '.amorphie.token';
    const domains = [
      window.location.hostname,           // Mevcut domain
      '.' + window.location.hostname,     // Alt domainler
      '.burgan.com.tr',                   // Özel domain
    ];
    for (const domain of domains) {
      document.cookie = `${cookieName}=; Path=/; Domain=${domain}; Expires=Thu, 01 Jan 1970 00:00:00 UTC;`;
    }

    console.log('.amorphie.token çerezi silindi!');


    const cookies = document.cookie.split(";"); // Çerezleri al
    for (let i = 0; i < cookies.length; i++) {
      const cookie = cookies[i];
      const eqPos = cookie.indexOf("="); // Çerez adını ayır
      const name = eqPos > -1 ? cookie.substring(0, eqPos).trim() : cookie.trim();

      document.cookie = `${name}=; Path=/; Expires=Thu, 01 Jan 1970 00:00:00 UTC;`;
      document.cookie = `${name}=; Path=/; Domain=${window.location.hostname}; Expires=Thu, 01 Jan 1970 00:00:00 UTC;`;

      const domainParts = window.location.hostname.split(".");
      while (domainParts.length > 1) {
        const domain = domainParts.join(".");
        document.cookie = `${name}=; Path=/; Domain=.${domain}; Expires=Thu, 01 Jan 1970 00:00:00 UTC;`;
        domainParts.shift(); // Bir üst domain'e geç
      }
    }

    sessionStorage.clear();
    localStorage.clear();
    console.log("Tüm çerezler ve oturum verileri temizlendi!");
  }


}
