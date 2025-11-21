import { AfterViewInit, Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { environment } from '../../../../../environments/environment';
import { Observable, Subject, Subscription } from 'rxjs';
import { User } from '../../../core/models/user';
import { VsMySetting } from '../../../core/models/ccmysetting';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomRouterService } from '../../../core/services/custom-router.service';
import { UserService } from '../../../core/services/user.service';
import { VsMySettingService } from '../../../core/services/ccmysetting.service';
import { EventEmitterService } from '../../../core/services/eventemitter.service';
import { Title } from '@angular/platform-browser';
import { GlobalService } from '../../../core/services/global.service';
import { AppComponent } from '../../../app.component';
import { CommonService } from '../../../core/services/common.service';
import { CommonModule } from '@angular/common';
import { AdminComponent } from '../admin.component';

@Component({
  selector: 'app-topbar',
  templateUrl: './app-topbar.component.html',
  styleUrls: ['./app-topbar.component.scss'],
  imports: [CommonModule]
})
export class AppTopbarComponent implements OnInit, OnDestroy, AfterViewInit {
    environment = environment;
    searchKey = '';

    _unSubscribeAll = new Subject<any>();
    _sub?: Subscription;
    currentRoute = '';
    fileApi = '';

    position = '';
    avatarUrl = '';
    crrUser?: User;
    display: any;
    mySetting = new VsMySetting();
    mySettingEdit = new VsMySetting();
    formGroup = [];
    balance = 0;
    balanceBlock = 0;
    items?: Observable<any[]>;
    list?: Array<any>;
    citiesRef?: Array<any>;
    docRef: any;
    CallTransactions?: {
        TransactionId: string
    }
    titleChange = false;
    interval: any;
    @Output() vsclosePopup = new EventEmitter<any>();

    constructor(
        public app: AdminComponent,
        private _activatedRoute: ActivatedRoute,
        private _customRouteService: CustomRouterService,
        private _router: Router,
        private _userService: UserService,
        public _globalService: GlobalService,
        public _commonService: CommonService,
        private _mySettingService: VsMySettingService,
        private _EventEmitterService: EventEmitterService,
        private titleService: Title,
    ) {}

    async ngOnInit() {
        this.mySetting = this._mySettingService.getCurrentSetting();
        this.crrUser = await this._userService.getCurrentUser();
        console.log(this.crrUser);
      
        //this._EventEmitterService.updateCountIconMessageChat.subscribe(item => this.mySetting = item);
    }

    ngAfterViewInit(): void {
      //throw new Error('Method not implemented.');
    }
    ngOnDestroy(): void {
      //throw new Error('Method not implemented.');
    }

}
