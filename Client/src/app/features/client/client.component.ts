import { CommonModule } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { AppMenuComponent } from '../admin/app-menu/app-menu.component';
import { HtmenuService } from '../../core/services/htmenu.service';
import { FormsModule } from '@angular/forms';

enum MenuOrientation {
    STATIC,
    OVERLAY,
}

@Component({
    selector: 'app-client',
    standalone: true,
    imports: [RouterOutlet, CommonModule, ButtonModule, ScrollPanelModule, AppMenuComponent, FormsModule],
    templateUrl: './client.component.html',
    styleUrls: ['./client.component.scss']
})
export class ClientComponent implements OnInit {
    menuClick?: boolean;
    rotateMenuButton?: boolean;
    overlayMenuActive?: boolean;
    layoutMode: MenuOrientation = MenuOrientation.STATIC;
    staticMenuDesktopInactive = false;
    staticMenuMobileActive?: boolean;
    appMenuModel: any[] | undefined;

    constructor(
        private _router: Router,
        private _menuService: HtmenuService,
    ) { }

    ngOnInit() {
        this.loadMenu();
    }

    loadMenu() {
        this._menuService.getByIdPhanHe(2).then(rs => {
            if (rs.status) {
                this.appMenuModel = rs.data;
            }
        });
    }

    onMenuButtonClick(event: any) {
        this.menuClick = true;
        this.rotateMenuButton = !this.rotateMenuButton;

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

    isDesktop() {
        return window.innerWidth > 1024;
    }

    isOverlay() {
        return this.layoutMode === MenuOrientation.OVERLAY;
    }

    logOut() {
        localStorage.clear();
        sessionStorage.clear();
        window.location.href = '/login';
    }
}
