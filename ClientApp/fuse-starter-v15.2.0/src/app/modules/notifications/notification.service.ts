import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { NotificationsViewModel } from './notification.model';


@Injectable({
	providedIn: 'root'
})
export class NotificationService {

	baseUrl: string = environment.baseUrl + 'notifications';

	constructor(private httpClient: HttpClient) { }

	getUserNotification(pageNumber: number): Observable<NotificationsViewModel[]> {

		const requestUrl = `${this.baseUrl}?page=${pageNumber}`;

		return this.httpClient.get<NotificationsViewModel[]>(requestUrl);

	}
}
