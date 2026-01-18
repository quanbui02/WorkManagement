import { firstValueFrom } from 'rxjs';
import { Injectable, Injector } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BaseService } from './base.service';
import { environment } from '../../../environments/environment';
import { ResponseResult } from '../models/response-result';
import { catchError } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class AiAsissTantService extends BaseService {

    constructor(http: HttpClient, injector: Injector) {
        super(http, injector, `${environment.apiDomain.workmanagementEndPoint}/AiAssistant`);
    }

    chat(item: any): Promise<ResponseResult> {
      const apiUrl = `${this.serviceUri}/chat`;
      return this.defaultPost(apiUrl, item);
   }
}
