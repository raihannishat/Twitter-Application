import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormGroup, NgForm, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';
import { finalize } from 'rxjs';

@Component({
    selector: 'auth-set-password',
    templateUrl: './set-password.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class AuthSetPasswordComponent implements OnInit {
    @ViewChild('forgotPasswordNgForm') setPasswordNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: ''
    };
    setPasswordForm: UntypedFormGroup;
    showAlert: boolean = false;

    /**
     * Constructor
     */
    constructor(
        private _authService: AuthService,
        private _formBuilder: UntypedFormBuilder,
        private router: Router
    ) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        // Create the form
        this.setPasswordForm = this._formBuilder.group({
            code: ['', Validators.required],
            password: ['', [Validators.required, Validators.minLength(6)]],
            confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
        },
            { validator: this.passwordMatchValidator }
        );
    }

    passwordMatchValidator(frm: FormGroup) {
        return frm.controls['password'].value === frm.controls['confirmPassword'].value ? null : { 'mismatch': true };
    }


    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Send the reset link
     */
    sendResetLink(): void {
        // Return if the form is invalid
        if (this.setPasswordForm.invalid) {
            return;
        }

        // Disable the form
        this.setPasswordForm.disable();

        // Hide the alert
        this.showAlert = false;

        // Forgot password
        let token = this.setPasswordForm.get('code')?.value;
        let password = this.setPasswordForm.get('password')?.value;
        this._authService.resetPassword(password, token)
            .pipe(
                finalize(() => {

                    // Re-enable the form
                    this.setPasswordForm.enable();

                    // Reset the form
                    this.setPasswordForm.reset();
                })
            )
            .subscribe(
                (response) => {

                    // Set the alert
                    this.alert = {
                        type: 'success',
                        message: 'password reset successfully. '
                    };
                    this.showAlert = true;
                    this.router.navigate(['/sign-in'])

                },
                (response) => {

                    // let errorMessageOfFirstKey = '';
                    // if(isObject(response.error) ){
                    //     let key = Object.keys(response.error.errors)[0];
                    //     errorMessageOfFirstKey = response.error.errors[key][0];
                    // }
                    // else{
                    //     errorMessageOfFirstKey = response.error;
                    // }
                    // let errorMessage = (!!errorMessageOfFirstKey ? errorMessageOfFirstKey: response.error);

                    // Set the alert
                    this.alert = {
                        type: 'error',
                        message: response.error
                    };
                    this.showAlert = true;

                }
            );
    }
}
