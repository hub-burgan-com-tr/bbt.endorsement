import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Subject, takeUntil} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import {environment} from "../../../environments/environment";

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
      this.state = params['state'];
      this.returnUrl = params['returnUrl'] || '/';
      if (this.code && this.state) {
        this.authService.ssoLogin(this.code).pipe(takeUntil(this.destroy$)).subscribe(res => {
          this.authService.login(res.access_token, this.state).pipe(takeUntil(this.destroy$)).subscribe(res => {
            if (res) {
              if (res.isStaff) {
                console.log(res);
                console.log("res");
                console.log("res");

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
