import { Component, OnInit } from '@angular/core';

import { Event, EventSearchQuery, User } from '../_models/index';
import { AlertService, AuthenticationService, EventService, ErrorHandlerService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'event-list.component.html'
})
export class EventListComponent implements OnInit {
    constructor(
        private eventService: EventService,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private errorHandlerService: ErrorHandlerService
    ) { }

    events: Event[] = [];
    user: User = null;
    query = new EventSearchQuery();
    loading = false;

    ngOnInit() {
        this.eventService.getIncoming()
            .subscribe(events => this.events = events);

        this.authenticationService.getCurrentUser()
            .subscribe(user => this.user = user);
    }

    search() {
        this.eventService.getByQuery(this.query)
            .subscribe(events => this.events = events);
    }

    clearFilters() {
        this.query.city = null;
        this.query.from = null;
        this.query.to = null;;
        this.query.tag = null;

        this.eventService.getIncoming()
            .subscribe(events => this.events = events);
    }

    remove(eventId: number) {
        this.eventService.remove(eventId)
            .subscribe(
            data => {
                this.alertService.success('Event removed', true);
                this.events = this.events.filter(x => x.id !== eventId);
            },
            error => {
                this.errorHandlerService.handle(error);
            });
    }
}