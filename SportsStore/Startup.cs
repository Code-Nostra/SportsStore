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
            Configuration = configuration;
        }

        // ����������� ���� ����� ��� ���������� ����� � ���������.
        public void ConfigureServices(IServiceCollection services)
        {
            //https://docs.microsoft.com/ru-ru/ef/core/dbcontext-configuration/
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(
             Configuration["Data:SportStoreProducts:ConnectionString"]));

            services.AddTransient<IProductRepository, EFProductRepository>();

            services.AddMvc(option => option.EnableEndpointRouting = false);
                        /*
            ����������� � ����� ConfigureServices () �������� �������� �������������� ASP.NET Core � ���, ��� ����� ���������� ����� ����������� ���������� ���������� ���������� IProductRepository, ��� ������ �������� ��������� ������
            FakeProductRepository. ����� AddTransient () ���������, ��� ������ ���, �����
            ��������� ���������� ���������� IProductRepository, ������ ����������� �����
            ������ FakeProductRepository
            */
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
        }
    }
}
