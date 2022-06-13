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
      console.log(this.code);
      console.log(this.state);
      console.log(this.returnUrl);
      if (this.code && this.state) {
        this.authService.login(this.code, this.state).pipe(takeUntil(this.destroy$)).subscribe(res => {
          console.log(res);
          if (res) {
            this.router.navigate([this.returnUrl]);
          }
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
