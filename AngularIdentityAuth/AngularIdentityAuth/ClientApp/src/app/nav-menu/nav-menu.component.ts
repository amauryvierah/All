import { Component } from '@angular/core';
import { IdentityService } from './../services/identity.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private identityService: IdentityService) { }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  getUsername(): string | undefined {
    return localStorage.getItem('Identity') ?? undefined;
  }

  logout(): void {
    this.identityService.removeIdentity();
    window.location.href = '/Account/Logout';
  }
}
