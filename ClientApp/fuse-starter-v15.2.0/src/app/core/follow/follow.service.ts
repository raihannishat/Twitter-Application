import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FollowersViewModel, FollowingsViewModel, FollowResponse } from 'app/modules/models/follow-model';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';


@Injectable({
	providedIn: 'root'
})
export class FollowService {


	baseUrl: string = environment.baseUrl + 'follow/';

	constructor(private _httpClient: HttpClient) { }

	follow(targetUserId: string): Observable<FollowResponse> {

		const requestUrl = `${this.baseUrl}user/${targetUserId}`;

		return this._httpClient.post<FollowResponse>(requestUrl, {})

	}

	getFollowers(userId: string, pageNumber: number): Observable<FollowersViewModel[]> {

		const requestUrl = `${this.baseUrl}user/${userId}/followers?page=${pageNumber}`;

		return this._httpClient.get<FollowersViewModel[]>(requestUrl);
	}

	getFollowing(userId: string, pageNumber: number): Observable<FollowingsViewModel[]> {

		const requestUrl = `${this.baseUrl}user/${userId}/following?page=${pageNumber}`;

		return this._httpClient.get<FollowingsViewModel[]>(requestUrl);
	}
}
