import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';

@Injectable({
   providedIn: 'root'
})
export class SidebarService {
   baseUrl = environment.baseUrl + 'tweets/';

   constructor(private httpClient: HttpClient) { }

   getHashtags() {
      const requestUrl = `${this.baseUrl}hashtags?page=1`;
      return this.httpClient.get<any>(requestUrl);
   }
}
