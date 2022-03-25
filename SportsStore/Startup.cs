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
        // Используйте этот метод для добавления служб в контейнер.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddTransient<IProductRepository, FakeProductRepository>();
                        /*
            Добавленный в метод ConfigureServices () оператор сообщает инфраструктуре ASP.NET Core о том, что когда компоненту вроде контроллера необходима реализация интерфейса IProductRepository, она должна получить экземпляр класса
            FakeProductRepository. Метод AddTransient () указывает, что каждый раз, когда
            требуется реализация интерфейса IProductRepository, должен создаваться новый
            объект FakeProductRepository
            */
        }

        // Используйте этот метод для настройки конвейера HTTP-запросов.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //используется для настройки средств, которые получают и
                                                                                //обрабатывают HTTP-запросы.Каждый метод, вызываемый в методе Configure() ,
                                                                                //представляет собой расширяющий метод, который настраивает средство обработки
                                                                                //HTTP-запросов 
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//Этот расширяющий метод отображает детали исключения,которое произошло в приложении, что полезно во время процесса разработки
                app.UseBrowserLink(); //Привязывания к браузеру (для удобной отладки) Microsoft.VisualStudio.Web.BrowserLink
            }
            app.UseMvcWithDefaultRoute();//включает инфраструктуру ASP.NET Core MVC
            app.UseStaticFiles();//Этот расширяющий метод включает поддержку для обслуживания статического содержимого из папки wwwroot
            app.UseStatusCodePages();//Этот расширяющий метод добавляет простое сообщениев HTTP-ответы, которые иначе не имели бы тела, такие как ответы 404 - Not Found

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Product}/{action=List}/{id?}");
            });
        }
    }
}
