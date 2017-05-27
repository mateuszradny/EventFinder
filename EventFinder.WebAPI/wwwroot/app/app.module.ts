import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BaseRequestOptions } from '@angular/http';

import { AppComponent } from './app.component';
import { routing } from './app.routing';

import { AlertComponent } from './_directives/index';
import { AlertService, AuthenticationService, EventService, UserService, ErrorHandlerService } from './_services/index';
import { EventListComponent } from './event-list/index';
import { LoginComponent } from './login/index';
import { RegisterComponent } from './register/index';
import { EventEditorComponent } from './event-editor/index';
import { EventViewerComponent } from './event-viewer/index';

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        routing
    ],
    declarations: [
        AppComponent,
        AlertComponent,
        EventListComponent,
        LoginComponent,
        RegisterComponent,
        EventEditorComponent,
        EventViewerComponent,
    ],
    providers: [
        AlertService,
        AuthenticationService,
        UserService,
        EventService,
        BaseRequestOptions,
        ErrorHandlerService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }