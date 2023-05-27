import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'app/core/auth/auth.service';
import { GlobalNotificationService } from 'app/core/notification/notification.service';
import { finalize, Subject, takeUntil, takeWhile, tap, timer } from 'rxjs';

@Component({
    selector: 'auth-sign-out',
    templateUrl: './sign-out.component.html',
    encapsulation: ViewEncapsulation.None
})
export class AuthSignOutComponent implements OnInit, OnDestroy {
    countdown: number = 5;
    countdownMapping: any = {
        '=1': '# second',
        'other': '# seconds'
    };
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _authService: AuthService,
        private _router: Router,
        private notificationService: GlobalNotificationService
    ) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        // Sign out

        this._authService.signOut().subscribe({
            next: (res) => {
                this.notificationService.stopHubConnection();
                console.log('successfully logged out');
                console.log(res);
                timer(1000, 1000)
                    .pipe(
                        finalize(() => {
                            this._router.navigate(['sign-in']);
                        }),
                        takeWhile(() => this.countdown > 0),
                        takeUntil(this._unsubscribeAll),
                        tap(() => this.countdown--)
                    )
                    .subscribe();
            },
            error: (err) => {
                console.log(err);
            }
        });

        // Redirect after the countdown

    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }
}
