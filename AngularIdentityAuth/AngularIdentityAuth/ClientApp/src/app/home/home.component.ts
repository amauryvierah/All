import { Component, OnInit } from '@angular/core';
import { IdentityService } from './../services/identity.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  
  constructor(identityService: IdentityService) { 
    identityService.getIdentity({
      next: (identity: string) => {
        console.log(`Retrieved stored claim: ${identity}`);
      },
      err: () => {
        identityService.downloadClaims(() => {
          console.log(`Downloaded claim: ${localStorage.getItem('Identity')}`);
        });
      }
    })
  }
}
