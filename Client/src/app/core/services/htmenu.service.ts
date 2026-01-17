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
export class HtmenuService extends BaseService {

    constructor(http: HttpClient, injector: Injector) {
        super(http, injector, `${environment.apiDomain.workmanagementEndPoint}/HtMenu`);
    }

    getByIdPhanHe(idPhanHe: number): Promise<ResponseResult> {
        const url = `${this.serviceUri}/?id=${idPhanHe}`;
        return this.defaultGet(url);
    }

    getDetailById(id: number): Promise<ResponseResult> {
        const url = `${this.serviceUri}/GetDetailById?id=${id}`;
        return this.defaultGet(url);
    }

    searchTree(phanHeId: number, trangThai: number): Promise<ResponseResult> {
        const url = `${this.serviceUri}/searchTree?phanHeId=${phanHeId}&trangThai=${trangThai}`;
        return this.defaultGet(url);
    }
    
    async deleteCustom(id: number): Promise<ResponseResult> {
        const url = `${this.serviceUri}/deleteCustom/${id}`;

        return await firstValueFrom(
            this._http.put<ResponseResult>(url, { id })
                .pipe(catchError((err: HttpErrorResponse) => this.handleError(err, this._injector)))
        );
    }

    async pinMenuItem(idMenu: number): Promise<ResponseResult> {
        const url = `${this.serviceUri}/PinMenuItem?menuId=${idMenu}`;

        return await firstValueFrom(
            this._http.post<ResponseResult>(url, null)
                .pipe(catchError(err => this.handleError(err, this._injector)))
        );
    }

    getsPinMenu(idPhanHe: number): Promise<ResponseResult> {
        const url = `${this.serviceUri}/GetsPinMenu?phanHeId=${idPhanHe}`;
        return this.defaultGet(url);
    }

    async unPinMenuItem(idMenu: number): Promise<ResponseResult> {
        const url = `${this.serviceUri}/UnPinMenuItem?id=${idMenu}`;

        return await firstValueFrom(
            this._http.post<ResponseResult>(url, null)
                .pipe(catchError(err => this.handleError(err, this._injector)))
        );
    }
}
