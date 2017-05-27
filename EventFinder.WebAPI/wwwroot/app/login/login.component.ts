import { Component, OnInit } from '@angular/core';

import { AlertService, AuthenticationService, ErrorHandlerService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'login.component.html',
    selector: 'login'
})
export class LoginComponent implements OnInit {
    model: any = {};
    loading = false;
    email: string;

    constructor(
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private errorHandlerService: ErrorHandlerService
    ) { }

    ngOnInit() {
        this.authenticationService.getCurrentUser().subscribe(user => this.email = (user ? user.email : null));
    }

    login() {
        this.loading = true;
        this.authenticationService.login(this.model.email, this.model.password)
            .subscribe(
            data => {
                this.alertService.success('Login successful');
                this.loading = false;
            },
            error => {
                this.errorHandlerService.handle(error);
                this.loading = false;
            });
    }

    logout() {
        this.authenticationService.logout();
    }
}