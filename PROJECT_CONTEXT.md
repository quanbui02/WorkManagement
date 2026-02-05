# Project Context (Auto-generated)

## Overview
- Solution: `WorkManagement.sln`
- Backend: `Api/WorkManagement` (.NET 9 Web API)
- Data layer: `Api/Work.DataContext` (EF Core + Identity)
- Frontend: `Client` (Angular 19 + PrimeNG + Tailwind)

## Backend Summary
### Entry & Config
- `Api/WorkManagement/Program.cs`: CORS, Swagger, JWT (RSA keys), Identity, DbContexts, DI services.
- `Api/WorkManagement/appsettings.json`: `DefaultConnection`, `Jwt` (issuer/audience/keys), `spring:application:rawname=workmanagement`.
- `Api/WorkManagement/ConfigService.cs`: DI registrations.

### Controllers
- Admin: `AuthController`, `AccountsController`, `AppRolesController`, `AppPermissionsController`, `HtMenuController`, `SyncPermissionController`
- Client: `WmUsersController`
- AI: `AiAssistantController`
- Base: `BaseController<T>` with `Get/Delete/DeleteRange` + `PermissionDefinition`

### Services
- Auth/Role/Permission: `AuthServices`, `AccountServices`, `AppRolesServices`, `AppPermissionsService`, `SyncPermissionServices`
- Client: `WmUsersService`, `UsersService`
- AI: `AIAsissTantService` (Ollama localhost)
- Base: `BaseService<T>`

### Permission System
- Attribute: `PermissionDefinitionAttribute` checks permissions bitmask by controller/index.
- Sync: `SyncPermissionServices.ScanAndSavePermissionsAsync()` scans controllers/actions with `PermissionDefinition`.
- Sync endpoint: `POST /SyncPermission/Sync` (Admin only).
- Permissions stored in `AppController`, `AppPermission`, `RolePermission`, `AppGrantedPermissions`.
- JWT claims include `permissions` (bitmask dict) + roles + user info.

### Identity/Data
- Identity context: `AppDbContext` with `AppUser`, `AppRole`, `AppPermission`, `AppController`, `RolePermission`, `AppGrantedPermissions`, `RefreshToken`.
- App data context: `WorkManagementContext` (auto-generated, ~154 DbSets).

## Frontend Summary
### App Setup
- Bootstrap: `Client/src/main.ts` + `Client/src/app/app.config.ts`
- Routes: `Client/src/app/app.routes.ts` (admin login, admin module, redirects)
- Theme: PrimeNG Aura preset

### Features
- `features/admin`: admin shell, menu, topbar, dashboard, login
- `features/client`: placeholder
- `features/dashboard`: placeholder

### Core Services
- `AuthService`: login/save token, decode JWT, store permissions
- `AuthorizeService`: validate bitmask
- `UserService`: current user, user APIs
- `HtmenuService`: menu APIs
- `AiAsissTantService`: AI chat API
- Various common/config/global helpers

### Guards/Interceptors
- `AuthGuard`: admin login check
- `GuardService`: OAuth flow + permissionRequired
- `authInterceptor`: adds Bearer token

### Templates & Styles
- `admin.component.html`: topbar + sidebar + AI box
- `admin-login.component.html`: login form
- `app-menu.component.html`: currently commented out
- `app-topbar.component.html`: avatar dropdown
- Many other HTML/SCSS placeholders

## Notes
- BaseController actions have PermissionDefinition but are not synced due to `DeclaredOnly` in scan.
- FE menu render is currently commented.
- AI box is in admin sidebar.
