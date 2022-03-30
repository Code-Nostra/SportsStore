using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using Microsoft.Extensions.Configuration; //��� ��
using Microsoft.EntityFrameworkCore;//��� ��
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;//��� ��������������

namespace SportsStore
{
    public class Startup
    {
        public IConfiguration Configuration;//��� ��
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;//����� ��� ��������� ������ �� appsettings.json, ��� ��������� ������ �����������
        }

        // ����������� ���� ����� ��� ���������� ����� � ���������.
        public void ConfigureServices(IServiceCollection services)
        {
            #region ���������
            //https://docs.microsoft.com/ru-ru/ef/core/dbcontext-configuration/

            // ��������� �������� ApplicationContext � �������� ������� � ����������
            // ���������� ��������� ������ � ���� ������� �������� ����� �������� ��� �
            // ������������ ����������� ����� �������� ��������� ������������.
            //� ���� ������ ���� ������ ��������������� � ������� ������ UseSqlServer()
            //� �������� ������ �����������, ������� �������� �� �������� Configuration.
            #endregion
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(
             Configuration["Data:SportStoreProducts:ConnectionString"]));

            //��� ��������������
            services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(
                Configuration["Data:SportStoreIdentity:ConnectionString"]));
            services.AddIdentity<IdentityUser, IdentityRole>()
                //��������� Entity Framework ���������� �������� �������� �� ��������������
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            #region ���������
            //���������� � ����������, ���������� � ����������� IProductRepository, �
            //������� � ��������� ������ ��������� ������ ���������� Product, ��� ��������
            //����� �������� ������ EFProductRepository, ��������������� �� ������ � ���������� � ���� ������
            #endregion
            services.AddTransient<IProductRepository, EFProductRepository>();
            /*
            ����������� � ����� ConfigureServices () �������� �������� �������������� ASP.NET Core � ���, 
            ��� ����� ���������� ����� ����������� ���������� ���������� ���������� IProductRepository, 
            ��� ������ �������� ��������� ������
            EFProductRepository. ����� AddTransient () ���������, ��� ������ ���, �����
            ��������� ���������� ���������� IProductRepository, ������ ����������� �����
            ������ EFProductRepository
            */



            services.AddTransient<IOrderRepository, EFOrderRepository>();

            #region ���������
            /*
            ����� AddScoped () ���������, ��� ��� �������������� ��������� �������� � ����������� Cart ������ ����������� ���� � ��� �� ������. 
            ������ ���������� �������� ����� ���� ���������������, �� �� ��������� ��� ������, ��� � ����� ��
            ����� ������ ���������� Cart �� ������� �����������, ������� ������������ ���
            �� ����� HTTP-������, ����� ���������� ���� � ��� �� ������.
            ������ �������������� ������ AddScoped () ����������� ����� ������, ���
            �������� ��� ���������, ����������� ������-���������, ������� ����� ����������� ��� �������������� �������� � Cart. ������-��������� �������� ���������
            �����, ������� ���� ����������������, � �������� �� ������ GetCart () ������
            SessionCart. � ���������� ������� ��� ������ Cart ����� �������������� �����
            �������� �������� SessionCart, ������� ������������� ���� ���� ��� ������ ������, ����� ��� ��������������.
            */
            #endregion
            //������������ � CartSummaryViewComponent 
            //����� ������������� ������� ��� �������� Cart ������� �������� SessionCart
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            #region ���������
            /*
            �� ����� �������� ������ � �������������� ������ Addsingleton (), �������
            ���������, ��� ������ ������ ����������� ���� � ��� �� ������. ��������� ������
            �������� �������������� MVC � ���, ��� ����� ��������� ���������� ����������
            IHttpContextAccessor, ���������� ������������ ����� HttpContextAccessor.
            ������ ������ �����������, ������� � ������ SessionCart ����� �������� ������
            � �������� ������
            */
            #endregion
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            //��������. ��� ������ ����� ������                
            services.AddMvc(/*options=>options.EnableEndpointRouting=false*/);
            //services.AddRazorPages();

            services.AddMemoryCache();
            services.AddSession();
            //Set Session Timeout. Default is 30 minutes.
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

        }

        // ����������� ���� ����� ��� ��������� ��������� HTTP-��������.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //������������ ��� ��������� �������, ������� �������� � ������������ HTTP-�������.������ �����, ���������� � ������ Configure() , ������������ ����� ����������� �����, ������� ����������� �������� ��������� HTTP-�������� 
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//���� ����������� ����� ���������� ������ ����������,������� ��������� � ����������, ��� ������� �� ����� �������� ����������
                app.UseBrowserLink(); //������������ � �������� (��� ������� �������) Microsoft.VisualStudio.Web.BrowserLink
            }
            
            //��������. ��� ������ ����� ������      
            //app.UseMvcWithDefaultRoute();//�������� �������������� ASP.NET Core MVC
            app.UseStaticFiles();//���� ����������� ����� �������� ��������� ��� ������������ ������������ ����������� �� ����� wwwroot
            app.UseStatusCodePages();//���� ����������� ����� ��������� ������� ���������� HTTP-������, ������� ����� �� ����� �� ����, ����� ��� ������ 404 - Not Found
            app.UseSession();
            app.UseAuthentication();//��� Identity
            app.UseRouting();//��� UseEndpoints
            app.UseEndpoints(endpoints =>
            {
                //������� ��������� �������� ������� �������� ���������
                endpoints.MapControllerRoute(
                name: "1",
                pattern: "{category}/Page{productPage:int}",
                defaults: new { controller = "Product", action = "List", productPage = 1 });

                //������� ��������� �������� ��������� ������ ���� ���������
                endpoints.MapControllerRoute(
                name: "2",
                pattern: "Page{productPage:int}",
                defaults: new { controller = "Product", action = "List", productPage=1});
                
                //������� ������ �������� ������� ��������� ���������
                endpoints.MapControllerRoute(
                name: "3",
                pattern: "{category}",
                defaults: new { controller = "Product", action = "List", productPage = 1 });

                //������� ������ �������� ������ ������� ���� ���������
                endpoints.MapControllerRoute(
                name: "4",
                pattern: "",
                defaults:new { controller = "Product", action = "List", productPage = 1 });

                endpoints.MapControllerRoute(name: "5", pattern: "{controller}/{action}/{id?}");

                endpoints.MapControllerRoute(
                name: "6",
                pattern: "{Prodict}/{List}",
                defaults: new { controller = "Product", action = "List", productPage = 1 });
            });
            
            SeedData.EnsurePopulated(app);//��������� ��������� �������(��������)
            IdentitySeedData.EnsurePopulated(app);//��������� ��������� �������(�������� ������)
            #region ���������
            /*
            ����� ������� ����������� ����������� URL ��� ����������, ������������ ���������� �������� MVC � ������������� �������� ����������� Product
            ��� ��������� �������. �������� ����������� Product �������� ����� ������������ ������ ProductController, �������� ��������� ������, ����������� ��������� I ProductRepositor �, � ����� ������������ ��������� MVC � ���, ���
            ��� ����� ������ ���� ������ � �������� ������ EFProductReposi tor�. ������
            EFProductRepository ���������� � ���������������� Entity Framework Core,
            ������� ��������� ������ �� SQL Server � ����������� �� � ������� Product. ���
            ���������� ������ ������ �� ������ Productcontroller, ������� ������ ��������
            ������, ����������� ��������� IProductRepository. � ���������� �������, ������� �� �������������
            */
            #endregion
        }
    }
}
