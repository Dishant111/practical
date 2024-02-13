using CustoomerToken.Configurations;
using CustoomerToken.Infrastructure.Repositories;
using CustoomerToken.Services;
using CustoomerToken.Services.Employees;
using CustoomerToken.Services.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddControllersWithViews(option =>
{
    option.Filters.Add<CustomResponceFilter>();
});

builder.Services.AddScoped<NotifyServices>();
builder.Services.AddScoped<TokenRepository>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.TryAddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

builder.Services.AddAuthentication("CookieSchema")
    //.AddScheme<EmployeeAuthenticationSchemaOption, EmployeeAuthenticationHandler>("CustomAuthHadler",op => { })
    .AddCookie("CookieSchema", op =>
    {
        op.ExpireTimeSpan = TimeSpan.FromMinutes(20);

        op.LoginPath = "/Employee/login";
    });

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("enployeePolicy", pb =>
    {
        pb.AuthenticationSchemes.Add("CookieSchema");
        pb.RequireRole("emp");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Token}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
