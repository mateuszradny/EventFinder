import { Routes, RouterModule } from '@angular/router';

import { EventListComponent } from './event-list/index';
import { LoginComponent } from './login/index';
import { RegisterComponent } from './register/index';
import { EventEditorComponent } from './event-editor/event-editor.component';
import { EventViewerComponent } from './event-viewer/event-viewer.component';

const appRoutes: Routes = [
    { path: '', component: EventListComponent },
    { path: 'event-editor/:id', component: EventEditorComponent },
    { path: 'event-viewer/:id', component: EventViewerComponent },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);