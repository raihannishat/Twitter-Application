import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, HostBinding, Input, OnChanges, OnDestroy, OnInit, Output, Renderer2, SimpleChanges, ViewChild, ViewEncapsulation } from '@angular/core';
import { UntypedFormControl } from '@angular/forms';
import { MatAutocomplete } from '@angular/material/autocomplete';
import { fuseAnimations } from '@fuse/animations/public-api';
import { environment } from 'environments/environment';
import { Subject, debounceTime, filter, map, takeUntil } from 'rxjs';
import { SearchService } from './search.service';


@Component({
    selector: 'search',
    templateUrl: './search.component.html',
    encapsulation: ViewEncapsulation.None,
    exportAs: 'fuseSearch',
    animations: fuseAnimations
})
export class SearchComponent implements OnChanges, OnInit, OnDestroy {
    @Input() appearance: 'basic' | 'bar' = 'basic';
    @Input() debounce: number = 500;
    @Input() minLength: number = 1;
    @Output() search: EventEmitter<any> = new EventEmitter<any>();

    searchUrl = environment.baseUrl + 'search';
    userUrl = environment.baseUrl + 'users';

    opened: boolean = false;
    resultSets: any[];
    resultSetsHash: any[];
    searchControl: UntypedFormControl = new UntypedFormControl();
    private _matAutocomplete: MatAutocomplete;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _elementRef: ElementRef,
        private _httpClient: HttpClient,
        private _renderer2: Renderer2,
        private searchService: SearchService
    ) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Host binding for component classes
     */
    @HostBinding('class') get classList(): any {
        return {
            'search-appearance-bar': this.appearance === 'bar',
            'search-appearance-basic': this.appearance === 'basic',
            'search-opened': this.opened
        };
    }

    /**
     * Setter for bar search input
     *
     * @param value
     */
    @ViewChild('barSearchInput')
    set barSearchInput(value: ElementRef) {
        // If the value exists, it means that the search input
        // is now in the DOM, and we can focus on the input..
        if (value) {
            // Give Angular time to complete the change detection cycle
            setTimeout(() => {

                // Focus to the input element
                value.nativeElement.focus();
            });
        }
    }

    /**
     * Setter for mat-autocomplete element reference
     *
     * @param value
     */
    @ViewChild('matAutocomplete')
    set matAutocomplete(value: MatAutocomplete) {
        this._matAutocomplete = value;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On changes
     *
     * @param changes
     */
    ngOnChanges(changes: SimpleChanges): void {
        // Appearance
        if ('appearance' in changes) {
            // To prevent any issues, close the
            // search after changing the appearance
            this.close();
        }
    }

    /**
     * On init
     */
    ngOnInit(): void {
        // Subscribe to the search field value changes
        this.searchControl.valueChanges
            .pipe(
                debounceTime(this.debounce),
                takeUntil(this._unsubscribeAll),
                map((value) => {

                    if (!value || value.length < this.minLength) {
                        this.resultSets = null;
                        this.resultSetsHash = null;
                    }
                    return value;
                }),
                filter(value => value && value.length >= this.minLength)
            )
            .subscribe((value) => {
                const specialChars = /[`#]/;
                if (specialChars.test(value)) {
                    this.searchService.getUsersByHashtag(value).subscribe((hashResult: any) => {
                        this.resultSetsHash = hashResult;
                        this.search.next(hashResult);
                    });
                } else {
                    this.searchService.getUsersByName(value).subscribe((resultSets: any) => {
                        this.resultSets = resultSets;
                        this.search.next(resultSets);
                    });
                }
            });
    }

    getHashSearch(value) {
        this.searchService.getHashPost(value);

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
     * On keydown of the search input
     *
     * @param event
     */
    onKeydown(event: KeyboardEvent): void {
        // Escape
        if (event.code === 'Escape') {
            // If the appearance is 'bar' and the mat-autocomplete is not open, close the search
            if (this.appearance === 'bar' && !this._matAutocomplete.isOpen) {
                this.close();
            }
        }
    }

    /**
     * Open the search
     * Used in 'bar'
     */
    open(): void {
        // Return if it's already opened
        if (this.opened) {
            return;
        }

        // Open the search
        this.opened = true;
    }

    /**
     * Close the search
     * * Used in 'bar'
     */
    close(): void {
        // Return if it's already closed
        if (!this.opened) {
            return;
        }

        // Clear the search input
        this.searchControl.setValue('');

        // Close the search
        this.opened = false;
    }

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    // trackByFn(index: number, item: any): any
    // {
    //     return item.id || index;
    // }
}
