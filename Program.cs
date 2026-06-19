using Lab06.Models;
using Lab06.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình EF Core kết nối cơ sở dữ liệu SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Đăng ký các dịch vụ Controller vào Container ứng dụng
builder.Services.AddControllers();

// 3. Đăng ký Dependency Injection cho Repository Pattern
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// 4. Cấu hình tài liệu trực quan hóa Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 5. Cấu hình chính sách CORS để Front-end (VSCode Live Server) có thể gọi API
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowOrigins", policy =>
    {
        // Cho phép các cổng localhost chạy từ Front-end
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 6. Cấu hình HTTP request pipeline (Middleware)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 7. Kích hoạt CORS (Lưu ý: Bắt buộc đặt TRƯỚC UseAuthorization)
app.UseCors("MyAllowOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();