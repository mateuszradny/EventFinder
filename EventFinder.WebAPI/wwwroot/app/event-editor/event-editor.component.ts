import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'

import { Event, User } from '../_models/index';
import { AlertService, AuthenticationService, EventService, ErrorHandlerService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'event-editor.component.html',
    selector: 'event-editor'
})
export class EventEditorComponent implements OnInit, OnDestroy {
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private eventService: EventService,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private errorHandlerService: ErrorHandlerService
    ) { }

    private isCreateForm: boolean = false;
    private sub: any;

    event: Event = new Event()

    ngOnInit() {
        this.authenticationService.getCurrentUser().subscribe(user => {
            if (user == null)
                this.router.navigate(['']);
        });

        this.sub = this.route.params.subscribe(params => {
            if (!params['id']) {
                this.isCreateForm = true;
            } else {
                this.eventService.get(+params['id']).subscribe(
                    event => this.event = event,
                    error => {
                        this.errorHandlerService.handle(error, true);
                        this.router.navigate(['']);
                    }
                )
            }
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    save() {
        let observable: any = this.isCreateForm ? this.eventService.add(this.event) : this.eventService.update(this.event);
        observable.subscribe(
            () => {
                this.alertService.success("Event saved", true);
                this.router.navigate(['']);
            },
            (error: any) => {
                this.errorHandlerService.handle(error);
            });
    }
}