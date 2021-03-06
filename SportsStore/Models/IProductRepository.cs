using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }//Объект IQueryable предоставляет удаленный доступ к базе данных и позволяет
                                             //перемещаться по данным как в прямом порядке от начала до конца, так и в обратном порядке
                                             //https://metanit.com/sharp/entityframework/1.4.php

        /*
        Применение интерфейса
        IQueryable<T> дает возможность запрашивать у базы данных именно те объекты, которые требуются, с помощью стандартных операторов LINQ, не нуждаясь в информации о том,
        какой сервер баз данных хранит данные или как он обрабатывает запрос. Без интерфейса
        IQueryable<T> пришлось бы извлекать из базы данных все объекты Product и затем
        отбрасывать ненужные, что с ростом объема данных, используемых приложением, превращается в затратную операцию. Именно по этой причине в интерфейсах и классах хранилища
        в форме базы данных обычно вместо IEnumerable<T> применяется IQueryable<T>.
        Однако во время использования интерфейса lQueryable<T> следует соблюдать осторожность, поскольку каждый раз, когда происходит перечисление коллекции объектов, запрос
        будет оцениваться заново, т.е. базе данных отправится новый запрос. В результате выгоды в плане эффективности от применения lQueryable<T> могут быть сведены на нет.
        В таких ситуациях lQueryable<T> можно преобразовать в более предсказуемую форму,
        используя расширяющий метод ToList () или ТоАггау ().
        */

        void SaveProduct(Product product);

        Product DeleteProduct(string productId);
    }

}
