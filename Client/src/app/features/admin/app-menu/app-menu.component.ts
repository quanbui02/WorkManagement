import { Component, Input, OnInit } from '@angular/core';
import { HtmenuService } from '../../../core/services/htmenu.service';

@Component({
  selector: 'app-menu',
  templateUrl: './app-menu.component.html',
  styleUrls: ['./app-menu.component.scss'],
})
export class AppMenuComponent implements OnInit {
  @Input() model: any[] | undefined;
  constructor() { }

  ngOnInit() {
  }

}
