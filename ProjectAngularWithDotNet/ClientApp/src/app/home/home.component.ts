import { Component } from '@angular/core';
//*Use this for auth*/
//Also add the services folder
import { IdentityService } from './../services/identity.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent {
  //*Use this for auth*/
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
