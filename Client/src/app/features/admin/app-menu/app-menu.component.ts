import { Component, Input, OnInit, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TreeModule } from 'primeng/tree';
import { TreeNode } from 'primeng/api';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule, TreeModule],
  templateUrl: './app-menu.component.html',
  styleUrls: ['./app-menu.component.scss'],
})
export class AppMenuComponent implements OnInit, OnChanges {
  @Input() model: any[] | undefined;
  @Input() basePath: string = '';
  treeNodes: TreeNode[] = [];
  selectedNode: TreeNode | null = null;

  constructor(private _router: Router) { }

  ngOnInit() { }

  ngOnChanges() {
    if (this.model) {
      this.treeNodes = this.buildTree(this.model);
    }
  }

  buildTree(items: any[]): TreeNode[] {
    const map = new Map<number, TreeNode>();
    const roots: TreeNode[] = [];

    items.forEach(item => {
      map.set(item.id, {
        key: String(item.id),
        label: item.ten,
        data: item,
        children: [],
        expanded: true,
        icon: item.icon || 'pi pi-file'
      });
    });

    items.forEach(item => {
      const node = map.get(item.id)!;
      if (item.idCha && item.idCha > 0 && map.has(item.idCha)) {
        map.get(item.idCha)!.children!.push(node);
      } else {
        roots.push(node);
      }
    });

    return roots;
  }

  onNodeSelect(event: any) {
    const node = event.node;
    const url = node.data?.url;
    if (!url || url === '#') return;

    // Nếu url đã có đầy đủ path (vd: /admin/menu-management) thì dùng luôn
    // Nếu url ngắn (vd: /) thì prefix basePath
    let targetUrl = url;
    if (this.basePath && !url.startsWith(this.basePath)) {
      targetUrl = this.basePath + (url === '/' ? '' : url);
    }

    this._router.navigate([targetUrl]);
  }
}
