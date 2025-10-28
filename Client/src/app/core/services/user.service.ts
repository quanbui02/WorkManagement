import { Injectable, Injector } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import * as JWT from 'jwt-decode';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Subject, Observable } from 'rxjs';
import { shareReplay, retry, catchError, map } from 'rxjs/operators';
import { ResponseResult, ResponsePagination } from '../models/response-result';
import { User } from '../models/user';
import { BaseService } from './base.service';
import * as moment from 'moment';
import { environment } from '../../../../environments/environment';
import { HqCommonService } from './hq-common.service';
import { HqAuthenService } from '../guards/authen.service';
import { jwtDecode,JwtPayload } from 'jwt-decode';

@Injectable()
export class UserService extends BaseService {
   readonly USER_INFO_KEY = 'user_info';
   readonly authenticationEndpoint = `${environment.apiDomain.authenticationEndpoint}/users`;
   readonly authorizationEndpoint = `${environment.apiDomain.authorizationEndpoint}/users`;
   protected override  RETRY_COUNT: number = 0;
   protected override  REPLAY_COUNT: number = 10;

   public tokenReceived = new Observable<any>();

   constructor(
      http: HttpClient,
      injector: Injector,
      private _oauthService: OAuthService,
      private _commonService: HqCommonService,

   ) {
      super(http, injector, `${environment.apiDomain.workmanagementEndPoint}/Users`);
   }

   cachedUserInfo!: User;

   promises = [];
   isReady = false;

  returnPromises(): void {
    while (this.promises.length > 0) {
      const pr = this.promises.pop();
      const accessToken = this._oauthService.getAccessToken();

      if (accessToken) {
        const decoded = jwtDecode<JwtPayload>(accessToken);
        this.getCurrentUser();
      }
    }
    this.isReady = true;
  }

 protected override handleError(error: HttpErrorResponse, injector: Injector): never {
    console.warn('UserService custom handleError:', error.status);

    if (error.status === 401) {
      const authService = injector.get(HqAuthenService);
      authService.logout();
    } else {
      console.error('Lỗi UserService:', error.message);
    }

    throw error;
  }

   changePassword(item: any): Promise<ResponseResult> {
      const apiUrl = `${this.authenticationEndpoint}/ChangePassword`;
      return this.defaultPost(apiUrl, item);
   }


   CreateCB(obj: any) {
      const queryString = `${this.serviceUri}/CreateCB`;
      return this.defaultPost(queryString, obj);
   }



   // ApproveTDV(obj: any) {
   //    const queryString = `${this.authenticationEndpoint}/ApproveTdv`;
   //    return this.defaultPost(queryString, obj);
   // }

   // RemoveTDV(obj: any) {
   //    const queryString = `${this.authenticationEndpoint}/RemoveTdv`;
   //    return this.defaultPost(queryString, obj);
   // }

   // UpdateCB(obj: any) {
   //    const queryString = `${this.authenticationEndpoint}/UpdateCB`;
   //    return this.defaultPost(queryString, obj);
   // }

   getUsersByIdClient(id: any): Promise<ResponseResult> {
      const url = `${this.serviceUri}/getUsersByIdClient?idClient=${id}`;
      return this.defaultGet(url);
   }

   getUsersByIdShop(id: any): Promise<ResponseResult> {
      const url = `${this.serviceUri}/getUsersByIdShop?idShop=${id}`;
      return this.defaultGet(url);
   }

   UpdateGeneral(obj: any) {
      const queryString = `${this.serviceUri}/UpdateGeneral`;
      return this.defaultPost(queryString, obj);
   }
   UpdateOmicall(obj: any) {
      const queryString = `${this.serviceUri}/UpdateOmicall`;
      return this.defaultPost(queryString, obj);
   }
   UpdateBank(obj: any, code: string) {
      const queryString = `${this.serviceUri}/UpdateBank?code=${code}`;
      return this.defaultPost(queryString, obj);
   }
   UpdateCode(obj: any) {
      const queryString = `${this.serviceUri}/UpdateCode`;
      return this.defaultPost(queryString, obj);
   }
   UpdateCmt(obj: any) {
      const queryString = `${this.serviceUri}/UpdateCmt`;
      return this.defaultPost(queryString, obj);
   }

   GetsListKOL(key: string) {
      const url = `${this.serviceUri}/GetsListKOL?key=${key}`;
      return this.defaultGet(url);
   }

   GetsListLeader(key: string) {
      const url = `${this.serviceUri}/GetsListLeader?key=${key}`;
      return this.defaultGet(url);
   }

   getCaptchaUrl(): string {
      return `${this.authenticationEndpoint}/captcha?${Date.now()}&logSession=${sessionStorage.getItem('log_session_key')}`;
   }

