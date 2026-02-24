import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Sidenav } from './layout/sidenav/sidenav';
import { MatSidenavModule } from '@angular/material/sidenav';
import { Header } from './layout/header/header';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  imports: [MatSidenavModule, Sidenav, Header, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly authService = inject(AuthService);
}
