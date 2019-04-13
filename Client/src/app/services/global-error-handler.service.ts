import { Injectable, ErrorHandler, Injector, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {

  constructor(private ngZone: NgZone, private injector: Injector) { }

  handleError(error: any) {
    const router = this.injector.get(Router);
    console.log(`Request URL: ${router.url}`);

    if (error instanceof HttpErrorResponse) {
      console.error('Backend returned status code:', error.status);
      console.error('Response body:', error.message);
    }
    else {
      console.error('An error occured:', error.message);
    }
    switch (error.status) {
      case (400): {
        this.ngZone.run(() => router.navigate(['error400'])).then();
        break;
      }
      case (404): {
        this.ngZone.run(() => router.navigate(['error404'])).then();
        break;
      }
      case (500): {
        this.ngZone.run(() => router.navigate(['error500'])).then();
        break;
      }
      case (401):{
        break;
      }
      default: {
        this.ngZone.run(() => router.navigate(['error'])).then();
        break;
      }
    }
  }
}
