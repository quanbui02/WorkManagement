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
   PickSupportCTV(userId: number) {
      const queryString = `${this.serviceUri}/PickSupportCTV?userId=${userId}`;
      return this.defaultGet(queryString);
   }
   RemoveSupportCTV(userId: number) {
      const queryString = `${this.serviceUri}/RemoveSupportCTV?userId=${userId}`;
      return this.defaultGet(queryString);
   }
   changeIdType(userId: number, idType: number) {
      const queryString = `${this.serviceUri}/ChangeIdType?userId=${userId}&idType=${idType}`;
      return this.defaultPost(queryString, {});
   }
   PickRef(user: number, userId: number) {
      const queryString = `${this.serviceUri}/PickRef?user=${user}&userId=${userId}`;
      return this.defaultGet(queryString);
   }
   RemovePickRef(user: number, userId: number) {
      const queryString = `${this.serviceUri}/RemovePickRef?user=${user}&userId=${userId}`;
      return this.defaultGet(queryString);
   }
   RemoveSupportCTVForAdmin(userId: number) {
      const queryString = `${this.serviceUri}/RemoveSupportCTVForAdmin?userId=${userId}`;
      return this.defaultGet(queryString);
   }
   onSetLogout(userId: number, approve: boolean) {
      const queryString = `${this.serviceUri}/SetLogout?userId=${userId}&approve=${approve}`;
      return this.defaultGet(queryString);
   }
   Approved(userId: number, approve: boolean) {
      const queryString = `${this.serviceUri}/Approved`;
      const item: any = { userId: userId, approve: approve };
      return this.defaultPost(queryString, item);
   }
   ApprovedCmt(userId: number, approve: boolean) {
      const queryString = `${this.serviceUri}/ApprovedCmt`;
      const item: any = { userId: userId, approve: approve };
      return this.defaultPost(queryString, item);
   }
   AdminClient(userId: number, approve: boolean) {
      const queryString = `${this.serviceUri}/AdminClient`;
      const item: any = { userId: userId, approve: approve };
      return this.defaultPost(queryString, item);
   }

   setKPITdv(kpiSales: number, kpiVisit: number, kpiCoverage: number, kpiOrder: number, userId?: number, idUserGroup?: number): Promise<any> {
      const queryString = `${this.serviceUri}/SetKPITdv`;

      let params = new HttpParams()
         .append('kpiSales', kpiSales.toString())
         .append('kpiVisit', kpiVisit.toString())
         .append('kpiCoverage', kpiCoverage.toString())
         .append('kpiOrder', kpiOrder.toString());

      if (userId !== undefined && userId !== null) {
         params = params.append('userId', userId.toString());
      }
      if (idUserGroup !== undefined && idUserGroup !== null) {
         params = params.append('idUserGroup', idUserGroup.toString());
      }
      return this._http.post(queryString, null, { params }).toPromise();
   }

   GetByListId(listId: string) {
      const queryString = `${this.serviceUri}/GetByListId?listId=${listId}`;
      return this.defaultGet(queryString);
   }
   AutoComplete(key: string, offset?: number, limit?: number) {
      const queryString = `${this.serviceUri}/AutoComplete?key=${key}&offset=${offset}&limit=${limit}`;
      return this.defaultGet(queryString);
   }

   AutoCompleteNT(key: string, offset?: number, limit?: number) {
      const queryString = `${this.serviceUri}/AutoCompleteNT?key=${key}&offset=${offset}&limit=${limit}`;
      return this.defaultGet(queryString);
   }

   AutoCompleteByRef(key: string, except: string, userId?: number, offset?: number, limit?: number) {
      const queryString = `${this.serviceUri}/AutoCompleteByRef?key=${key}&except=${except}&userId=${userId}&offset=${offset}&limit=${limit}`;
      return this.defaultGet(queryString);
   }

   AutoCompleteWithChoise(key: string, except: string, userId?: number, idUserGroups?: number, idUserTypes?: number, idLocation?: number, offset?: number, limit?: number) {
      const queryString = `${this.serviceUri}/AutoCompleteWithChoise?key=${key}&except=${except}&userId=${userId}&idUserGroups=${idUserGroups}&idUserTypes=${idUserTypes}&idLocation=${idLocation}&offset=${offset}&limit=${limit}`;
      return this.defaultGet(queryString);
   }

   AutoCompleteWithChoiseTdv(key: string, except: string, userId?: number, idUserGroups?: number, isLeader?: number, offset?: number, limit?: number) {
      const queryString = `${this.serviceUri}/AutoCompleteWithChoiseTdv?key=${key}&except=${except}&userId=${userId}&idUserGroups=${idUserGroups}&isLeader=${isLeader}&offset=${offset}&limit=${limit}`;
      return this.defaultGet(queryString);
   }

   SearchNotInClient(key: string, offset?: number, limit?: number, sortField?: string, isAsc: boolean = false) {
      const queryString = `${this.serviceUri}/SearchNotInClient?key=${key}&offset=${offset}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}`;
      return this.defaultGet(queryString);
   }
   SearchInClient(key: string, idClient: number, offset?: number, limit?: number, sortField?: string, isAsc: boolean = false) {
      const queryString = `${this.serviceUri}/SearchInClient?key=${key}&idClient=${idClient}&offset=${offset}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}`;
      return this.defaultGet(queryString);
   }

   gets(key: string, userId: number, status: number, offset?: number, limit?: number, sortField?: string, isAsc: boolean = false, isApproved: number = -1, idType: number = -1, isCheckingUser: boolean = false) {
      const queryString = `${this.serviceUri}?key=${key}&userId=${userId}&status=${status}&offset=${offset}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}&isApproved=${isApproved}&idType=${idType}&isCheckingUser=${isCheckingUser}`;
      return this.defaultGet(queryString);
   }

   GetsInvited(key: string, offset?: number, limit?: number, sortField?: string, isAsc: boolean = false) {
      const queryString = `${this.serviceUri}/GetsInvited?key=${key}&offset=${offset}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}`;
      return this.defaultGet(queryString);
   }

   GetsInvitedKOLPlus(userId: number, key: string, fromDate: Date, toDate: Date, idProvince?: number, offset?: number, limit?: number, sortField?: string, isAsc: boolean = false) {
      let fDate;
      let tDate;
      if (fromDate) {
         fDate = fromDate.toISOString();
      }
      if (toDate) {
         tDate = toDate.toISOString();
      }

      const queryString = `${this.serviceUri}/GetsInvitedKOLPlus?userId=${userId}&key=${key}&fromDate=${fDate}&toDate=${tDate}&offset=${offset}&idProvince=${idProvince}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}`;
      return this.defaultGet(queryString);
   }

   GetsForSupport(key: string, idSupport: number, status: number, offset?: number, limit?: number, sortField?: string, isAsc: boolean = false) {
      const queryString = `${this.serviceUri}/GetsForSupport?key=${key}&idSupport=${idSupport}&status=${status}&offset=${offset}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}`;
      return this.defaultGet(queryString);
   }

   GetsForSupportCSKH(key: string, fromDate: Date, toDate: Date, idSupport: number, idProvince: number, status: number, isKol: number, userGroup: number, userType: number, idRef: number, offset?: number, limit?: number, sortField?: string, isAsc: boolean = false) {
      let fDate;
      let tDate;
      if (fromDate) {
         fDate = fromDate.toISOString();
      }
      if (toDate) {
         tDate = toDate.toISOString();
      }

      const queryString = `${this.serviceUri}/GetsForSupportCSKH?key=${key}&fromDate=${fDate}&toDate=${tDate}&idSupport=${idSupport}&idProvince=${idProvince}&status=${status}&isKol=${isKol}&isKol=${isKol}&idUserGroup=${userGroup}&idUserType=${userType}&idRef=${idRef}&offset=${offset}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}`;
      return this.defaultGet(queryString);
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

   GetByUserIdOrder(userId: number) {
      return this.defaultGet(`${this.serviceUri}/GetByUserIdOrder?userId=${userId}`);
   }

   GetListTn(key: string, offset?: number, limit?: number, sortField?: string, isAsc: boolean = false) {
      const queryString = `${this.serviceUri}/GetListTn?key=${key}&offset=${offset}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}`;
      return this.defaultGet(queryString);
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

   GetListUserSip() {
      const queryString = `${this.serviceUri}/GetListUserSip`;
      return this.defaultGet(queryString);
   }
   GetListUserInstructor() {
      const queryString = `${this.serviceUri}/GetListUserInstructor`;
      return this.defaultGet(queryString);
   }
   GetShort() {
      const queryString = `${this.serviceUri}/GetShort`;
      return this.defaultGet(queryString);
   }

   GetShortByPharmacies(userId: number, offset?: number, limit?: number) {
      const queryString = `${this.serviceUri}/GetShortByPharmacies?userId=${userId}&offset=${offset}&limit=${limit}`;
      return this.defaultGet(queryString);
   }

   GetShortTdvByPharmacies(userId: number, offset?: number, limit?: number) {
      const queryString = `${this.serviceUri}/GetShortTdvByPharmacies?userId=${userId}&offset=${offset}&limit=${limit}`;
      return this.defaultGet(queryString);
   }

   GetCustomerCSKH(key: string, userId: number, idType: number, idProvince: number, idUserType: number, idUserGroup: number, isDeleted: number, isApproved: number, offset?: number, limit?: number, sortField?: string, isAsc: boolean = false) {
      const queryString = `${this.serviceUri}/GetCustomerCSKH?key=${key}&userId=${userId}&idType=${idType}&idProvince=${idProvince}&idUserType=${idUserType}&idUserGroup=${idUserGroup}&isDeleted=${isDeleted}&isApproved=${isApproved}&offset=${offset}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}`;
      return this.defaultGet(queryString);
   }

   GetByPharmacies(key: string, fromDate: string, toDate: string, idProvince: number, idDistrict: number, idWard: number, idUserType: number, idUserGroup: number, isApproved: number, offset?: number, limit?: number, sortField?: string, isAsc: boolean = false) {
      const queryString = `${this.serviceUri}/GetByPharmacies?key=${key}&fromDate=${fromDate}&toDate=${toDate}&idProvince=${idProvince}&idDistrict=${idDistrict}&idWard=${idWard}&idUserType=${idUserType}&idUserGroup=${idUserGroup}&isApproved=${isApproved}&offset=${offset}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}`;
      return this.defaultGet(queryString);
   }

   ViewLocationInMap(key: string, fromDate: string, toDate: string, userId: number, idProvince: number, idUserType: number, idUserGroup: number, offset?: number, limit?: number, sortField?: string, isAsc: boolean = false) {
      const queryString = `${this.serviceUri}/ViewLocationInMap?key=${key}&fromDate=${fromDate}&toDate=${toDate}&userId=${userId}&idProvince=${idProvince}&idUserType=${idUserType}&idUserGroup=${idUserGroup}&offset=${offset}&limit=${limit}&sortField=${sortField}&isAsc=${isAsc}`;
      return this.defaultGet(queryString);
   }

   GetUserTempById(userId: number) {
      return this.defaultGet(`${this.serviceUri}/GetUserTempById?userId=${userId}`);
   }

   AutocompleteUsersEditOrder(key: string, offset?: number, limit?: number) {
      const queryString = `${this.serviceUri}/AutocompleteUsersEditOrder?key=${key}&offset=${offset}&limit=${limit}`;
      return this.defaultGet(queryString);
   }

   GetUsersEditOrder(userId: number) {
      const queryString = `${this.serviceUri}/GetUsersEditOrder?userId=${userId}`;
      return this.defaultGet(queryString);
   }

   GetTaxUpdateOrder(userId: number) {
      const queryString = `${this.serviceUri}/GetTaxUpdateOrder?userId=${userId}`;
      return this.defaultGet(queryString);
   }

   GetTax(userId: number) {
      const queryString = `${this.serviceUri}/GetTax?userId=${userId}`;
      return this.defaultGet(queryString);
   }

   AssignCertificateType(userId: number, id: string, optionAssign: number): Promise<any> {
      const queryString = `${this.serviceUri}/AssignCertificateType`;

      let params = new HttpParams()
         .append('userId', userId.toString())
         .append('id', id.toString())
         .append('optionAssign', optionAssign.toString());

      return this._http.post(queryString, null, { params }).toPromise();
   }

   GetListAsmRsm() {
      var queryString = `${this.serviceUri}/GetListAsmRsm`;
      return this.defaultGet(queryString);
   }

   GetTdvByOrganization() {
      var queryString = `${this.serviceUri}/GetTdvByOrganization`;
      return this.defaultGet(queryString);
   }

}
