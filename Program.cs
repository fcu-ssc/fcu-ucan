using System.Text;
using fcu_ucan.Data;
using fcu_ucan.Entities;
using fcu_ucan.Services;
using fcu_ucan.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo
    .File("Logs/fcu-ucan.txt", shared: true, rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 512 * 1024 * 1024, rollOnFileSizeLimit: true)
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);
    
    builder.Services.AddSerilog();

    builder.Services.AddControllersWithViews();

    builder.Services.AddResponseCompression(options =>
    {
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
        options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
        {
            "image/svg+xml",
            "image/webp",
            "application/manifest+json"
        });
    });

    builder.Services.AddHttpClient("NID", c => { c.BaseAddress = new Uri(builder.Configuration["NID:Url"]!); });

    builder.Services.AddHttpClient("UCAN", c => { c.BaseAddress = new Uri(builder.Configuration["Domain"]!); })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
        });

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;
        options.SignIn.RequireConfirmedEmail = true;
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.User.AllowedUserNameCharacters = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        options.User.RequireUniqueEmail = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
        options.Lockout.MaxFailedAccessAttempts = 10;
        options.Lockout.AllowedForNewUsers = true;
    });

    builder.Services.AddSession();

    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"]!,
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"]!,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
            };
        });

    builder.Services.AddScoped<IOAuthService, OAuthService>();

    builder.Services.AddSingleton<IMailService, MailService>();

    var app = builder.Build();

    app.UseResponseCompression();

    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseStatusCodePagesWithRedirects("/error?code={0}");
    }
    else
    {
        app.UseExceptionHandler("/ucan/error");
        app.UseHsts();
        app.UseStatusCodePagesWithRedirects("/ucan/error?code={0}");
    }

    app.UseSession();

    app.Use(async (context, next) =>
    {
        var token = context.Session.GetString("token");
        if (!string.IsNullOrEmpty(token))
        {
            context.Request.Headers.Append("Authorization", $"Bearer {token}");
        }

        await next();
    });

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = ctx =>
        {
            // 靜態檔案快取一年
            ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=31536000");
        }
    });

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}