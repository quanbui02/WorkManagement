---
description: Thêm feature mới cho Frontend (Angular 19 + PrimeNG)
---

# Thêm Feature Frontend

## Quy trình thêm 1 feature mới cho Frontend Angular

### 1. Tạo Service (nếu cần gọi API mới)
- **File**: `Client/src/app/core/services/{feature}.service.ts`
- **Bắt buộc** kế thừa `BaseService`
- `providedIn: 'root'` → không cần import vào module
- URL endpoint lấy từ `environment.apiDomain.workmanagementEndPoint`

```typescript
// Mẫu Service
@Injectable({ providedIn: 'root' })
export class {Feature}Service extends BaseService {
    constructor(http: HttpClient, injector: Injector) {
        super(http, injector, `${environment.apiDomain.workmanagementEndPoint}/{ControllerName}`);
    }

    getList(key: string, offset: number, limit: number): Promise<ResponseResult> {
        const url = `${this.serviceUri}?key=${key}&offset=${offset}&limit=${limit}`;
        return this.defaultGet(url);
    }

    save(item: any): Promise<ResponseResult> {
        return this.post(item);
    }
}
```

### 2. Tạo Component (standalone)
- **Thư mục**: `Client/src/app/features/admin/{feature}/` (admin) hoặc `Client/src/app/features/client/{feature}/` (client)
- **Files**: `{feature}.component.ts`, `{feature}.component.html`, `{feature}.component.scss`
- Component phải là **standalone** (`standalone: true`)
- Import PrimeNG modules trực tiếp trong component

```typescript
// Mẫu Component
@Component({
    selector: 'app-{feature}',
    standalone: true,
    imports: [CommonModule, FormsModule, /* PrimeNG modules */],
    templateUrl: './{feature}.component.html',
    styleUrls: ['./{feature}.component.scss']
})
export class {Feature}Component implements OnInit {
    constructor(private _{feature}Service: {Feature}Service) {}

    ngOnInit() {
        this.loadData();
    }

    async loadData() {
        const rs = await this._{feature}Service.getList('', 0, 20);
        if (rs.status) {
            // xử lý data
        }
    }
}
```

### 3. Đăng ký Route
- **Admin feature**: Mở `Client/src/app/features/admin/admin.module.ts`, thêm route với `loadComponent`
- **Client feature**: Mở `Client/src/app/features/client/client.module.ts`, thêm route với `loadComponent`

```typescript
// Trong children routes
{
    path: '{feature-path}',
    loadComponent: () =>
        import('./{feature}/{feature}.component')
            .then(c => c.{Feature}Component)
},
```

### 4. SCSS
- Viết SCSS trong file `.scss` của component, KHÔNG dùng inline styles
- Nếu style dùng chung → viết vào `Client/src/styles.scss` hoặc shared styles
- KHÔNG dùng CSS thuần, luôn dùng SCSS

### 5. Thêm vào Menu (nếu cần)
- Dùng API `POST /HtMenu` để tạo menu item mới trong DB
- Menu sẽ tự render dựa trên `appMenuModel` từ `HtmenuService.getByIdPhanHe(idPhanHe)`
  - `idPhanHe = 1` → Admin menu
  - `idPhanHe = 2` → Client menu

### Checklist
- [ ] Service kế thừa BaseService (nếu cần API)
- [ ] Standalone component với PrimeNG
- [ ] SCSS riêng cho component
- [ ] Route trong module tương ứng (loadComponent)
- [ ] Menu item (nếu cần)
