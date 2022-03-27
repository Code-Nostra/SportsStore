using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent:ViewComponent
    {
        #region Пояснение
        /*
        Конструктор, определенный в листинге , принимает аргумент типа
        IProductRepository. Когда инфраструктуре MVC необходимо создать экземпляр
        класса компонента представления, она отметит потребность в предоставлении этого аргумента и просмотрит конфигурацию в классе Startup, чтобы выяснить, какой
        объект реализации должен использоваться. Мы имеем дело с тем же самым средством
        внедрения зависимостей, которое применялось в контроллере , и результат
        будет аналогичным — обеспечение компоненту представления доступа к данным без
        необходимости знать используемую реализацию хранилища, как описано в главе
        */
        #endregion
        private IProductRepository repository;
        public NavigationMenuViewComponent(IProductRepository repo)
        {
            repository = repo;
        }
        //Используется в _Layout.cshtml который автоматом вызывается(указывается) как помпоновщик по умолчанию _ViewStart.cshtml
        public IViewComponentResult Invoke()
        {
            #region Как надо делать
            /*
            Значение категории можно было бы передать представлению путем создания еще одного класса
            модели представления (и так бы делалось в реальном проекте), но ради разнообразия
            применим объект ViewBag
            */
            #endregion
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x));
        }
    }
}
