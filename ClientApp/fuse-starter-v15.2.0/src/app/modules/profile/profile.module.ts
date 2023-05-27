import { NgModule } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { FuseAlertModule } from '@fuse/components/alert';
import { FuseCardModule } from '@fuse/components/card';
import { ProfileComponent } from 'app/modules/profile/profile.component';
import { profileRoutes } from 'app/modules/profile/profile.routing';
import { SharedModule } from 'app/shared/shared.module';
import { AvatarModule } from "ngx-avatar";
@NgModule({
    declarations: [
        ProfileComponent,

    ],
    imports: [
        RouterModule.forChild(profileRoutes),
        MatFormFieldModule,
        MatInputModule,
        MatTooltipModule,
        FuseCardModule,
        SharedModule,
        AvatarModule,
        FuseAlertModule
    ]
})
export class ProfileModule {
}
