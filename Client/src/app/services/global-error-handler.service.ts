import { Injectable, ErrorHandler, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {

  constructor(private injector: Injector) { }

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
        router.navigate(['error400']);
        break;
      }
      case (404): {
        router.navigate(['error404']);
        break;
      }
      case (500): {
        router.navigate(['error500']);
        break;
      }
      default: {
        router.navigate(['error']);
        break;
      }
    }
  }
}
