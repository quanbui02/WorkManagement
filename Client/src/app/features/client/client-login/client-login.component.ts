import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-client-login',
  standalone: false,
  templateUrl: './client-login.component.html',
  styleUrls: ['./client-login.component.scss']
})
export class ClientLoginComponent implements OnInit {
  form!: FormGroup;
  loading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  ngOnInit() {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/app']);
      return;
    }
  }

  async onSubmit() {
    if (this.form.invalid) return;

    this.loading = true;
    this.errorMessage = '';

    const { username, password } = this.form.value;

    this.authService.login(username, password).subscribe({
      next: (res) => {
        if (res?.data?.token) {
          this.authService.saveToken(res.data.token, res.data.expires);
          this.router.navigate(['/app']);
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
