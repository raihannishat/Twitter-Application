import { Component, OnInit } from '@angular/core';
import { AuthService } from './core/auth/auth.service';
import { GlobalNotificationService } from './core/notification/notification.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    /**
     * Constructor
     */
    constructor(private authService: AuthService,
        private notificationService: GlobalNotificationService) {
    }
    ngOnInit(): void {
        let token = localStorage.getItem('accessToken') ?? '';

        if (!!token) {
            this.notificationService.stopHubConnection();
            this.notificationService.createHubConnection();
        }
    }
}
