import {Injectable} from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree
} from '@angular/router';
import {Observable} from 'rxjs';
import {AuthService} from "../services/auth.service";

@Injectable({
  providedIn: 'root'
})
export class AuthoryGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const currentUser = this.authService.currentUserValue;
    let hasAuthory = true;
    if (route.data.role) {
      for (let name in currentUser.authory) {
        if (name.indexOf(route.data.role) === 0) {
          hasAuthory = currentUser.authory[name];
        }
      }
    }
    if (!hasAuthory) {
      this.router.navigate(['/']);
      return false;
    }
    return true;
  }
}
