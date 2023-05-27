import { HttpClient } from "@angular/common/http";
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { UntypedFormControl } from '@angular/forms';
import { MatDrawer } from '@angular/material/sidenav';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { Contact, Country } from 'app/modules/admin/apps/contacts/contacts.types';
import { environment } from 'environments/environment';
import { Observable, Subject, takeUntil } from 'rxjs';
import { ContactsService } from '../../contacts/contacts.service';




@Component({
    selector: 'contacts-list',
    templateUrl: './list.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.Default
})
export class ContactsListComponent implements OnInit, OnDestroy {
    @ViewChild('matDrawer', { static: true }) matDrawer: MatDrawer;

    get contacts(): Observable<Contact[]> {
        return this._contactsService.contacts$;
    };

    contactsCount: number = 0;
    contactsTableColumns: string[] = ['name', 'email', 'phoneNumber', 'job'];
    countries: Country[];
    drawerMode: 'side' | 'over';
    searchInputControl: UntypedFormControl = new UntypedFormControl();
    selectedContact: Contact;

    baseUrl = environment.baseUrl + 'admin/';

    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _changeDetectorRef: ChangeDetectorRef,
        private _contactsService: ContactsService,
        private _router: Router,
        private _httpClient: HttpClient,
        private _fuseMediaWatcherService: FuseMediaWatcherService
    ) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {

        // Get the contacts
        this._contactsService.contacts$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((contacts: Contact[]) => {

                // Update the counts
                this.contactsCount = contacts.length;

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });

        // Get the contact
        this._contactsService.contact$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((contact: Contact) => {

                // Update the selected contact
                this.selectedContact = contact;

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });


        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {

                // Set the drawerMode if the given breakpoint is active
                if (matchingAliases.includes('lg')) {
                    this.drawerMode = 'side';
                }
                else {
                    this.drawerMode = 'over';
                }

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });
    }

    logout() {
        this._router.navigate(['/logout']);
    }
    editUser(id): any {
        // this._router.navigate(['/contacts', id]);

    }

    blockUser(id): any {
        this._httpClient.post<any>(`${this.baseUrl}users/${id}/block`, '').subscribe((data) => {
            this.contacts.subscribe((res) => {
                let user = res.find(x => x.id === id);
                user.isBlockedByAdmin = data.isBlocked;

                this._changeDetectorRef.markForCheck();
            })
        });

    }

    onScroll(): void {
        this._contactsService.pageNumber = this._contactsService.pageNumber + 1;
        this._httpClient.get<Contact[]>(
            `${this.baseUrl}users?page=${this._contactsService.pageNumber}`)
            .subscribe((contacts) => {
                let allContacts = this._contactsService._contacts.value;
                contacts.map(contact => {
                    allContacts?.push(contact);
                });
                this._contactsService._contacts.next(allContacts);
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * On backdrop clicked
     */
    onBackdropClicked(): void {
        // Go back to the list
        this._router.navigate(['./'], { relativeTo: this._activatedRoute });

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * Create contact
     */
    createContact(): void {
        // Create the contact
        this._contactsService.createContact().subscribe((newContact) => {

            // Go to the new contact
            this._router.navigate(['./', newContact.id], { relativeTo: this._activatedRoute });

            // Mark for check
            this._changeDetectorRef.markForCheck();
        });
    }

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }
}
