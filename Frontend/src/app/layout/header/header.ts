import { Component, Input, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenav } from '@angular/material/sidenav';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-header',
  imports: [MatIcon, RouterLink, MatButtonModule],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
  private authService = inject(AuthService);

  @Input() sidenav?: MatSidenav;
  @Input() isMobile = false;

  logout(): void {
    this.authService.logout();
  }
}
