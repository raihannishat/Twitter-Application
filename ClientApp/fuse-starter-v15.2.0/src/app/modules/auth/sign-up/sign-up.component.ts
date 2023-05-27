import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormGroup, NgForm, UntypedFormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';
import { UserValidator } from 'app/modules/user/user-validator';
import { UserService } from 'app/modules/user/user.service';

@Component({
    selector: 'auth-sign-up',
    templateUrl: './sign-up.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class AuthSignUpComponent implements OnInit {
    @ViewChild('signUpNgForm') signUpNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: ''
    };
    signUpForm: FormGroup;
    showAlert: boolean = false;

    /**
     * Constructor
     */
    constructor(
        private _authService: AuthService,
        private _formBuilder: UntypedFormBuilder,
        private _router: Router,
        private _userService: UserService
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
        this.signUpForm = this._formBuilder.group({
            name: ['', Validators.required, Validators.pattern('^[A-Za-z\\s]*$')],
            email: ['',
                [Validators.required, Validators.email],
                [UserValidator.emailValidator(this._userService)],
            ],
            password: ['', [Validators.required, Validators.pattern(
                '^(?=.*[A-Za-z])(?=.*?[0-9])(?=.*?[!@#$&*~]).{6,}$'
            )]],
        }
        );

    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Sign up
     */
    signUp(): void {
        // Do nothing if the form is invalid
        if (this.signUpForm.invalid) {
            return;
        }

        // Hide the alert
        this.showAlert = false;

        // Sign up
        this._authService.signUp(this.signUpForm.value)
            .subscribe(
                (response) => {

                    // Navigate to the confirmation required page
                    this._router.navigateByUrl('/confirmation-required');
                },
                (response) => {

                    // Set the alert
                    this.alert = {
                        type: 'error',
                        message: response.error
                    };

                    // Show the alert
                    this.showAlert = true;
                }
            );
    }
}
