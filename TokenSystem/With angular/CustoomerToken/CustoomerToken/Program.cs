using CustoomerToken.Configurations;
using CustoomerToken.Infrastructure.Repositories;
using CustoomerToken.Services;
using CustoomerToken.Services.Authentication;
using CustoomerToken.Services.Employees;
using CustoomerToken.Services.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

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
    .AddScheme<EmployeeAuthenticationSchemaOption, EmployeeAuthenticationHandler>("CustomAuthHadler", op => { })
    .AddCookie("CookieSchema", op =>
    {
        op.ForwardForbid = "CustomAuthHadler";
        op.ForwardChallenge = "CustomAuthHadler";
        op.ExpireTimeSpan = new TimeSpan(1, 0, 0); // Expires in 1 hour
        //// Only use this when the sites are on different domains
    });

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("enployeePolicy", pb =>
    {
        pb.AuthenticationSchemes.Add("CookieSchema");
        pb.RequireRole("emp");
    });
});

builder.Services.AddCors(op =>
{
    op.AddPolicy("allowall", o =>
    {
        o.WithOrigins("https://localhost:7107", "https://localhost:4200");
        o.AllowAnyHeader().AllowAnyMethod().AllowCredentials();
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
app.UseCookiePolicy(
    new CookiePolicyOptions
    {
        Secure = CookieSecurePolicy.Always
    });
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("allowall");
app.UseStaticFiles();
app.UseRouting();
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Token}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
