import { Route } from '@angular/router';
import { UserComponent } from '../user/user/user.component';
import { PeopleComponent } from './people.component';

export const PeopleRouting: Route[] = [
    { path: '', component: UserComponent },
    { path: ':id', component: PeopleComponent },
];
