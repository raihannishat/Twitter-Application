import { Component, ViewEncapsulation } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';

import { Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';
import { finalize, Subject, takeUntil, takeWhile, tap, timer } from 'rxjs';

@Component({
	selector: 'auth-confirmation-required',
	templateUrl: './confirmation-required.component.html',
	encapsulation: ViewEncapsulation.None,
	animations: fuseAnimations
})
export class AuthConfirmationRequiredComponent {

	tokenForm: UntypedFormGroup;
	showAlert: boolean = false;

	isVerified: boolean = false;
	countdown: number = 5;
	countdownMapping: any = {
		'=1': '# second',
		'other': '# seconds'
	};

	alert: { type: FuseAlertType; message: string } = {
		type: 'success',
		message: ''
	};
	private _unsubscribeAll: Subject<any> = new Subject<any>();

	/**
	 * Constructor
	 */
	constructor(
		private _authService: AuthService,
		private _formBuilder: UntypedFormBuilder,
		private _router: Router,
	) {
	}

	ngOnInit(): void {
		this.tokenForm = this._formBuilder.group({
			token: ['', Validators.required],
		}
		);
	}

	verifyCode() {
		// Do nothing if the form is invalid
		if (this.tokenForm.invalid) {
			return;
		}

		// Disable the form
		this.tokenForm.disable();

		// Hide the alert
		this.showAlert = false;
		this._authService.verifyAccount(this.tokenForm.get('token').value)
			.subscribe({
				next: (response) => {
					this.isVerified = true;
				},
				error: (response) => {

					// Re-enable the form
					this.tokenForm.enable();

					// Reset the form


					// Set the alert
					this.alert = {
						type: 'error',
						message: 'Something went wrong, please try again.'
					};

					// Show the alert
					this.showAlert = true;
				}
			});
		if (this.isVerified) {
			this.redirectToOtherPageAfterCountDown();
		}
		// Redirect after the countdown
		// this.isVerified = true;
		// if (this.isVerified) {
		// 	this.redirectToOtherPageAfterCountDown();
		// }
	}

	private redirectToOtherPageAfterCountDown() {
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
