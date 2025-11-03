import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-admin-login',
  standalone: false,
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.scss']
})
export class AdminLoginComponent implements OnInit {
  form!: FormGroup;
  loading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    // ✅ Nếu đã đăng nhập thì redirect luôn
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/admin']);
      return;
    }

    this.form = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  async onSubmit() {
    if (this.form.invalid) return;

    this.loading = true;
    this.errorMessage = '';

    const { username, password } = this.form.value;

    this.authService.login(username, password).subscribe({
      next: (res) => {
        console.log(res);
        
        if (res?.data?.token) {
          this.authService.saveToken(res.data.token);
          this.router.navigate(['/admin']);
        } else {
          this.errorMessage = 'Không nhận được token từ máy chủ!';
        }
      },
      error: (err) => {
        console.error('Login error:', err);
        this.errorMessage =
          err?.error?.message || 'Sai tên đăng nhập hoặc mật khẩu!';
      },
      complete: () => (this.loading = false)
    });
  }
}
