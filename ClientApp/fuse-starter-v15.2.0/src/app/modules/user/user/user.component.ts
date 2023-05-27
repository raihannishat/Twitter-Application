import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FollowService } from 'app/core/follow/follow.service';
import { FollowResponse, FollowersViewModel } from 'app/modules/models/follow-model';
import { UserService } from '../user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  users: FollowersViewModel[];

  pageNumber = 1;

  constructor(
    private userService: UserService,
    private followService: FollowService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getUsers(this.pageNumber).subscribe((res) => {
      this.users = res;
    });
  }

  onScroll(): void {
    this.pageNumber++;
    this.userService.getUsers(this.pageNumber).subscribe((res) => {
      this.users.push(...res);
    });
  }

  onRedirectToProfile(userId: string): void {
    this.router.navigate(['/profile/', userId]);
  }

  follow(follow: FollowersViewModel): void {
    this.followService.follow(follow.id).subscribe({
      next: (response: FollowResponse) => {
        const index = this.users.indexOf(follow);
        this.users[index].isFollowing = response.isFollowing;
      },
      error: (err) => {
        console.log(err);
      }
    });
  }
}
