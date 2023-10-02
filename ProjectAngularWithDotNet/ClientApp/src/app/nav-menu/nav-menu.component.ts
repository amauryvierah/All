import { Component } from '@angular/core';
//*Use this for auth*/
//Also add the services folder
import { IdentityService } from './../services/identity.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  //*Use this for auth*/
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
  //*Use this for auth*/
  logout(): void {
    this.identityService.removeIdentity();
    window.location.href = '/Account/Logout';
  }
}
