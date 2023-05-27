import { ChangeDetectorRef, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, ReplaySubject, tap } from 'rxjs';
import { Navigation } from 'app/core/navigation/navigation.types';
import { GlobalNotificationService } from '../notification/notification.service';

@Injectable({
    providedIn: 'root'
})
export class NavigationService
{
    _navigation: ReplaySubject<Navigation> = new ReplaySubject<Navigation>(1);

    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient, private notificationService: GlobalNotificationService) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for navigation
     */
    get navigation$(): Observable<Navigation>
    {
        return this._navigation.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get all navigation data
     */
    get(): Observable<Navigation>
    {
        return this._httpClient.get<Navigation>('api/common/navigation').pipe(
            tap((navigation) => {
                if(this.notificationService.hasNotification){
                let notf = navigation.compact.find(x => x.id === 'notificationKey');
                    notf.badge = {
                        title  : 'New',
                        classes: 'px-2 bg-red-600 text-white rounded-full'
                    }
                }
                
                this._navigation.next(navigation);
            })
        );
    }
}
