import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Subject, takeUntil} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";

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

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService) {
  }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  login() {
    this.submitted = true;
    this.authService.login(this.name).pipe(takeUntil(this.destroy$)).subscribe(res => {
      if (res) {
        this.router.navigate([this.returnUrl]);
      }
    });
  }
}
