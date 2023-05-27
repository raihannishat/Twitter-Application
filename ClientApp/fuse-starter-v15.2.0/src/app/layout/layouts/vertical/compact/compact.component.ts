import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil, tap } from 'rxjs';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { FuseNavigationService, FuseVerticalNavigationComponent } from '@fuse/components/navigation';
import { Navigation } from '../../../../core/navigation/navigation.types';
import { NavigationService } from '../../../../core/navigation/navigation.service';
import { GlobalNotificationService } from '../../../../core/notification/notification.service';
import { HttpClient } from '@angular/common/http';

@Component({
    selector     : 'compact-layout',
    templateUrl  : './compact.component.html',
    encapsulation: ViewEncapsulation.None
})
export class CompactLayoutComponent implements OnInit, OnDestroy
{
    isScreenSmall: boolean;
    navigation: Navigation;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private _navigationService: NavigationService,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _fuseNavigationService: FuseNavigationService,
        private notificationService: GlobalNotificationService,
        private _httpClient: HttpClient,
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for current year
     */
    get currentYear(): number
    {
        return new Date().getFullYear();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {

        this.notificationService.isNotificationArrived.subscribe(() => {
            
            this._httpClient.get<Navigation>('api/common/navigation').subscribe((navigation) => {
                    if(this.notificationService.hasNotification){
                    let notf = navigation.compact.find(x => x.id === 'notificationKey');
                        notf.badge = {
                            title  : 'New',
                            classes: 'px-2 bg-red-600 text-white rounded-full'
                        }
                    }
                    this.navigation = navigation;
                    this._navigationService._navigation.next(navigation);
                });
        });
        // Subscribe to navigation data
        this._navigationService.navigation$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((navigation: Navigation) => {
                this.navigation = navigation;
            });

        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({matchingAliases}) => {

                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Toggle navigation
     *
     * @param name
     */
    toggleNavigation(name: string): void
    {
        // Get the navigation
        const navigation = this._fuseNavigationService.getComponent<FuseVerticalNavigationComponent>(name);

        if ( navigation )
        {
            // Toggle the opened status
            navigation.toggle();
        }
    }
}
