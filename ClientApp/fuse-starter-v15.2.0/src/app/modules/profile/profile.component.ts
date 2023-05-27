import { ChangeDetectorRef, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseAlertService } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';
import { BlockService } from 'app/core/block/block.service';
import { CommentService } from 'app/core/comment/comment.service';
import { FollowService } from 'app/core/follow/follow.service';
import { LikeService } from 'app/core/like/like.service';
import { GlobalNotificationService } from 'app/core/notification/notification.service';
import { PhotoService } from 'app/core/photo/photo.service';
import { RetweetService } from 'app/core/retweet/retweet.service';
import { TweetService } from 'app/core/tweet/tweet.service';
import { BlockResponse } from 'app/modules/models/block-model';
import { CommentViewModel, CreateComment } from 'app/modules/models/comment-model';
import { FollowResponse } from 'app/modules/models/follow-model';
import { TweetViewModel } from 'app/modules/models/tweet-model';
import { UserViewModel } from '../user/user.model';
import { UserService } from '../user/user.service';

@Component({
	selector: 'profile',
	templateUrl: './profile.component.html',
	styles: ['.likeColor { color: #F91880; }', '.retweetColor { color:#2ecc71; }', '.display-block{display: block}', '.display-none{display: none}'],
	encapsulation: ViewEncapsulation.None,
	// changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProfileComponent implements OnInit {
	/**
	 * Constructor
	 */
	pageNumber = 1;
	commentPageNumber = 1;
	count = 1;
	throttle = 0;
	distance = 2;
	CommentOpen: boolean = false;
	postId: string;
	selectedFile: File = null;

	alertMessage = "Post Create Successfully";
	currentUserId = this.authService.userId;
	userVM: UserViewModel;
	tweetVMList: TweetViewModel[];
	commentVMList: CommentViewModel[];
	createCommentDto: CreateComment;
	userId: string;
	commentForm = new FormControl('', Validators.required);


	tweetForm = new FormControl('', Validators.required);

	constructor(
		private tweetService: TweetService,
		private route: ActivatedRoute,
		private userService: UserService,
		private likeService: LikeService,
		private retweetService: RetweetService,
		private crd: ChangeDetectorRef,
		private authService: AuthService,
		private _router: Router,
		private commentService: CommentService,
		private followService: FollowService,
		private blockService: BlockService,
		private notificationService: GlobalNotificationService,
		private router: Router,
		private fuseAlertService: FuseAlertService,
		private photoService: PhotoService
	) {
	}

	ngOnInit() {
		this.subscribetoNewSearch();
	}

	subscribetoNewSearch() {
		this.route.params.subscribe((res) => {
			this.userId = res.userId;
			this.timelineDataload();
		});
	}

	timelineDataload() {
		// this.userId = this.route.snapshot.params['userId'];
		if (!this.userId) {
			this.userId = this.authService.userId;
		}
		this.getUserById();
		this.pageNumber = 1;
		this.tweetService.getUserTimelineData(this.userId, this.pageNumber).subscribe({
			next: (res) => {
				this.tweetVMList = res;
				// console.log(res);
				this.crd.detectChanges();
			},
			error: (err) => {
				console.log(err);
			}
		});
	}

	followerList(id): void {
		this._router.navigate(['/people/', id], { queryParams: { followerTab: 0 } });
	}

	followingList(id): void {
		this._router.navigate(['/people/', id], { queryParams: { followerTab: 1 } });
	}

	blockUsers() {
		this._router.navigate(['/blocked-user']);
	}

	follow(targetId: string) {
		this.followService.follow(targetId).subscribe({
			next: (response: FollowResponse) => {
				this.userVM.isFollowing = response.isFollowing;
				this.userVM.followers = response.followers
				// debugger
			},
			error: (err) => {
				console.log(err);
			}
		});
	}

	onRedirectToProfile(userId: string): void {
		this.router.navigate(['/profile/', userId]);
	}

	block(targetId: string) {
		this.blockService.block(targetId).subscribe({
			next: (response: BlockResponse) => {
				this.userVM.isBlockedByCurrentUser = response.isBlocked;
				this.tweetService.getUserTimelineData(this.userId, 1).subscribe({
					next: (res) => {
						this.tweetVMList = res;
						this.pageNumber = 1;
						console.log(res);
						this.crd.detectChanges();
					},
					error: (err) => {
						console.log(err);
					}
				})
			},
			error: (err) => {
				console.log(err);
			}
		});
	}

	onScroll(): void {
		this.tweetService
			.getUserTimelineData(this.userId, ++this.pageNumber)
			.subscribe((tweets: TweetViewModel[]) => {
				this.tweetVMList.push(...tweets);
				this.count++;
				// console.log(this.pageNumber);
				// console.log(tweets);
				this.crd.detectChanges();
			});
	}


	postTweet(): void {

		this.tweetService.tweet.content = this.tweetForm.value;

		this.tweetService.createPost(this.tweetService.tweet).subscribe({
			//user id should be here current logges in userId
			next: res => {
				this.tweetService.getUserTimelineData(this.userId, 1).subscribe({
					next: res => {
						this.tweetForm.reset();
						this.tweetVMList = res;
						this.pageNumber = 1;
						this.crd.detectChanges();
					},
					error: err => {
						console.log(err);
					}
				});
			},
			error: err => {
				console.log(err);
			},
		});
	}


	deletePost(tweet: TweetViewModel): void {
		this.tweetService.deletePost(tweet.id).subscribe({
			next: res => {
				let index = this.tweetVMList.indexOf(tweet);

				this.tweetVMList.splice(index, 1);
				this.crd.detectChanges();

			},
			error: err => {
				console.log(err);
			},
		});
	}

	likeChange(tweet: TweetViewModel) {

		this.likeService.likeTweet(tweet.id).subscribe({
			next: response => {
				let index = this.tweetVMList.indexOf(tweet);

				this.tweetVMList[index].likes = response.likes;

				this.tweetVMList[index].isLikedByCurrentUser = response.isLikedByCurrentUser;

				if (response.isLikedByCurrentUser) {
					this.notificationService.sendLike(tweet.id, tweet.tweetCreatorId);
				}
			},
			error: err => {
				console.log(err);
			}
		})
	}

	retweetChange(tweet: TweetViewModel) {

		this.retweetService.retweet(tweet.id).subscribe({
			next: response => {
				let index = this.tweetVMList.indexOf(tweet);
				this.tweetVMList[index].retweets = response.retweets;
				this.tweetVMList[index].isRetweetedByCurrentUser = response.isRetweetedByCurrentUser;

				if (response.isRetweetedByCurrentUser === true) {
					if (this.currentUserId === tweet.tweetCreatorId && this.currentUserId === this.userId) {
						this.tweetVMList[index].isRetweeted = true;
					}
					this.notificationService.sendRetweet(tweet.id, tweet.tweetCreatorId);
				}
				else {
					if (this.currentUserId === tweet.tweetCreatorId && this.currentUserId === this.userId) {
						this.tweetVMList[index].isRetweeted = false;

					}
					else if (this.currentUserId === this.userId) {
						this.tweetVMList.splice(index, 1);
					}
				}

			},
			error: err => {
				console.log(err);
			}
		})
	}

	createComment(tweet: TweetViewModel) {

		const payload = {
			tweetId: tweet.id,
			tweetCreatorId: tweet.tweetCreatorId,
			content: this.commentForm.value
		};

		this.commentService.createComment(payload).subscribe({

			next: (response) => {
				this.commentForm.reset();

				const index = this.tweetVMList.indexOf(tweet);

				this.tweetVMList[index].comments = response.totalComments;

				this.notificationService.sendComment(tweet.id, tweet.tweetCreatorId);

				this.commentService.getComments(tweet.id, 1).subscribe({
					next: (res) => {
						this.commentVMList = res;
					},
					error: (err) => {
						console.log(err);
					}
				});
			},
			error: (err) => {
				console.log(err);
			}
		});
	}

	getComments(tweet: TweetViewModel) {
		this.postId = tweet.id;
		if (this.CommentOpen === true) {
			this.CommentOpen = false;
		}
		this.CommentOpen = !this.CommentOpen;
		this.commentService.getComments(tweet.id, 1).subscribe({
			next: (res) => {
				this.commentVMList = res;
				this.commentPageNumber = 1;
			},
			error: (err) => {
				console.log(err);
			}
		});
	}

	viewMoreComment(tweet: TweetViewModel) {
		this.commentService.getComments(tweet.id, ++this.commentPageNumber).subscribe({
			next: (comments: CommentViewModel[]) => {
				this.commentVMList.push(...comments);
			},
			error: (err) => {
				console.log(err);
			}
		});
	}

	getUserById() {
		this.userService.getUserById(this.userId).subscribe({
			next: (res) => {
				this.userVM = res;
				this.crd.detectChanges();
			},
			error: (err) => {
				console.log(err);
			}
		});
	}

	deleteComment(tweet: TweetViewModel, comment: CommentViewModel) {
		this.commentService.deleteComment(tweet.id, comment.id).subscribe((response) => {
			const index = this.tweetVMList.indexOf(tweet);
			this.tweetVMList[index].comments = response.totalComments;
			const commentIndex = this.commentVMList.indexOf(comment);
			this.commentVMList.splice(commentIndex, 1);
		});
	}

	uploadPofileImage(event) {
		this.selectedFile = <File>event.target.files[0];
		const fd = new FormData();
		fd.append('File', this.selectedFile, this.selectedFile.name);
		this.photoService.uploadPhoto(fd).subscribe({
			next: (res) => {
				localStorage.setItem('image', res.image);
				this.userVM.image = res.image;
				this.tweetService.getUserTimelineData(this.userId, 1).subscribe({
					next: res => {
						this.tweetForm.reset();
						this.tweetVMList = res;
						this.pageNumber = 1;
						this.crd.detectChanges();
					},
					error: err => {
						console.log(err);
					}
				});
			},
			error: (err) => {
				console.log(err);
			}
		})
	}

	uploadCoverImage(event) {
		this.selectedFile = <File>event.target.files[0];
		const fd = new FormData();
		fd.append('File', this.selectedFile, this.selectedFile.name);
		this.photoService.uploadCoverPhoto(fd).subscribe({
			next: (res) => {
				console.log(res);
				this.userVM.coverImage = res.image;
			},
			error: (err) => {
				console.log(err);
			}
		})
	}
}
