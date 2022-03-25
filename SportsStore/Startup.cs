using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore
{
    public class Startup
    {
        // ����������� ���� ����� ��� ���������� ����� � ���������.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddTransient<IProductRepository, FakeProductRepository>();
                        /*
            ����������� � ����� ConfigureServices () �������� �������� �������������� ASP.NET Core � ���, ��� ����� ���������� ����� ����������� ���������� ���������� ���������� IProductRepository, ��� ������ �������� ��������� ������
            FakeProductRepository. ����� AddTransient () ���������, ��� ������ ���, �����
            ��������� ���������� ���������� IProductRepository, ������ ����������� �����
            ������ FakeProductRepository
            */
        }

        // ����������� ���� ����� ��� ��������� ��������� HTTP-��������.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //������������ ��� ��������� �������, ������� �������� �
                                                                                //������������ HTTP-�������.������ �����, ���������� � ������ Configure() ,
                                                                                //������������ ����� ����������� �����, ������� ����������� �������� ���������
                                                                                //HTTP-�������� 
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
        }
    }
}
