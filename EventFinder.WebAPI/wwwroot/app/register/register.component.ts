import { Component, OnInit } from '@angular/core';

import { AlertService, AuthenticationService, UserService, ErrorHandlerService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'register.component.html',
    selector: 'register'
})
export class RegisterComponent implements OnInit {
    model: any = {};
    loading = false;
    isLoggedIn: boolean;

    constructor(
        private userService: UserService,
        private alertService: AlertService,
        private authenticationService: AuthenticationService,
        private errorHandlerService: ErrorHandlerService
    ) { }

    ngOnInit() {
        this.authenticationService.getCurrentUser().subscribe(user => this.isLoggedIn = user != null);
    }

    register() {
        this.loading = true;
        this.userService.register(this.model.email, this.model.password, this.model.confirmPassword)
            .subscribe(
            data => {
                this.alertService.success('Registration successful', true);
                this.loading = false;
            },
            error => {
                this.errorHandlerService.handle(error);
                this.loading = false;
            });
    }
}