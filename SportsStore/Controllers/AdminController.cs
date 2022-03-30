using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class AdminController:Controller
    {
        private IProductRepository repository;
        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index() => View(repository.Products);

        public ViewResult Edit(string productId) => View(repository.Products
            .FirstOrDefault(p => p.ProductID == productId));

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                #region Пояснение
                /*
                После сохранения изменений в хранилище сообщение сохраняется с использованием средства TempData, которое является частью средства состояния сеанса
                ASP.NET Core. Это словарь пар “ключ/значение”, похожий на применяемые ранее
                средства данных сеанса и ViewBag. Основное отличие объекта TempData от данных
                сеанса в том, что он хранится до тех пор, пока не будет прочитан.
                В такой ситуации использовать ViewBag невозможно, потому что объект ViewBag
                передает данные между контроллером и представлением, и он не может удерживать
                данные дольше, чем длится текущий HTTP-запрос. Когда редактирование успешно,
                браузер перенаправляется на новый URL, поэтому данные ViewBag утрачиваются.
                Мы могли бы прибегнуть к средству данных сеанса, но тогда сообщение хранилось
                бы вплоть до его явного удаления, чего делать бы не хотелось
                */
                #endregion
                TempData["message"] = $"{product.Name} has benn saved";
                return RedirectToAction("Index");
            }
            else return View(product);
        }
        public ViewResult Create() => View("Edit", new Product());

        public IActionResult Deleted(string productId)
        {
            #region Ремарка
            /*
            При попытке удалить товар, для которого ранее был создан заказ, возникнет
            ошибка. Когда объект Order сохраняется в базе данных, он превращается в запись внутри таблицы базы данных, которая содержит ссылку на связанный с ним объект Product,
            что известно как отношение внешнего ключа. Это означает, что по умолчанию база данных не позволит удалить объект Product, если для него был создан объект Order,
            поскольку такое действие внесло бы несогласованность в базу данных. Существует несколько способов решения указанной проблемы, включая удаление объектов Order при
            удалении объекта Product, к которому они относятся, либо изменение отношения между
            объектами Product и Order. Подробные сведения можно найти в документации Entity
            Framework Core.
            */
            #endregion
            Product deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"]=$"{deletedProduct.Name} was deleted";
            }
            return RedirectToAction("Index");
        }
    }

}
