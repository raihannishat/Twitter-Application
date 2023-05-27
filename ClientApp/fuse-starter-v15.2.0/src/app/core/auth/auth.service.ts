import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthUtils } from 'app/core/auth/auth.utils';
import { UserService } from 'app/core/user/user.service';
import { environment } from 'environments/environment';
import { catchError, Observable, of, switchMap, throwError } from 'rxjs';
import { GlobalNotificationService } from '../notification/notification.service';


@Injectable()
export class AuthService {
    public _authenticated: boolean = false;
    baseUrl = environment.baseUrl + 'auth/';
    /**
     * Constructor
     */
    constructor(
        private _httpClient: HttpClient,
        private _userService: UserService,
        private _notificationService: GlobalNotificationService
    ) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for access token
     */
    set accessToken(token: string) {
        localStorage.setItem('accessToken', token);
    }

    get accessToken(): string {
        return localStorage.getItem('accessToken') ?? '';
    }

    set refreshToken(token: string) {
        localStorage.setItem('refreshToken', token);
    }

    get refreshToken(): string {
        return localStorage.getItem('refreshToken') ?? '';
    }

    get userId(): string {
        const helper = new JwtHelperService();
        const decodedToken = helper.decodeToken(this.accessToken);
        return decodedToken.nameid;
    }

    get role(): string {
        const helper = new JwtHelperService();
        const decodedToken = helper.decodeToken(this.accessToken);
        return decodedToken.role;
        // console.log(decodedToken.email);
        // console.log(decodedToken.nameid);
        // return localStorage.getItem('role');
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Forgot password
     *
     * @param email
     */
    forgotPassword(email: string): Observable<any> {
        return this._httpClient.post(this.baseUrl + 'forget-password?email=' + email, {});
    }

    /**
     * Reset password
     *
     * @param password
     */
    resetPassword(password: string, token?: string): Observable<any> {
        return this._httpClient.post(this.baseUrl + 'reset-password', { password: password, token: token });
    }


    verifyAccount(verifyCode: string): Observable<any> {

        const requestUrl = `${this.baseUrl}verify-account?token=${verifyCode}`;

        return this._httpClient.post(requestUrl, {});
    }

    getRefreshToken(): Observable<any> {

        const requestUrl = `${this.baseUrl}refreshToken`;

        return this._httpClient.post(requestUrl, { accessToken: this.accessToken, refreshToken: this.refreshToken });
    }

    /**
     * Sign in
     *
     * @param credentials
     */
    signIn(credentials: { email: string; password: string }): Observable<any> {
        // Throw error, if the user is already logged in
        if (this._authenticated) {
            return throwError('User is already logged in.');
        }

        return this._httpClient.post(this.baseUrl + 'sign-in', credentials).pipe(
            switchMap((response: any) => {

                // Store the access token in the local storage
                this.accessToken = response.accessToken;

                // Store the refresh token in the local storage
                this.refreshToken = response.refreshToken;

                // Set the authenticated flag to true
                this._authenticated = true;

                // Store the user on the user service
                this._userService.user = response.user;
                // this.userId = response.user.id;
                // this.role = response.user.role;
                // console.log(this._userService.user);
                // console.log(response.user);

                // Return a new observable with the response
                return of(response);
            })
        );
    }

    /**
     * Sign in using the access token
     */
    signInUsingToken(): Observable<any> {
        // Sign in using the token
        return this._httpClient.post('api/auth/sign-in-with-token', {
            accessToken: this.accessToken
        }).pipe(
            catchError(() =>

                // Return false
                of(false)
            ),
            switchMap((response: any) => {

                // Replace the access token with the new one if it's available on
                // the response object.
                //
                // This is an added optional step for better security. Once you sign
                // in using the token, you should generate a new one on the server
                // side and attach it to the response object. Then the following
                // piece of code can replace the token with the refreshed one.
                if (response.accessToken) {
                    this.accessToken = response.accessToken;
                }

                // Set the authenticated flag to true
                this._authenticated = true;

                // Store the user on the user service
                this._userService.user = response.user;

                // Return true
                return of(true);
            })
        );
    }

    /**
     * Sign out
     */
    signOut(): Observable<any> {

        // Remove the access token from the local storage
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('userId');
        localStorage.removeItem('role');
        localStorage.removeItem('image');
        // Set the authenticated flag to false
        this._authenticated = false;

        // const requestUrl = `${this.baseUrl}sign-out?userId=${this.userId}`;

        // return this._httpClient.post<any>(requestUrl, {});
        return of(true);


        // Return the observable
    }

    /**
     * Sign up
     *
     * @param user
     */
    signUp(user: { name: string; email: string; password: string; }): Observable<any> {
        return this._httpClient.post(this.baseUrl + 'sign-up', user);
    }

    /**
     * Unlock session
     *
     * @param credentials
     */
    unlockSession(credentials: { email: string; password: string }): Observable<any> {
        return this._httpClient.post('api/auth/unlock-session', credentials);
    }

    /**
     * Check the authentication status
     */
    check(): Observable<boolean> {
        // Check if the user is logged in
        if (this._authenticated) {
            return of(true);
        }

        // Check the access token availability
        if (!this.accessToken) {
            return of(false);
        }

        // Check the access token expire date
        if (AuthUtils.isTokenExpired(this.accessToken)) {
            return of(false);
        }

        // If the access token exists and it didn't expire, sign in using it
        return this.signInUsingToken();
    }
}
