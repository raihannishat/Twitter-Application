import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BlockService } from 'app/core/block/block.service';
import { FollowService } from 'app/core/follow/follow.service';
import { BlockedUsersViewModel, BlockResponse } from 'app/modules/models/block-model';

@Component({
	selector: 'app-blocked-user',
	templateUrl: './blocked-user.component.html',
	styleUrls: ['./blocked-user.component.scss']
})
export class BlockedUserComponent implements OnInit {

	blockedUser: BlockedUsersViewModel[];

	pageNumber = 1;

	constructor(
		private httpClient: HttpClient,
		private blockService: BlockService,
		private followService: FollowService,
		private router: Router
	) { }

	ngOnInit(): void {
		this.blockService.getBlockUsers(this.pageNumber).subscribe({
			next: res => {
				this.blockedUser = res;
			},
			error: res => {
				console.log(res);
			}
		})
	}

	onScroll() {
		this.blockService.getBlockUsers(++this.pageNumber).subscribe((res) => {
			this.blockedUser.push(...res);
		});
	}

	onRedirectToProfile(userId: string): void {
		this.router.navigate(['/profile/', userId]);
	}

	block(block: BlockedUsersViewModel) {
		this.blockService.block(block.id).subscribe({
			next: (response: BlockResponse) => {

				let index = this.blockedUser.indexOf(block);

				this.blockedUser[index].isBlocked = response.isBlocked;
				// debugger
			},
			error: (err) => {
				console.log(err);
			}
		});
	}
}
