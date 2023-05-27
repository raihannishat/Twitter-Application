import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { delay, map, Observable } from 'rxjs';
import { FollowersViewModel } from '../models/follow-model';
import { UserViewModel } from './user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl: string = environment.baseUrl + 'users';

  constructor(private httpClient: HttpClient) { }

  getUserById(userId: string): Observable<UserViewModel> {

    return this.httpClient.get<UserViewModel>(this.baseUrl + `/${userId}`);
  }

  getUsers(pageNumber: number): Observable<FollowersViewModel[]> {
    const requestUrl = `${this.baseUrl}?page=${pageNumber}`;
    return this.httpClient.get<FollowersViewModel[]>(requestUrl);
  }

  checkIfEmailExists(value: string) {
    return this.getUserEmail(value).pipe(
      delay(500),
      map(result => (result ? { emailAlreadyExists: true } : null))
    );
  }

  getUserEmail(value: string): Observable<boolean> {
    const val = this.httpClient.get<boolean>(
      this.baseUrl + '/email-exist' + '/' + value
    );
    // console.log(val);
    return val;
  }

  getUsersForSidebar(): Observable<boolean> {
    const val = this.httpClient.get<boolean>(
      this.baseUrl + '?PageNumber=1'
    );
    return val;
  }
}
