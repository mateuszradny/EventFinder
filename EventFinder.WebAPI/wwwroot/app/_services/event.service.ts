import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'

import { AuthenticationService } from './authentication.service';
import { Event, EventSearchQuery } from '../_models/index';

@Injectable()
export class EventService {
    constructor(private http: Http, private authenticationService: AuthenticationService) { }

    add(event: Event) {
        return this.http.post('api/event/', event, this.authenticationService.getAuthHeaders())
            .map(response => response.json());
    }

    get(id: number): Observable<Event> {
        return this.http.get('/api/event/' + id)
            .map(response => response.json());
    }

    getByQuery(query: EventSearchQuery): Observable<Event[]> {
        return this.http.post('/api/event/query', query)
            .map(response => response.json());
    }

    getIncoming(): Observable<Event[]> {
        return this.http.get('/api/event/incoming')
            .map(response => response.json());
    }

    remove(id: number) {
        return this.http.delete('/api/event/' + id, this.authenticationService.getAuthHeaders());
    }

    update(event: Event) {
        return this.http.put('/api/event/' + event.id, event, this.authenticationService.getAuthHeaders());
    }
}