   getBasicUserInfo(): User {
      const crrUser = new User();
      const accessToken = this._oauthService.getAccessToken();
      if (accessToken) {
         //const claims: any = JWT(accessToken);
         
         const claims = jwtDecode<any>(accessToken);
         if (claims) {
            crrUser.displayName = claims.displayname;
            crrUser.email = claims.email;
            // crrUser.fullName = claims.firstname.concat(' ', claims.lastname);
            crrUser.issuperuser = claims.issuperuser.toLowerCase() === 'true';
            crrUser.permissions = claims.permissions;
            crrUser.roleassign = claims.roleassign;
            crrUser.scope = claims.scope;
            crrUser.userId = claims.sub;
            crrUser.userName = claims.username;
            crrUser.avatar = claims.avatar;
         }
      }
      return crrUser;
   }

   async getCurrentUser(): Promise<User> {
      const crrUser = new User();
      const accessToken = this._oauthService.getAccessToken();
      if (accessToken) {
         const claims = jwtDecode<any>(accessToken);
         if (claims) {
            crrUser.displayName = claims.displayname;
            crrUser.email = claims.email;
            // crrUser.fullName = claims.firstname.concat(' ', claims.lastname);
            crrUser.issuperuser = claims.issuperuser.toLowerCase() === 'true';
            crrUser.permissions = claims.permissions;
            crrUser.scope = claims.scope;
            crrUser.userId = claims.sub;
            crrUser.idClient = claims.idClient;
            crrUser.idShop = claims.idShop;
            crrUser.idPortal = claims.idPortal;
            crrUser.userName = claims.username;
            crrUser.avatar = claims.avatar; //this._commonService.getFileUrl(claims.avatar);
            crrUser.roleassign = claims.roleassign; //list roles


            if (localStorage.getItem(this.USER_INFO_KEY)) {
               try {
                  return JSON.parse(localStorage.getItem(this.USER_INFO_KEY) ?? '{}');

               } catch (e) {

               }
            } else {
               await this.getCurrent().then(rs => {
                  if (rs.status) {
                     crrUser.email = rs.data.email;
                     crrUser.avatar = rs.data.avatar;
                     crrUser.id = rs.data.id;
                     crrUser.idClient = rs.data.idClient;
                     crrUser.idShop = rs.data.idShop;
                     crrUser.name = rs.data.name;
                     crrUser.phone = rs.data.phone;
                     crrUser.idProvince = rs.data.idProvince;
                     crrUser.idDistrict = rs.data.idDistrict;
                     crrUser.idWard = rs.data.idWard;
                     crrUser.address = rs.data.address;
                     crrUser.isDeleted = rs.data.isDeleted;
                     crrUser.idBank = rs.data.idBank;
                     crrUser.bankFullName = rs.data.bankFullName;
                     crrUser.bankNumber = rs.data.bankNumber;
                     crrUser.bankCardNumber = rs.data.bankCardNumber;
                     crrUser.bankBranch = rs.data.bankBranch;
                     crrUser.idBankNavigation = rs.data.idBankNavigation;
                     crrUser.isOmiCall = rs.data.isOmiCall;
                     crrUser.omiCallSipUser = rs.data.omiCallSipUser;
                     crrUser.omiCallSecretKey = rs.data.omiCallSecretKey;
                     crrUser.omiCallDomain = rs.data.omiCallDomain;
                     localStorage.setItem(this.USER_INFO_KEY, JSON.stringify(crrUser));
                  }
               });
            }
            return JSON.parse(localStorage.getItem(this.USER_INFO_KEY) ?? '{}');
         }
      }

      return crrUser;
   }

   GetByRoleName(RoleCode: string, offset?: number, limit?: number) {
      const queryString = `${this.authorizationEndpoint}/GetByRoleName?RoleCode=${RoleCode}&offset=${offset}&limit=${limit}`;
      return this.defaultGet(queryString);
   }
   managerment(term: string, pageIndex: number, pageSize: number, isActive: number, roleId: number, isDisable: number, isSuperUser: number) {
      const queryString = `${this.authorizationEndpoint}/managerment?term=${term}&pageIndex=${pageIndex}&pageSize=${pageSize}&isActive=${isActive}&roleId=${roleId}&isDisable=${isDisable}&isSuperUser=${isSuperUser}`;
      return this.defaultGet(queryString);
   }
   SearchNotInGroup(key: string, offset?: number, limit?: number, sortField?: string, isAsc: boolean = false) {
      const queryString = `${this.serviceUri}/SearchNotInGroup?key=${key}&offset=${offset}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}`;
      return this.defaultGet(queryString);
   }

   getCurrent() {
      return this.defaultGet(`${this.serviceUri}/getCurrent`);
   }

   GetByUserId(userId: number) {
      return this.defaultGet(`${this.serviceUri}/GetByUserId?userId=${userId}`);
   }

   GetByDomain(key: string, domainStatus: number, offset?: number, limit?: number, sortField: string = '', isAsc: Number = 0) {
      const queryString = `${this.serviceUri}/GetByDomain?key=${key}&domainStatus=${domainStatus}&offset=${offset}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}`;
      return this.defaultGet(queryString);
   }

   LoadConfig() {
      return this.defaultGet(`${this.serviceUri}/LoadConfig`);
   }

   UpdateIsLogout() {
      return this.defaultGet(`${this.serviceUri}/UpdateIsLogout`);
   }

   GetShort() {
      const queryString = `${this.serviceUri}/GetShort`;
      return this.defaultGet(queryString);
   }

}
