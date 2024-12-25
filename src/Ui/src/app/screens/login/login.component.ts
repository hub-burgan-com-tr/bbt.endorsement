import { Component, OnInit } from '@angular/core';
import { AuthService } from "../../services/auth.service";
import { Subject, takeUntil } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { environment } from "../../../environments/environment";
import { throttleTime } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  private destroy$ = new Subject();
  submitted = false;
  name = '';
  returnUrl: string;
  code;
  state;

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService) {
    this.route.queryParams.subscribe(params => {
      this.code = params['code'];
      this.returnUrl = params['returnUrl'] || '/';
      if (this.code) {
        this.authService.login(this.code).pipe(
          throttleTime(1000),
          takeUntil(this.destroy$)).subscribe(rest => {
            this.authService.getUserInfo(this.code).pipe(takeUntil(this.destroy$)).subscribe(res => {
              if (res) {
                if (res.isStaff && res.authory.isUIVisible) {
                  this.router.navigate(['approvals-i-want']);
                } else {
                  this.router.navigate(['my-approval']);
                }
              }
            });
          });

      }
    });
  }

  ngOnInit(): void {
  }

  login() {
    window.location.href = environment.loginUrl;
  }
}
