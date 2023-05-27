import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
// import * as signalR from "@microsoft/signalr";
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'environments/environment';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, Observable } from 'rxjs';
import { NotificationViewModel } from '../../modules/models/notification-model';


@Injectable({
	providedIn: 'root'
})
export class GlobalNotificationService {

	isNotificationArrived: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
	hasNotification = false;

	private hubConnection!: HubConnection;

	baseUrl: string = environment.baseUrl + 'notifications';

	hubUrl: string = environment.hubUrl + 'notificationHub';

	constructor(private _httpClient: HttpClient,
		private toastr: ToastrService) { }

	createHubConnection() {
		let token = localStorage.getItem('accessToken') ?? '';
		this.hubConnection = new HubConnectionBuilder()
			.withUrl(this.hubUrl, {
				accessTokenFactory: () => token
			})
			.withAutomaticReconnect()
			.build();

		// this.hubConnection.serverTimeoutInMilliseconds = 300000;
		// this.hubConnection.keepAliveIntervalInMilliseconds = 300000;

		this.hubConnection.start()
			.then(() => console.log("Connection started"))
			.catch((error) => console.log(error));

		this.hubConnection.on('ReceiveNotification', (messages) => {
			this.hasNotification = true;
			this.isNotificationArrived.next(true);
			// this.toastr.success('', "You have New Notification");
			// window.alert('You have new notification')
		});

	}

	stopHubConnection() {
		if (this.hubConnection) {
			this.hubConnection.stop().catch(error => console.log(error));
		}
	}

	getNotifications(pageNumber: number): Observable<NotificationViewModel[]> {

		const requestUrl = `${this.baseUrl}?page=${pageNumber}`;

		return this._httpClient.get<NotificationViewModel[]>(requestUrl);
	}

	sendComment(tweetId: string, tweetCreatorId: string) {
		return this.hubConnection.invoke('SendComment',
			{ tweetId: tweetId, tweetCreatorId: tweetCreatorId }).catch((error) => console.log(error));
	}

	sendLike(tweetId: string, tweetCreatorId: string) {
		return this.hubConnection.invoke('SendLike', {
			tweetId: tweetId,
			tweetCreatorId: tweetCreatorId
		}).
			catch((error) => console.log(error));
	}


	sendRetweet(tweetId: string, tweetCreatorId: string) {
		return this.hubConnection.invoke('SendRetweet', {
			tweetId: tweetId,
			tweetCreatorId: tweetCreatorId
		}).
			catch((error) => console.log(error));
	}

}
