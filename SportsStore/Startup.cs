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
using Microsoft.AspNetCore.Identity;//Для аутентификации

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
            #region Пояснение
            //https://docs.microsoft.com/ru-ru/ef/core/dbcontext-configuration/

            // добавляем контекст ApplicationContext в качестве сервиса в приложение
            // Добавление контекста данных в виде сервиса позволит затем получать его в
            // конструкторе контроллера через механизм внедрения зависимостей.
            //В этом случае база данных конфигурируется с помощью метода UseSqlServer()
            //и указания строки подключения, которая получена из свойства Configuration.
            #endregion
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(
             Configuration["Data:SportStoreProducts:ConnectionString"]));

            //Для аутентификации
            services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(
                Configuration["Data:SportStoreIdentity:ConnectionString"]));
            services.AddIdentity<IdentityUser, IdentityRole>()
                //Добавляет Entity Framework реализацию хранилищ сведений об удостоверениях
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            #region Пояснение
            //Компоненты в приложении, работающие с интерфейсом IProductRepository, к
            //которым в настоящий момент относится только контроллер Product, при создании
            //будут получать объект EFProductRepository, предоставляющий им доступ к информации в базе данных
            #endregion
            services.AddTransient<IProductRepository, EFProductRepository>();
            /*
            Добавленный в метод ConfigureServices () оператор сообщает инфраструктуре ASP.NET Core о том, 
            что когда компоненту вроде контроллера необходима реализация интерфейса IProductRepository, 
            она должна получить экземпляр класса
            EFProductRepository. Метод AddTransient () указывает, что каждый раз, когда
            требуется реализация интерфейса IProductRepository, должен создаваться новый
            объект EFProductRepository
            */



            services.AddTransient<IOrderRepository, EFOrderRepository>();

            #region Пояснение
            /*
            Метод AddScoped () указывает, что для удовлетворения связанных запросов к экземплярам Cart должен применяться один и тот же объект. 
            Способ связывания запросов может быть сконфигурирован, но по умолчанию это значит, что в ответ на
            любой запрос экземпляра Cart со стороны компонентов, которые обрабатывают тот
            же самый HTTP-запрос, будет выдаваться один и тот же объект.
            Вместо предоставления методу AddScoped () отображения между типами, как
            делалось для хранилища, указывается лямбда-выражение, которое будет выполняться для удовлетворения запросов к Cart. Лямбда-выражение получает коллекцию
            служб, которые были зарегистрированы, и передает ее методу GetCart () класса
            SessionCart. В результате запросы для службы Cart будут обрабатываться путем
            создания объектов SessionCart, которые сериализируют сами себя как данные сеанса, когда они модифицируются.
            */
            #endregion
            //Используется в CartSummaryViewComponent 
            //Чтобы удовлетворять запросы для объектов Cart выдачей объектов SessionCart
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            #region Пояснение
            /*
            Мы также добавили службу с использованием метода Addsingleton (), который
            указывает, что всегда должен применяться один и тот же объект. Созданная служба
            сообщает инфраструктуре MVC о том, что когда требуются реализации интерфейса
            IHttpContextAccessor, необходимо использовать класс HttpContextAccessor.
            Данная служба обязательна, поэтому в классе SessionCart можно получать доступ
            к текущему сеансу
            */
            #endregion
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            //Устарело. Для сессий нужно убрать                
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

        // Используйте этот метод для настройки конвейера HTTP-запросов.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //используется для настройки средств, которые получают и обрабатывают HTTP-запросы.Каждый метод, вызываемый в методе Configure() , представляет собой расширяющий метод, который настраивает средство обработки HTTP-запросов 
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//Этот расширяющий метод отображает детали исключения,которое произошло в приложении, что полезно во время процесса разработки
                app.UseBrowserLink(); //Привязывания к браузеру (для удобной отладки) Microsoft.VisualStudio.Web.BrowserLink
            }
            
            //Устарело. Для сессий нужно убрать      
            //app.UseMvcWithDefaultRoute();//включает инфраструктуру ASP.NET Core MVC
            app.UseStaticFiles();//Этот расширяющий метод включает поддержку для обслуживания статического содержимого из папки wwwroot
            app.UseStatusCodePages();//Этот расширяющий метод добавляет простое сообщениев HTTP-ответы, которые иначе не имели бы тела, такие как ответы 404 - Not Found
            app.UseSession();
            app.UseAuthentication();//Для Identity
            app.UseRouting();//Для UseEndpoints
            app.UseEndpoints(endpoints =>
            {
                //Выводит указанную страницу товаров заданной категории
                endpoints.MapControllerRoute(
                name: "1",
                pattern: "{category}/Page{productPage:int}",
                defaults: new { controller = "Product", action = "List", productPage = 1 });

                //Выводит указанную страницу отображая товары всех категорий
                endpoints.MapControllerRoute(
                name: "2",
                pattern: "Page{productPage:int}",
                defaults: new { controller = "Product", action = "List", productPage=1});
                
                //Выводит первую страницу товаров указанной категории
                endpoints.MapControllerRoute(
                name: "3",
                pattern: "{category}",
                defaults: new { controller = "Product", action = "List", productPage = 1 });

                //Выводит первую страницу списка товаров всех категорий
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
            
            SeedData.EnsurePopulated(app);//Начальное заполение данными(товарами)
            IdentitySeedData.EnsurePopulated(app);//Начальное заполение данными(добавили админа)
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
