import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { FuseCardModule } from '@fuse/components/card';
import { SharedModule } from 'app/shared/shared.module';
import { AvatarModule } from 'ngx-avatar';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import {PostComponent} from './post.component';
import {PostRouting} from './post.routing';



@NgModule({
    declarations: [
        PostComponent,

    ],
    imports: [
        RouterModule.forChild(PostRouting),
        MatButtonModule,
        MatDividerModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
        MatTooltipModule,
        FuseCardModule,
        SharedModule,
        InfiniteScrollModule,
        AvatarModule,
    ]
})
export class PostModule {

}
