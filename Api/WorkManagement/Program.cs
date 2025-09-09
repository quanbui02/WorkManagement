using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Work.DataContext;
using Work.DataContext.Models;
using WorkManagement.Model;
using WorkManagement;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WorkManagement.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "WorkManagement", Version = "v1" });

    // Thêm hỗ trợ JWT
    c.AddSecurityDefinition("Bearer", new()
    {
        Description = @"JWT Authorization header.  
                        Nhập vào: Bearer {token}",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new() { Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache(); // cache trên local
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


// Cho tạo mật khẩu tối thiểu 6 số ko yêu cầu các ký tự đặc biệt hoặc viết hoa
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
});

// 🔑 1. Add JWT Bearer
var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>();

var privateKeyPath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["Jwt:PrivateKeyPath"]);
var rsaPrivateKey = JwtKeyHelper.LoadPrivateKey(privateKeyPath);

var publicKeyPath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["Jwt:PublicKeyPath"]);
var rsaSecurityKey = JwtKeyHelper.LoadPublicKey(publicKeyPath);

builder.Services.AddSingleton<SecurityKey>(rsaPrivateKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
        IssuerSigningKey = rsaSecurityKey,
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var identity = context.Principal.Identity as ClaimsIdentity;
            var roleAssignClaim = identity.FindFirst("roleassign")?.Value; // config authorize theo roleassign

            if (!string.IsNullOrEmpty(roleAssignClaim))
            {
                var roles = System.Text.Json.JsonSerializer.Deserialize<List<string>>(roleAssignClaim);
                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                }
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Work.DataContext") // migration sang class Library Work.DataContext
    ));

builder.Services.AddDbContext<WorkManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddCustomService(builder.Configuration);
var app = builder.Build();

// khởi tạo seed data cho user là admin => nếu cần
//await SeedHelper.SeedAdminUserAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
