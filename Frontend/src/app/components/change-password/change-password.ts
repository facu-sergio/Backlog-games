import { Component, signal, inject } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/services/auth.service';
import { MatDivider } from '@angular/material/divider';

const passwordsMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const newPassword = control.get('newPassword')?.value;
  const confirmPassword = control.get('confirmPassword')?.value;
  return newPassword && confirmPassword && newPassword !== confirmPassword
    ? { passwordsMismatch: true }
    : null;
};

@Component({
  selector: 'app-change-password',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    RouterLink,
    MatDivider
  ],
  templateUrl: './change-password.html',
  styleUrl: './change-password.scss',
})
export class ChangePasswordComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  form = this.fb.group(
    {
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
    },
    { validators: passwordsMatchValidator }
  );

  hideCurrentPassword = signal(true);
  hideNewPassword = signal(true);
  hideConfirmPassword = signal(true);
  loading = signal(false);
  successMessage = signal('');
  errorMessage = signal('');

  toggleCurrent(): void { this.hideCurrentPassword.update((v) => !v); }
  toggleNew(): void { this.hideNewPassword.update((v) => !v); }
  toggleConfirm(): void { this.hideConfirmPassword.update((v) => !v); }

  submit(): void {
    if (this.form.invalid) return;

    this.loading.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');

    const { currentPassword, newPassword } = this.form.value;

    this.authService.changePassword(currentPassword!, newPassword!).subscribe({
      next: () => {
        this.loading.set(false);
        this.successMessage.set('Contraseña actualizada correctamente');
        this.form.reset();
      },
      error: () => {
        this.loading.set(false);
        this.errorMessage.set('La contraseña actual es incorrecta');
      },
    });
  }
}
