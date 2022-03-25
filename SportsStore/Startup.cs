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
            //https://docs.microsoft.com/ru-ru/ef/core/dbcontext-configuration/

            // ��������� �������� ApplicationContext � �������� ������� � ����������
            // ���������� ��������� ������ � ���� ������� �������� ����� �������� ��� �
            // ������������ ����������� ����� �������� ��������� ������������.
            //� ���� ������ ���� ������ ��������������� � ������� ������ UseSqlServer()
            //� �������� ������ �����������, ������� �������� �� �������� Configuration.
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(
             Configuration["Data:SportStoreProducts:ConnectionString"]));


            //���������� � ����������, ���������� � ����������� IProductRepository, �
            //������� � ��������� ������ ��������� ������ ���������� Product, ��� ��������
            //����� �������� ������ EFProductRepository, ��������������� �� ������ � ���������� � ���� ������
            services.AddTransient<IProductRepository, EFProductRepository>();
            /*
            ����������� � ����� ConfigureServices () �������� �������� �������������� ASP.NET Core � ���, 
            ��� ����� ���������� ����� ����������� ���������� ���������� ���������� IProductRepository, 
            ��� ������ �������� ��������� ������
            EFProductRepository. ����� AddTransient () ���������, ��� ������ ���, �����
            ��������� ���������� ���������� IProductRepository, ������ ����������� �����
            ������ EFProductRepository
            */
            services.AddMvc(option => option.EnableEndpointRouting = false);

        }

        // ����������� ���� ����� ��� ��������� ��������� HTTP-��������.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //������������ ��� ��������� �������, ������� �������� � ������������ HTTP-�������.������ �����, ���������� � ������ Configure() , ������������ ����� ����������� �����, ������� ����������� �������� ��������� HTTP-�������� 
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//���� ����������� ����� ���������� ������ ����������,������� ��������� � ����������, ��� ������� �� ����� �������� ����������
                app.UseBrowserLink(); //������������ � �������� (��� ������� �������) Microsoft.VisualStudio.Web.BrowserLink
            }
            app.UseMvcWithDefaultRoute();//�������� �������������� ASP.NET Core MVC
            app.UseStaticFiles();//���� ����������� ����� �������� ��������� ��� ������������ ������������ ����������� �� ����� wwwroot
            app.UseStatusCodePages();//���� ����������� ����� ��������� ������� ���������� HTTP-������, ������� ����� �� ����� �� ����, ����� ��� ������ 404 - Not Found

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Product}/{action=List}/{id?}");
            });
            SeedData.EnsurePopulated(app);
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
