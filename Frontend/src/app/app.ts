import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Sidenav } from './layout/sidenav/sidenav';
import { MatSidenavModule } from '@angular/material/sidenav';
import { Header } from './layout/header/header';

@Component({
  selector: 'app-root',
  imports: [MatSidenavModule,Sidenav, Header,RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('backlog-games');
}
