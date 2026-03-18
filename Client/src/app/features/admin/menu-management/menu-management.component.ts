import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TreeModule } from 'primeng/tree';
import { ContextMenuModule } from 'primeng/contextmenu';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TreeNode } from 'primeng/api';
import { MenuItem } from 'primeng/api';
import { HtmenuService } from '../../../core/services/htmenu.service';
import { environment } from '../../../../environments/environment';
import { InputNumberModule } from 'primeng/inputnumber';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

interface HtMenuForm {
  id: number;
  idCha: number | null;
  ma: string;
  ten: string;
  url: string;
  icon: string;
  maMau: string;
  thuTu: number | null;
  trangThai: number | null;
  phanHe: number | null;
  phanQuyen: string;
  idDuongDan: string;
  tenDuongDan: string;
}

@Component({
  selector: 'app-menu-management',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TreeModule,
    ContextMenuModule,
    ButtonModule,
    InputTextModule,
    SelectModule,
    InputNumberModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './menu-management.component.html',
  styleUrls: ['./menu-management.component.scss']
})
export class MenuManagementComponent implements OnInit {
  treeNodes: TreeNode[] = [];
  selectedNode: TreeNode | null = null;
  contextMenuItems: MenuItem[] = [];
  flatMenuList: any[] = [];

  phanHeOptions = [
    { label: 'Quản trị hệ thống', value: 1 },
    { label: 'Work', value: 2 },
  ];

  trangThaiOptions = [
    { label: 'Sử dụng', value: 1 },
    { label: 'Không sử dụng', value: 0 },
  ];

  form: HtMenuForm = this.getEmptyForm();

  capCha: string = '';
  isEditing = false;

  constructor(
    private _menuService: HtmenuService,
    private _messageService: MessageService
  ) { }

  ngOnInit() {
    this.initContextMenu();
    this.loadMenu();
  }

  getEmptyForm(): HtMenuForm {
    return {
      id: 0,
      idCha: null,
      ma: '',
      ten: '',
      url: '',
      icon: '',
      maMau: '',
      thuTu: null,
      trangThai: 1,
      phanHe: environment.clientDomain.idPhanhe,
      phanQuyen: '',
      idDuongDan: '',
      tenDuongDan: ''
    };
  }

  initContextMenu() {
    this.contextMenuItems = [
      {
        label: 'Thêm menu con',
        icon: 'pi pi-plus',
        command: () => this.addChild()
      },
      {
        label: 'Thêm menu cùng cấp',
        icon: 'pi pi-plus-circle',
        command: () => this.addSibling()
      },
      {
        separator: true
      },
      {
        label: 'Xóa',
        icon: 'pi pi-trash',
        command: () => this.deleteMenu()
      }
    ];
  }

  async loadMenu() {
    try {
      const rs = await this._menuService.getByIdPhanHe(environment.clientDomain.idPhanhe);
      if (rs.status) {
        this.flatMenuList = rs.data || [];
        this.treeNodes = this.buildTree(this.flatMenuList);
      }
    } catch (err) {
      console.error('Lỗi khi load menu:', err);
    }
  }

  buildTree(items: any[]): TreeNode[] {
    const map = new Map<number, TreeNode>();
    const roots: TreeNode[] = [];

    // Tạo map tất cả node
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

    // Gắn con vào cha
    items.forEach(item => {
      const node = map.get(item.id)!;
      if (item.idCha && map.has(item.idCha)) {
        map.get(item.idCha)!.children!.push(node);
      } else {
        roots.push(node);
      }
    });

    return roots;
  }

  onNodeSelect(event: any) {
    const node = event.node;
    this.loadDetail(node.data.id);
  }

  async loadDetail(id: number) {
    try {
      const rs = await this._menuService.getDetailById(id);
      if (rs.status) {
        const data = rs.data;
        this.form = {
          id: data.id,
          idCha: data.idCha,
          ma: data.ma || '',
          ten: data.ten || '',
          url: data.url || '',
          icon: data.icon || '',
          maMau: data.maMau || '',
          thuTu: data.thuTu,
          trangThai: data.trangThai,
          phanHe: data.phanHe,
          phanQuyen: data.phanQuyen || '',
          idDuongDan: data.idDuongDan || '',
          tenDuongDan: data.tenDuongDan || ''
        };
        this.isEditing = true;

        // Tìm tên cấp cha
        if (data.idCha) {
          const parent = this.flatMenuList.find(m => m.id === data.idCha);
          this.capCha = parent ? parent.ten : '';
        } else {
          this.capCha = '(Gốc)';
        }
      }
    } catch (err) {
      console.error('Lỗi load detail:', err);
    }
  }

  onNodeRightClick(event: any) {
    this.selectedNode = event.node;
  }

  addChild() {
    if (!this.selectedNode) return;
    this.form = this.getEmptyForm();
    this.form.idCha = Number(this.selectedNode.key);
    this.form.phanHe = this.selectedNode.data.phanHe;
    this.capCha = this.selectedNode.label || '';
    this.isEditing = false;
  }

  addSibling() {
    if (!this.selectedNode) return;
    this.form = this.getEmptyForm();
    this.form.idCha = this.selectedNode.data.idCha;
    this.form.phanHe = this.selectedNode.data.phanHe;
    this.isEditing = false;

    if (this.form.idCha) {
      const parent = this.flatMenuList.find(m => m.id === this.form.idCha);
      this.capCha = parent ? parent.ten : '';
    } else {
      this.capCha = '(Gốc)';
    }
  }

  async deleteMenu() {
    if (!this.selectedNode) return;
    const id = Number(this.selectedNode.key);

    try {
      const rs = await this._menuService.deleteMenu(id);
      this._messageService.add({
        severity: 'success',
        summary: 'Thành công',
        detail: 'Xóa menu thành công'
      });
      this.form = this.getEmptyForm();
      this.capCha = '';
      this.isEditing = false;
      this.selectedNode = null;
      await this.loadMenu();
    } catch (err) {
      this._messageService.add({
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Xóa menu thất bại'
      });
    }
  }

  async save() {
    if (!this.form.ten) {
      this._messageService.add({
        severity: 'warn',
        summary: 'Cảnh báo',
        detail: 'Vui lòng nhập tên menu'
      });
      return;
    }

    try {
      const rs = await this._menuService.post(this.form);
      if (rs.status) {
        this._messageService.add({
          severity: 'success',
          summary: 'Thành công',
          detail: this.isEditing ? 'Cập nhật menu thành công' : 'Thêm menu thành công'
        });
        await this.loadMenu();

        // Nếu tạo mới, chuyển sang edit mode
        if (!this.isEditing && rs.data?.id) {
          this.loadDetail(rs.data.id);
        }
      }
    } catch (err) {
      this._messageService.add({
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Lưu menu thất bại'
      });
    }
  }

  cancel() {
    this.form = this.getEmptyForm();
    this.capCha = '';
    this.isEditing = false;
    this.selectedNode = null;
  }
}
