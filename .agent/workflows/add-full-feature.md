---
description: Thêm feature fullstack (BE + FE) hoàn chỉnh
---

# Thêm Feature Fullstack

## Quy trình thêm feature hoàn chỉnh cả Backend + Frontend

### Bước 1: Backend — Xem workflow `/add-be-feature`
Tạo Model → Service → Controller → DI → Sync Permission

### Bước 2: Frontend — Xem workflow `/add-fe-feature`
Tạo Service → Component → Route → SCSS → Menu

### Bước 3: Kiểm tra
// turbo
1. Build Backend: `dotnet build` tại `Api/WorkManagement/`
// turbo
2. Build Frontend: `ng build` tại `Client/`
3. Test API bằng Swagger hoặc HTTP file
