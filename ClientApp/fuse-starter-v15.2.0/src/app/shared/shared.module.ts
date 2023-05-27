import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterModule } from '@angular/router';
import { FuseAlertModule } from '@fuse/components/alert';
import { FuseCardModule } from '@fuse/components/card';
import { DateAgoPipe } from 'app/pipes/date-ago.pipe';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { SidebarComponent } from './components/sidebar/sidebar.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        RouterModule,
        MatButtonModule,
        MatDividerModule,
        MatIconModule,
        MatMenuModule,
        FuseCardModule,
        MatFormFieldModule,
        MatCheckboxModule,
        MatInputModule,
        MatProgressSpinnerModule,
        FuseAlertModule,
        InfiniteScrollModule
    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        RouterModule,
        MatButtonModule,
        MatDividerModule,
        MatIconModule,
        MatMenuModule,
        FuseCardModule,
        MatFormFieldModule,
        SidebarComponent,
        MatCheckboxModule,
        MatInputModule,
        MatProgressSpinnerModule,
        FuseAlertModule,
        InfiniteScrollModule,
        DateAgoPipe
    ],
    declarations: [SidebarComponent, DateAgoPipe]
})
export class SharedModule {
}
