import { ChangeDetectorRef, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'app/core/auth/auth.service';
import { LikeService } from 'app/core/like/like.service';
import { GlobalNotificationService } from 'app/core/notification/notification.service';
import { RetweetService } from 'app/core/retweet/retweet.service';
import { TweetViewModel } from 'app/modules/models/tweet-model';
import { ToastrService } from 'ngx-toastr';
import { CommentService } from "../../core/comment/comment.service";
import { TweetService } from '../../core/tweet/tweet.service';
import { SearchService } from "../../layout/common/search/search.service";
import { CommentViewModel } from "../models/comment-model";
import { UserViewModel } from '../user/user.model';
import { UserService } from '../user/user.service';

@Component({
    selector: 'home',
    templateUrl: './post.component.html',
    styles: ['.likeColor { color: #F91880; }', '.retweetColor { color:#2ecc71; }'],
    encapsulation: ViewEncapsulation.None,
})
export class PostComponent implements OnInit {
    show: boolean = false;
    pageNumber = 1;
    postId: string;
    CommentOpen: boolean = false;
    commentPageNumber = 1;
    commentVMList: CommentViewModel[];
    commentForm = new FormControl('', Validators.required);
    infiniteScrollStatus = false;
    userData: UserViewModel;
    searchText: string;
    // tweetVMlist: TweetViewModel[];
    get tweetVMlist() {
        return this.searchService.hashTweet;
    }
    tweetForm = new FormControl('', Validators.required);


    constructor(
        public tweetService: TweetService,
        private cdr: ChangeDetectorRef,
        private likeService: LikeService,
        private retweetService: RetweetService,
        private userService: UserService,
        private authService: AuthService,
        private searchService: SearchService,
        private toastService: ToastrService,
        private commentService: CommentService,
        private activatedRoute: ActivatedRoute,
        private notificationService: GlobalNotificationService,
        private router: Router) {
    }

    onRedirectToProfile(userId: string): void {
        this.router.navigate(['/profile/', userId]);
    }

    ngOnInit(): void {
        this.userService.getUserById(this.authService.userId).subscribe({
            next: res => {
                this.userData = res;
            },
            error: err => {
                console.log(err);
            }
        });
        this.searchText = this.activatedRoute.snapshot.queryParamMap.get('search').toString();
        this.searchService.getHashPost(this.searchText);
        // this.tweetVMlist = this.searchService.hashTweet;
        // console.log(this.tweetVMlist);
    }
    viewMoreComment(tweet: TweetViewModel): void {
        this.commentService.getComments(tweet.id, ++this.commentPageNumber).subscribe({
            next: (comments: CommentViewModel[]) => {
                this.commentVMList.push(...comments);
            },
            error: (err) => {
                console.log(err);
            }
        });
    }
    createComment(tweet: TweetViewModel): void {

        const payload = {
            tweetId: tweet.id,
            tweetCreatorId: tweet.tweetCreatorId,
            content: this.commentForm.value
        };

        this.commentService.createComment(payload).subscribe({

            next: (response) => {
                this.commentForm.reset();

                const index = this.tweetVMlist.indexOf(tweet);

                this.tweetVMlist[index].comments = response.totalComments;

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

    getComments(tweet: TweetViewModel): void {
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

    deleteComment(tweet: TweetViewModel, comment: CommentViewModel): void {
        this.commentService.deleteComment(tweet.id, comment.id).subscribe((response) => {
            const index = this.tweetVMlist.indexOf(tweet);
            this.tweetVMlist[index].comments = response.totalComments;
            const commentIndex = this.commentVMList.indexOf(comment);
            this.commentVMList.splice(commentIndex, 1);
        });
    }

    likeChange(tweet: TweetViewModel) {

        this.likeService.likeTweet(tweet.id).subscribe({
            next: response => {
                let index = this.tweetVMlist.indexOf(tweet);
                this.tweetVMlist[index].likes = response.likes;
                this.tweetVMlist[index].isLikedByCurrentUser = response.isLikedByCurrentUser;
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
                const index = this.tweetVMlist.indexOf(tweet);

                this.tweetVMlist[index].retweets = response.retweets;

                this.tweetVMlist[index].isRetweetedByCurrentUser = response.isRetweetedByCurrentUser;

                if (response.isRetweetedByCurrentUser) {
                    this.notificationService.sendRetweet(tweet.id, tweet.tweetCreatorId);
                }

            },
            error: err => {
                console.log(err);
            }
        })
    }


    onScroll(): void {
        this.pageNumber = this.pageNumber + 1;

        this.searchService.getHashPost(this.searchText, this.pageNumber);
    }


}
