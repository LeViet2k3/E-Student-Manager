using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StudentApp.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// 🛠️ Thêm Authentication bằng Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // ✅ Trang đăng nhập
        options.LogoutPath = "/Login/Logout"; // ✅ Trang đăng xuất
        options.AccessDeniedPath = "/AccessDenied"; // ✅ Nếu cần trang lỗi truy cập
    });

builder.Services.AddAuthorization();

// 🛠️ Đăng ký dịch vụ Database & Service
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// 🔥 Thêm Session vào dịch vụ
builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ RAM để lưu session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session hết hạn sau 30 phút
    options.Cookie.HttpOnly = true; // Chỉ truy cập qua HTTP
    options.Cookie.IsEssential = true; // Bắt buộc lưu session
});

var app = builder.Build();

// 🛠️ Cấu hình Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔥 Bật Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 🔥 Quan trọng: Bật Session trước `app.UseEndpoints();`
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
