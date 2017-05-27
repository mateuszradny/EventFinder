import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'

import { Event, User } from '../_models/index';
import { AuthenticationService, AlertService, EventService, ErrorHandlerService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'event-viewer.component.html',
    selector: 'event-viewer'
})
export class EventViewerComponent implements OnInit, OnDestroy {
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private eventService: EventService,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private errorHandlerService: ErrorHandlerService
    ) { }

    private sub: any;

    event: Event = new Event();
    user: User;

    ngOnInit() {
        this.authenticationService.getCurrentUser().subscribe(user => this.user = user);

        this.sub = this.route.params.subscribe(params => {
            if (!params['id']) {
                this.router.navigate(['']);
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

    remove() {
        this.eventService.remove(this.event.id).subscribe(
            data => {
                this.alertService.success("Zdarzenie zostało usunięte z systemu", true);
                this.router.navigate(['']);
            },
            error => {
                this.errorHandlerService.handle(error, true);
            }
        )
    }
}