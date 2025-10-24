import { Injector } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError, retry, shareReplay, firstValueFrom } from 'rxjs';
import { ResponseResult } from '../models/response-result';

export abstract class BaseService {
  protected _http: HttpClient;
  protected _injector: Injector;
  protected serviceUri = '';

  protected  RETRY_COUNT = 0;
  protected  REPLAY_COUNT = 1;
  protected  LIMIT_DEFAULT = 1000;

  constructor(http: HttpClient, injector: Injector, serviceUri: string) {
    this._http = http;
    this._injector = injector;
    this.serviceUri = serviceUri;
  }

  // 🔹 Lấy chi tiết theo ID
  async getDetail(id: number | string): Promise<ResponseResult> {
    const url = `${this.serviceUri}/${id}`;
    return await this.defaultGet(url);
  }

  // 🔹 POST
  async post(item: any): Promise<ResponseResult> {
    return await firstValueFrom(
      this._http.post<ResponseResult>(this.serviceUri, item).pipe(
        catchError(err => this.handleError(err, this._injector))
      )
    );
  }

  // 🔹 PUT
  async put(id: number | string, item: any): Promise<ResponseResult> {
    const url = `${this.serviceUri}/${id}`;
    return await firstValueFrom(
      this._http.put<ResponseResult>(url, item).pipe(
        catchError(err => this.handleError(err, this._injector))
      )
    );
  }

  // 🔹 DELETE 1 bản ghi
  async delete(id: number): Promise<ResponseResult> {
    const url = `${this.serviceUri}/${id}`;
    return await firstValueFrom(
      this._http.delete<ResponseResult>(url).pipe(
        retry(this.RETRY_COUNT),
        catchError(err => this.handleError(err, this._injector))
      )
    );
  }

  // 🔹 DELETE nhiều bản ghi
  async deleteMany(lstId: string): Promise<ResponseResult> {
    const url = `${this.serviceUri}/DeleteMany/${lstId}`;
    return await firstValueFrom(
      this._http.delete<ResponseResult>(url).pipe(
        retry(this.RETRY_COUNT),
        catchError(err => this.handleError(err, this._injector))
      )
    );
  }

  // 🔹 GET cơ bản
  async defaultGet(apiUrl: string): Promise<ResponseResult> {
    return await firstValueFrom(
      this._http.get<ResponseResult>(apiUrl).pipe(
        shareReplay(this.REPLAY_COUNT),
        retry(this.RETRY_COUNT),
        catchError((err: HttpErrorResponse) => this.handleError(err, this._injector))
      )
    );
  }

  // 🔹 POST cơ bản (custom endpoint)
  async defaultPost(apiUrl: string, item: any): Promise<ResponseResult> {
    return await firstValueFrom(
      this._http.post<ResponseResult>(apiUrl, item).pipe(
        catchError(err => this.handleError(err, this._injector))
      )
    );
  }

  // 🔹 DELETE cơ bản (custom endpoint)
  async defaultDelete(apiUrl: string): Promise<ResponseResult> {
    return await firstValueFrom(
      this._http.delete<ResponseResult>(apiUrl).pipe(
        retry(this.RETRY_COUNT),
        catchError(err => this.handleError(err, this._injector))
      )
    );
  }

  // 🔹 GET bỏ qua cache client
  async getIgnoreClientCache(apiUrl: string): Promise<ResponseResult> {
    const options = {
      headers: new HttpHeaders({ 'Cache-Control': 'no-cache' })
    };

    return await firstValueFrom(
      this._http.get<ResponseResult>(apiUrl, options).pipe(
        shareReplay(this.REPLAY_COUNT),
        retry(this.RETRY_COUNT),
        catchError((err: HttpErrorResponse) => this.handleError(err, this._injector))
      )
    );
  }

  // 🔹 Xử lý lỗi chung
    protected handleError(error: HttpErrorResponse, injector: Injector): never {
    let customMessage = '';

    if (error.status === 401 || error.status === 403) {
        customMessage = `Bạn không có quyền truy cập (${error.status})`;
        // const authService = injector.get(VsAuthenService);
        // authService.logout();
    } else {
        customMessage = `${error.message} (${error.status})`;
    }

    // Gói lại thành 1 error mới (giữ nguyên HttpErrorResponse)
    const customError = {
        ...error,
        customMessage
    };

    console.error('API Error:', customMessage);
    throw customError;
    }
}
