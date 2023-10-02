////*Use this for auth*/ Create models folder with same files
import { Injectable, OnDestroy } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Subscription } from "rxjs";
import { IdentityRetrievalResult } from "../models/identityretrievalresult";
import { IdentityRetrievalCallbacks } from "../models/identityretrievalcallbacks";

@Injectable({
  providedIn: 'root'
})
export class IdentityService implements OnDestroy {

  identityApiService: Subscription | undefined;

  constructor(private http: HttpClient) { }
  
  ngOnDestroy(): void {
    if (this.identityApiService) {
      this.identityApiService.unsubscribe();
    }
  }

  downloadClaims(callback: Function | undefined = undefined): void {
    if (this.identityApiService) {
      this.identityApiService.unsubscribe();
    }
    
    this.identityApiService = this.http.get<IdentityRetrievalResult>('/Account/GetIdentity')
      .subscribe({
        next: (identity: IdentityRetrievalResult) => {
          localStorage.setItem('Identity', identity?.claims?.email ?? '');

          if (callback) {
            callback();
          }
        }
      });
  }

  getIdentity(callbacks: IdentityRetrievalCallbacks | undefined) {
    const identity = localStorage.getItem('Identity');

    if ((!identity || identity.trim() === '') && callbacks?.err) {
      callbacks.err();
    } else if (callbacks?.next) {
      callbacks.next(identity);
    }
  }

  removeIdentity(): void {
    if (localStorage.getItem('Identity')) {
      localStorage.removeItem('Identity');
    }
  }
}
