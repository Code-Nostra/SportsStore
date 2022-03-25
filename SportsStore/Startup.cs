using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using Microsoft.Extensions.Configuration; //Для бд
using Microsoft.EntityFrameworkCore;//Для бд
using Microsoft.Extensions.Hosting;

namespace SportsStore
{
    public class Startup
    {
        public IConfiguration Configuration;//Для БД
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;//Нужно для получения данных из appsettings.json, где храниться строка подключения
        }

        // Используйте этот метод для добавления служб в контейнер.
        public void ConfigureServices(IServiceCollection services)
        {
            //https://docs.microsoft.com/ru-ru/ef/core/dbcontext-configuration/

            // добавляем контекст ApplicationContext в качестве сервиса в приложение
            // Добавление контекста данных в виде сервиса позволит затем получать его в
            // конструкторе контроллера через механизм внедрения зависимостей.
            //В этом случае база данных конфигурируется с помощью метода UseSqlServer()
            //и указания строки подключения, которая получена из свойства Configuration.
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(
             Configuration["Data:SportStoreProducts:ConnectionString"]));


            //Компоненты в приложении, работающие с интерфейсом IProductRepository, к
            //которым в настоящий момент относится только контроллер Product, при создании
            //будут получать объект EFProductRepository, предоставляющий им доступ к информации в базе данных
            services.AddTransient<IProductRepository, EFProductRepository>();
            /*
            Добавленный в метод ConfigureServices () оператор сообщает инфраструктуре ASP.NET Core о том, 
            что когда компоненту вроде контроллера необходима реализация интерфейса IProductRepository, 
            она должна получить экземпляр класса
            EFProductRepository. Метод AddTransient () указывает, что каждый раз, когда
            требуется реализация интерфейса IProductRepository, должен создаваться новый
            объект EFProductRepository
            */
            services.AddMvc(option => option.EnableEndpointRouting = false);

        }

        // Используйте этот метод для настройки конвейера HTTP-запросов.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //используется для настройки средств, которые получают и обрабатывают HTTP-запросы.Каждый метод, вызываемый в методе Configure() , представляет собой расширяющий метод, который настраивает средство обработки HTTP-запросов 
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
            SeedData.EnsurePopulated(app);
            #region Пояснение
            /*
            Когда браузер запрашивает стандартный URL для приложения, конфигурация приложения сообщает MVC о необходимости создания контроллера Product
            для обработки запроса. Создание контроллера Product означает вызов конструктора класса ProductController, которому требуется объект, реализующий интерфейс I ProductRepositor у, и новая конфигурация указывает MVC о том, что
            для этого должен быть создан и применен объект EFProductReposi torу. Объект
            EFProductRepository обращается к функциональности Entity Framework Core,
            которая загружает данные из SQL Server и преобразует их в объекты Product. Вся
            упомянутая работа скрыта от класса Productcontroller, который просто получает
            объект, реализующий интерфейс IProductRepository. и пользуется данными, которые он предоставляет
            */
            #endregion
        }
    }
}
