using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiMatos.Context;
using MiMatos.Repositories;
using MiMatos.Repositories.Interfaces;
using MiMatos.Services;
using ReflectionIT.Mvc.Paging;

namespace MiMatos;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IProprietarioRepository, ProprietarioRepository>();
        services.AddTransient<IPropriedadeRepository, PropriedadeRepository>();
        services.AddTransient<ITipoRepository, TipoRepository>();
        services.AddTransient<ILocationRepository, LocationRepository>();
        services.AddScoped<ISeedUserRoles, SeedUserRoles>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin",
                p => p.RequireRole("Admin"));
        });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddControllersWithViews();

        services.AddPaging(options =>
        {
            options.ViewName = "Bootstrap4";
            options.PageParameterName = "pageindex";
        });

        services.AddMemoryCache();
        services.AddSession();
    }

    public void Configure (IApplicationBuilder app, IWebHostEnvironment environment, ISeedUserRoles seedUserRoles)
    {
        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        seedUserRoles.SeedRoles();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
