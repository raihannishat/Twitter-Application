import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { NgModule } from '@angular/core';
import { MatButtonToggleModule } from "@angular/material/button-toggle";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from "@angular/material/tabs";
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { FuseCardModule } from '@fuse/components/card';
import { SharedModule } from 'app/shared/shared.module';
import { AvatarModule } from "ngx-avatar";
import { AuthInterceptor } from "../../core/auth/auth.interceptor";
import { AuthService } from "../../core/auth/auth.service";
import { BlockedUserComponent } from "./blocked-user.component";
import { BlockedUserRouting } from "./blocked-user.routing";

@NgModule({
   declarations: [
      BlockedUserComponent
   ],
   imports: [
      RouterModule.forChild(BlockedUserRouting),
      MatFormFieldModule,
      MatInputModule,
      MatTooltipModule,
      FuseCardModule,
      SharedModule,
      MatTabsModule,
      MatButtonToggleModule,
      AvatarModule
   ],
   providers: [
      AuthService,
      {
         provide: HTTP_INTERCEPTORS,
         useClass: AuthInterceptor,
         multi: true
      }
   ]
})
export class BlockedUserModule {
}
