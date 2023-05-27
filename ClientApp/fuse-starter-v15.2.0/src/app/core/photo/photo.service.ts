import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PhotoUploadResponse } from 'app/modules/models/photo-model';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  baseUrl: string = environment.baseUrl + 'photos/';

  constructor(private _httpClient: HttpClient) { }

  uploadPhoto(file: FormData): Observable<PhotoUploadResponse> {

    const requestUrl = `${this.baseUrl}profile`;

    return this._httpClient.post<PhotoUploadResponse>(requestUrl, file);

  }

  uploadCoverPhoto(file: FormData): Observable<PhotoUploadResponse> {

    const requestUrl = `${this.baseUrl}cover`;

    return this._httpClient.post<PhotoUploadResponse>(requestUrl, file);

  }
}
