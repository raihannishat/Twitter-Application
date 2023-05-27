import { NgModule } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { FuseCardModule } from '@fuse/components/card';
import { SharedModule } from 'app/shared/shared.module';
import { AvatarModule } from "ngx-avatar";
import { NotificationsComponent } from './notifications.component';
import { notificationRoutes } from './notifications.routing';

@NgModule({
    declarations: [
        NotificationsComponent
    ],
    imports: [
        RouterModule.forChild(notificationRoutes),
        MatFormFieldModule,
        MatInputModule,
        MatTooltipModule,
        FuseCardModule,
        SharedModule,
        AvatarModule
    ]
})
export class NotificationsModule {
}
