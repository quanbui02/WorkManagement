# WorkManagement Project Rules

## 1. Cấu trúc Project
- **Backend**: `Api/WorkManagement/` — .NET 9 Web API
- **Data Layer**: `Api/Work.DataContext/` — EF Core + Identity
- **Frontend**: `Client/` — Angular 19 + PrimeNG + Tailwind (sass/scss)

## 2. Backend Conventions

### Namespace & Thư mục
- Controllers Admin: `Api/WorkManagement/Controllers/Admins/` → namespace `WorkManagement.Controllers.Admins`
- Controllers Client: `Api/WorkManagement/Controllers/Clients/` → namespace `WorkManagement.Controllers.Clients`
- Services Admin: `Api/WorkManagement/Services/Admins/` → namespace `WorkManagement.Services.Admins`
- Services Client: `Api/WorkManagement/Services/Clients/` → namespace `WorkManagement.Services.Clients`
- Models: `Api/WorkManagement/Models/Admin/` hoặc `Models/Request/`
- Common: `Api/WorkManagement/Common/` — Result, UserInfo, Caching, Attributes, Base

### Patterns bắt buộc
- **Service pattern**: Interface `I{Name}Service` + class `{Name}Service` khai báo trong CÙNG 1 FILE
- **DI**: Đăng ký Scoped trong `ConfigService.cs` → method `AddCustomService`
- **Response**: Luôn dùng `Result<T>.Success(data, totalRecord, message)` hoặc `Result<T>.Error(message)`
- **DB Context**: 
  - `AppDbContext` → Identity, Permission, Role (bảng hệ thống)
  - `WorkManagementContext` → Business data (bảng nghiệp vụ)
- **Base class**: 
  - `BaseController<TModel, TInterface>` cho CRUD generic
  - `BaseService<TModel, TDbContext>` cho CRUD generic

### Phân quyền
- Admin controllers dùng `[Authorize(Roles = "Admin")]` — không cần PermissionDefinition
- Client/Business controllers dùng `[PermissionDefinition("Tên quyền", index)]` trên từng action
- SuperUser bypass mọi bitmask check
- Sau khi thêm PermissionDefinition mới → cần gọi `POST /SyncPermission/Sync`

## 3. Frontend Conventions

### Cấu trúc thư mục
```
Client/src/app/
├── core/          # Services, Guards, Interceptors, Models, Constants
├── shared/        # Components, Directives, Pipes, Configs dùng chung
├── features/      # Các feature module
│   ├── admin/     # Admin shell + sub-features
│   ├── client/    # Client shell + sub-features
│   ├── auth/      # Authentication
│   ├── dashboard/ # Dashboard
│   └── users/     # Users
└── layout/        # Header, Sidebar layout
```

### Patterns bắt buộc
- **Component**: Luôn dùng `standalone: true`, import PrimeNG modules trực tiếp
- **Service**: 
  - Kế thừa `BaseService` (từ `core/services/base.service.ts`)
  - `providedIn: 'root'`
  - URL = `${environment.apiDomain.workmanagementEndPoint}/{ControllerName}`
  - Return `Promise<ResponseResult>` (không dùng Observable trực tiếp)
- **Routing**: 
  - Feature modules dùng `loadChildren` với NgModule
  - Sub-features dùng `loadComponent` (standalone component)
  - Guard: `AuthGuard` cho route cần đăng nhập
- **Styles**: 
  - LUÔN dùng SCSS (file `.scss` riêng cho component)
  - Style dùng chung → `styles.scss` hoặc shared
  - KHÔNG dùng inline CSS, KHÔNG dùng CSS thuần
- **Response handling**: Check `rs.status` trước khi dùng `rs.data`

### PrimeNG
- Theme: Aura preset (`@primeuix/themes/aura`)
- Import module trực tiếp: `ButtonModule`, `TableModule`, `DialogModule`, v.v.
- KHÔNG dùng `p-` prefix import, dùng module import

## 4. Token Optimization Rules (cho AI Assistant)

### KHÔNG làm
- KHÔNG đọc lại file đã đọc trong cùng conversation (trừ khi file bị thay đổi)
- KHÔNG giải thích code trước khi viết — viết trước, giải thích ngắn gọn sau
- KHÔNG tạo file test trừ khi được yêu cầu
- KHÔNG refactor code không liên quan đến yêu cầu
- KHÔNG đọc `node_modules/`, `bin/`, `obj/`, `.vs/`, `Migrations/`
- KHÔNG đọc `PROJECT_CONTEXT.md` nếu đã có rules này (rules đầy đủ hơn)

### LUÔN làm
- Đọc rules này ĐẦU TIÊN trước khi bắt tay code
- Dùng `grep_search` để tìm pattern thay vì đọc toàn bộ file
- Đọc chỉ file liên quan, đọc 1 lần, ghi nhớ
- Khi thêm feature mới → follow workflow `/add-be-feature`, `/add-fe-feature`, hoặc `/add-full-feature`
- Khi chạy project → follow workflow `/run-project`
- Trả lời ngắn gọn, đúng trọng tâm, bằng tiếng Việt
