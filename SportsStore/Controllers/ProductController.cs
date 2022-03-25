﻿using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class ProductController:Controller
    {
        private IProductRepository repository;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }
        #region Пояснение
        /*
        Когда инфраструктуре MVC необходимо создать новый экземпляр класса Productcontroller для обработки HTTP-запроса, MVC проинспектирует
        конструктор и выяснит, что он требует объекта, который реализует интерфейс
        IProductRepository. Чтобы определить, какой класс реализации должен использоваться, инфраструктура MVC обращается к конфигурации в классе Startup, которая
        сообщает о том, что нужно применять класс FakeRepository и каждый раз создавать
        его новый экземпляр. Инфраструктура MVC создает новый объект FakeRepository и
        использует его для вызова конструктора Productcontroller с целью создания объекта контроллера, который будет обрабатывать НТТР-запрос.
        Такой подход известен под названием внедрение зависимостей и позволяет объекту Productcontroller получать доступ к хранилищу приложения через интерфейс
        IProductRepository без необходимости в знании того, какой класс реализации был
        сконфигурирован. Позже мы заменим фиктивное хранилище реальным, а благодаря
        внедрению зависимостей контроллер продолжит работать безо всяких изменений.
        */
        #endregion 

        public ViewResult List() => View(repository.Products);
    }
}
