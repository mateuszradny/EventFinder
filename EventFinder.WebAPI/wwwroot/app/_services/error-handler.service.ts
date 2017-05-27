import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Router } from '@angular/router';

import { AlertService } from './alert.service';
import { AuthenticationService } from './authentication.service';

@Injectable()
export class ErrorHandlerService {
    constructor(
        private alertService: AlertService,
        private authenticationService: AuthenticationService,
        private router: Router) { }

    public handle(errorResponse: Response, keepAfterNavigationChange = false) {
        if (errorResponse.status === 401) {
            this.authenticationService.logout();
            this.alertService.error("Twoja sesja wygasła. Zaloguj się ponownie.", true);
            this.router.navigate(['']);
        }

        let errorMessage = this.toString(errorResponse);
        this.alertService.error(errorMessage, keepAfterNavigationChange);
    }

    private toString(errorResponse: Response): string {
        if (errorResponse.headers.get("content-type").indexOf("text/plain") !== -1)
            return errorResponse.text();

        let errors = errorResponse.json();
        let errorMessage = '';

        for (let errorKey in errors)
            errorMessage += errors[errorKey] + " ";

        return errorMessage;
    }
}