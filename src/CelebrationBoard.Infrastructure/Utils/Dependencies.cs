namespace CelebrationBoard.Infrastructure.Utils;

using System.Text;
using CelebrationBoard.Domain.Authentication;
using CelebrationBoard.Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

public static class Dependencies
{
  public static void Init(IServiceProvider serviceProvider, IWebHostEnvironment environment)
  {
    if (environment.IsDevelopment())
    {
      var userDbContext = serviceProvider.GetRequiredService<UserDbContext>();
      userDbContext.Database.EnsureDeleted();
      userDbContext.Database.EnsureCreated();
    }
    else
    {
      throw new NotImplementedException("Handle migrations in production");
    }
  }

  public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
  {
    services.AddDbContext<UserDbContext>(options =>
    {
      options.UseNpgsql(configuration["CelebrationBoardUsersConnectionString"])
      .LogTo(Console.WriteLine, LogLevel.Information);
    });

    services.AddScoped<IUserManager, UserManagerService>();

    services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<UserDbContext>()
          .AddDefaultTokenProviders();

    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = "Bearer";
      options.DefaultChallengeScheme = "Bearer";
      options.DefaultScheme = "Bearer";
    })
    .AddJwtBearer(options =>
    {
      options.SaveToken = true;
      options.RequireHttpsMetadata = false;
      options.TokenValidationParameters = new TokenValidationParameters()
      {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
      };
    });

    services.Configure<IdentityOptions>(options =>
    {
      options.Password.RequireDigit = false;
      options.Password.RequireLowercase = false;
      options.Password.RequireNonAlphanumeric = false;
      options.Password.RequireUppercase = false;
      options.Password.RequiredLength = 12;
      options.Password.RequiredUniqueChars = 0;

      options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
      options.Lockout.MaxFailedAccessAttempts = 5;
      options.Lockout.AllowedForNewUsers = true;

      options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
      options.User.RequireUniqueEmail = true;
    });

    services.ConfigureApplicationCookie(options =>
    {
      options.Cookie.HttpOnly = true;
      options.ExpireTimeSpan = TimeSpan.FromMinutes(1000);

      // options.LoginPath = "/Identity/Account/Login";
      // options.AccessDeniedPath = "/Identity/Account/AccessDenied";
      options.SlidingExpiration = true;
    });
  }
}
