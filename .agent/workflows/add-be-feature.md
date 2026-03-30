---
description: Thêm feature mới cho Backend (.NET API)
---

# Thêm Feature Backend

## Quy trình thêm 1 feature mới cho Backend (Controller + Service + DI)

### 1. Tạo Model (nếu cần request/response riêng)
- Tạo file trong `Api/WorkManagement/Models/Admin/` (admin) hoặc `Api/WorkManagement/Models/` (client)
- Đặt namespace: `WorkManagement.Models`

### 2. Tạo Service
- **File**: `Api/WorkManagement/Services/Admins/{Feature}Services.cs` (admin) hoặc `Api/WorkManagement/Services/Clients/{Feature}Service.cs` (client)
- **Pattern bắt buộc**:
  - Khai báo `interface I{Feature}Service` + `class {Feature}Service` trong cùng 1 file
  - Inject `AppDbContext _db` (cho Identity/Permission) và/hoặc `WorkManagementContext _context` (cho business data)
  - Return `Result<object>.Success(data, totalRecord, message)` hoặc `Result<object>.Error(message)`
  - Nếu cần CRUD generic → kế thừa `BaseService<TModel, TDbContext>`

```csharp
// Mẫu Service
public interface I{Feature}Service
{
    Task<object> GetList(string key, int offset, int limit);
}
public class {Feature}Service : I{Feature}Service
{
    private readonly WorkManagementContext _context; // hoặc AppDbContext _db
    private readonly IUserInfo _userInfo;
    public {Feature}Service(WorkManagementContext context, IUserInfo userInfo)
    {
        _context = context;
        _userInfo = userInfo;
    }
    public async Task<object> GetList(string key, int offset, int limit)
    {
        var query = _context.TableName.AsQueryable();
        if (!string.IsNullOrEmpty(key))
            query = query.Where(x => x.Name.ToLower().Contains(key.ToLower()));
        var data = await query.Skip(offset).Take(limit).ToListAsync();
        return Result<object>.Success(data, await query.CountAsync());
    }
}
```

### 3. Tạo Controller
- **File**: `Api/WorkManagement/Controllers/Admins/{Feature}Controller.cs` (admin) hoặc `Api/WorkManagement/Controllers/Clients/{Feature}Controller.cs` (client)
- **Admin controller**:
  - Dùng `[Authorize(Roles = "Admin")]` cho toàn bộ controller
  - KHÔNG cần `[PermissionDefinition]` trên từng action vì admin-only
- **Client/Business controller**:
  - Kế thừa `BaseController<TModel, TInterface>` nếu cần CRUD chuẩn
  - Gắn `[PermissionDefinition("Tên quyền", index)]` trên từng action cần phân quyền bitmask
  - `[PermissionDefinition("Tên controller", GroupName = "Nhóm")]` trên class

```csharp
// Mẫu Admin Controller
[Produces("application/json")]
[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class {Feature}Controller : ControllerBase
{
    private readonly I{Feature}Service _{feature}Service;
    private readonly IUserInfo _userInfo;
    public {Feature}Controller(I{Feature}Service service, IUserInfo userInfo)
    {
        _{feature}Service = service;
        _userInfo = userInfo;
    }

    [HttpGet]
    public async Task<IActionResult> GetList(string? key, int offset = 0, int limit = 20)
    {
        var data = await _{feature}Service.GetList(key, offset, limit);
        return Ok(data);
    }
}
```

### 4. Đăng ký DI
- Mở `Api/WorkManagement/ConfigService.cs`
- Thêm dòng: `services.AddScoped<I{Feature}Service, {Feature}Service>();` trong region `Dependency Injection`

### 5. Sync Permission (nếu dùng PermissionDefinition)
- Gọi `POST /SyncPermission/Sync` để đồng bộ permission mới vào DB

### Checklist
- [ ] Model request/response (nếu cần)
- [ ] Interface + Service class
- [ ] Controller với attribute phù hợp
- [ ] Đăng ký DI trong ConfigService.cs
- [ ] Sync permission (nếu có PermissionDefinition)
