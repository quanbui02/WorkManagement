import { CommonModule } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { AppTopbarComponent } from './app-topbar/app-topbar.component';
import { environment } from '../../../environments/environment';
import { CommonService } from '../../core/services/common.service';
import { GlobalService } from '../../core/services/global.service';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { AppMenuComponent } from './app-menu/app-menu.component';
import { HtmenuService } from '../../core/services/htmenu.service';
enum MenuOrientation {
    STATIC,
    OVERLAY,
    SLIM,
    HORIZONTAL
}
@Component({
    selector: 'app-admin',
    standalone: true,
    imports: [RouterOutlet, CommonModule, ButtonModule, AppTopbarComponent, ScrollPanelModule, AutoCompleteModule, AppMenuComponent],
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
    menuClick?: boolean;
    profileMode = 'top';
    rotateMenuButton?: boolean;
    topbarMenuActive?: boolean;
    overlayMenuActive?: boolean;
    layoutMode: MenuOrientation = MenuOrientation.STATIC;
    staticMenuDesktopInactive = true;
    staticMenuMobileActive?: boolean;
    topbarItemClick?: boolean;
    appMenuModel: any[] | undefined;
    constructor(
        private _router: Router,
        public _commonService: CommonService,
        private injector: Injector,
        private _globalService: GlobalService,
        private _menuService: HtmenuService
    ) { }

    ngOnInit() {
    }

    onSwitchModule(moduleCode: number) {
        switch (moduleCode) {
            case 1: {
                this._router.navigate([environment.clientDomain.qthtDomain]);
                break;
            }
            default: { // frontend
                this._router.navigate([environment.clientDomain.appDomain]);
                break;
            }
        }
    }

    onMenuButtonClick(event: any) {
        this.menuClick = true;
        this.rotateMenuButton = !this.rotateMenuButton;
        this.topbarMenuActive = false;

        if (this.layoutMode === MenuOrientation.OVERLAY) {
            this.overlayMenuActive = !this.overlayMenuActive;
        } else {
            if (this.isDesktop()) {
                this.staticMenuDesktopInactive = !this.staticMenuDesktopInactive;
            } else {
                this.staticMenuMobileActive = !this.staticMenuMobileActive;
            }
        }

        event.preventDefault();
    }
    onTopbarMenuButtonClick(event: Event) {
        this.topbarItemClick = true;
        this.topbarMenuActive = !this.topbarMenuActive;

        this.hideOverlayMenu();

        event.preventDefault();
    }
    isDesktop() {
        return window.innerWidth > 1024;
    }
    isMobile() {
        return window.innerWidth <= 640;
    }

    isOverlay() {
        return this.layoutMode === MenuOrientation.OVERLAY;
    }

    isHorizontal() {
        return this.layoutMode === MenuOrientation.HORIZONTAL;
    }

    isSlim() {
        return this.layoutMode === MenuOrientation.SLIM;
    }

    hideOverlayMenu() {
        this.rotateMenuButton = false;
        this.overlayMenuActive = false;
        this.staticMenuMobileActive = false;
    }

    loadMenu() {
        this._menuService.getByIdPhanHe(environment.clientDomain.idPhanhe).then(rs => {
            if (rs.status) {
                this.appMenuModel = rs.data;
                console.log(this.appMenuModel);
            }
        });
    }
}
