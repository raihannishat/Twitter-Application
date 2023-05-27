import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, TemplateRef, ViewChild, ViewContainerRef } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';
import { FollowService } from 'app/core/follow/follow.service';
import { MessagesService } from 'app/layout/common/messages/messages.service';
import { Message } from 'app/layout/common/messages/messages.types';
import { Subject } from 'rxjs';
import { FollowResponse, FollowersViewModel } from '../models/follow-model';
import { UserViewModel } from '../user/user.model';
import { UserService } from '../user/user.service';
import { NotificationsViewModel } from './people.model';

@Component({
	selector: 'app-notifications',
	templateUrl: './people.component.html',
	styleUrls: ['./people.component.scss'],

})
export class PeopleComponent implements OnInit, OnDestroy {


	@ViewChild('messagesOrigin') private _messagesOrigin: MatButton;
	@ViewChild('messagesPanel') private _messagesPanel: TemplateRef<any>;

	messages: Message[];
	notifications: NotificationsViewModel[];
	pageNumber = 1;
	unreadCount: number = 0;

	followerList: FollowersViewModel[] = [];
	followingList: FollowersViewModel[] = [];
	userId: string = '';
	followerTab: number;
	userVM: UserViewModel;
	private _overlayRef: OverlayRef;
	private _unsubscribeAll: Subject<any> = new Subject<any>();

	/**
	 * Constructor
	 */
	constructor(
		private _changeDetectorRef: ChangeDetectorRef,
		private _messagesService: MessagesService,
		private _overlay: Overlay,
		private _viewContainerRef: ViewContainerRef,
		private followService: FollowService,
		private activatedRoute: ActivatedRoute,
		private router: Router,
		private userService: UserService,
	) {
		this.userId = this.activatedRoute.snapshot.paramMap.get('id');
		this.followerTab = Number(this.activatedRoute.snapshot.queryParams['followerTab']);
	}

	// -----------------------------------------------------------------------------------------------------
	// @ Lifecycle hooks
	// -----------------------------------------------------------------------------------------------------

	/**
	 * On init
	 */
	ngOnInit(): void {
		this.getFollowerFollowingList();
	}

	onRedirectToProfile(follow: FollowersViewModel): void {
		this.router.navigate(['/profile/', follow.id]);
	}

	getFollowerFollowingList(): any {
		this.followService.getFollowers(this.userId, this.pageNumber).subscribe((res) => {
			this.followerList.push(...res);
		});
		this.followService.getFollowing(this.userId, this.pageNumber).subscribe((res) => {
			this.followingList.push(...res);
		});
	}

	onScroll(): any {
		this.pageNumber += 1;
		this.getFollowerFollowingList();
	}

	ngOnDestroy(): void {
		this._unsubscribeAll.next(null);
		this._unsubscribeAll.complete();

		// Dispose the overlay
		if (this._overlayRef) {
			this._overlayRef.dispose();
		}
	}

	/**
	 * Open the messages panel
	 */
	openPanel(): void {
		// Return if the messages panel or its origin is not defined
		if (!this._messagesPanel || !this._messagesOrigin) {
			return;
		}

		// Create the overlay if it doesn't exist
		if (!this._overlayRef) {
			this._createOverlay();
		}

		// Attach the portal to the overlay
		this._overlayRef.attach(new TemplatePortal(this._messagesPanel, this._viewContainerRef));
	}

	/**
	 * Close the messages panel
	 */
	closePanel(): void {
		this._overlayRef.detach();
	}

	/**
	 * Mark all messages as read
	 */
	markAllAsRead(): void {
		// Mark all as read
		this._messagesService.markAllAsRead().subscribe();
	}

	/**
	 * Toggle read status of the given message
	 */
	toggleRead(message: Message): void {
		// Toggle the read status
		message.read = !message.read;

		// Update the message
		this._messagesService.update(message.id, message).subscribe();
	}

	/**
	 * Delete the given message
	 */
	delete(message: Message): void {
		// Delete the message
		this._messagesService.delete(message.id).subscribe();
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

	// -----------------------------------------------------------------------------------------------------
	// @ Private methods
	// -----------------------------------------------------------------------------------------------------

	/**
	 * Create the overlay
	 */
	private _createOverlay(): void {
		// Create the overlay
		this._overlayRef = this._overlay.create({
			hasBackdrop: true,
			backdropClass: 'fuse-backdrop-on-mobile',
			scrollStrategy: this._overlay.scrollStrategies.block(),
			positionStrategy: this._overlay.position()
				.flexibleConnectedTo(this._messagesOrigin._elementRef.nativeElement)
				.withLockedPosition(true)
				.withPush(true)
				.withPositions([
					{
						originX: 'start',
						originY: 'bottom',
						overlayX: 'start',
						overlayY: 'top'
					},
					{
						originX: 'start',
						originY: 'top',
						overlayX: 'start',
						overlayY: 'bottom'
					},
					{
						originX: 'end',
						originY: 'bottom',
						overlayX: 'end',
						overlayY: 'top'
					},
					{
						originX: 'end',
						originY: 'top',
						overlayX: 'end',
						overlayY: 'bottom'
					}
				])
		});

		// Detach the overlay from the portal on backdrop click
		this._overlayRef.backdropClick().subscribe(() => {
			this._overlayRef.detach();
		});
	}

	followChange(targetId): any {
		this.followService.follow(targetId.id).subscribe({
			next: (response: FollowResponse) => {
				const index = this.followerList.indexOf(targetId);
				// const indexFollowing = this.followingList.indexOf(targetId);
				this.followerList[index].isFollowing = response.isFollowing;
				this.followChangeFollowing(targetId);
				// this.followingList[indexFollowing].isFollowing = response.isFollowing;
			},
			error: (err) => {
				console.log(err);
			}
		});
	}
	followChangeFollowing(targetId): any {
		this.followService.follow(targetId.id).subscribe({
			next: (response: FollowResponse) => {
				const index = this.followingList.indexOf(targetId);
				// const indexFollow = this.followerList.indexOf(targetId);
				this.followingList[index].isFollowing = response.isFollowing;
				this.followChange(targetId);
				// this.followerList[indexFollow].isFollowing = response.isFollowing;
			},
			error: (err) => {
				console.log(err);
			}
		});
	}
}
