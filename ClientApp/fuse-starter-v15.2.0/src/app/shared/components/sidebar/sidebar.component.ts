import { Component, OnInit } from '@angular/core';
import { SidebarService } from './sidebar.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  hasTag: any;

  constructor(private sidebarService: SidebarService) { }

  ngOnInit(): void {
    this.sidebarService.getHashtags().subscribe(res => {
      this.hasTag = res;
    });
  }
}
