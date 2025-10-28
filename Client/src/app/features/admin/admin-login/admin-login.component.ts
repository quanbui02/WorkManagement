import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-login',
  standalone: false,
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.scss']
})
export class AdminLoginComponent implements OnInit {
  loading = false;
  errorMessage = '';
  form!: FormGroup;

  constructor(
    private authService: AuthService, 
    private fb: FormBuilder, 
    private http: HttpClient, 
    private router: Router
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
        });
  }

 onSubmit() {
    // if (this.form.invalid) return;
    // this.loading = true;
    // this.errorMessage = '';

    // const { username, password } = this.form.value;

    // this.http.post<any>(`${environment.apiDomain}/api/Admin/Login`, { username, password })
    //   .subscribe({
    //     next: (res) => {
    //       if (res && res.token) {
    //         localStorage.setItem('access_token', res.token);
    //         const decoded: any = jwtDecode(res.token);
    //         localStorage.setItem('user_info', JSON.stringify(decoded));
    //         this.router.navigate(['/admin']);
    //       } else {
    //         this.errorMessage = 'Không nhận được token từ máy chủ!';
    //       }
    //     },
    //     error: (err) => {
    //       this.errorMessage = err.error?.message || 'Sai tài khoản hoặc mật khẩu!';
    //     },
    //     complete: () => this.loading = false
    //   });
  }
}
