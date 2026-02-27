import { Component, signal, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    RouterLink,
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  form = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  hidePassword = signal(true);
  loading = signal(false);
  errorMessage = signal('');

  readonly coverGradients = [
    'linear-gradient(160deg, #1a1a2e 0%, #0f3460 100%)',
    'linear-gradient(160deg, #2d6a4f 0%, #1b4332 100%)',
    'linear-gradient(160deg, #7c3aed 0%, #4c1d95 100%)',
    'linear-gradient(160deg, #b91c1c 0%, #7f1d1d 100%)',
    'linear-gradient(160deg, #1d4ed8 0%, #1e3a8a 100%)',
    'linear-gradient(160deg, #b45309 0%, #92400e 100%)',
    'linear-gradient(160deg, #0f766e 0%, #134e4a 100%)',
    'linear-gradient(160deg, #6d28d9 0%, #4c1d95 100%)',
    'linear-gradient(160deg, #9f1239 0%, #881337 100%)',
    'linear-gradient(160deg, #1e40af 0%, #1e3a8a 100%)',
    'linear-gradient(160deg, #065f46 0%, #064e3b 100%)',
    'linear-gradient(160deg, #5b21b6 0%, #312e81 100%)',
  ];

  togglePassword(): void {
    this.hidePassword.update((v) => !v);
  }

  submit(): void {
    if (this.form.invalid) return;

    this.loading.set(true);
    this.errorMessage.set('');

    const { username, password } = this.form.value;

    this.authService.login(username!, password!).subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: () => {
        this.loading.set(false);
        this.errorMessage.set('Usuario o contraseña incorrectos');
      },
    });
  }
}
