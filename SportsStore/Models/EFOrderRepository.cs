using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;

        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        #region Пояснение
        /*
        Include () и Thenlnclude () указано, что когда объект Order читается из базы
        данных, то также должна загружаться коллекция, ассоциированная со свойством Lines,
        наряду с объектами Product, которые связаны с элементами коллекции:
        */
        #endregion
        public IQueryable<Order> Orders => context.Orders
            .Include(o => o.Lines)//запрос для одного типа сущности также загружает связанные сущности в составе запроса
                                  //на подобии соединения Join
            .ThenInclude(l => l.Product);//Загрузка связанных сущностей сущьности которая указана выше(Lines), то есть Join Lines

        public void SaveOrder(Order order)
        {
            #region Поясение
            /*
            Дополнительный шаг требуется и при сохранении объекта Order в базе данных. Когда данные корзины пользователя десериализируются из состояния сеанса, пакет JSON создает
            новые объекты, не известные инфраструктуре Entity Framework Core, которая затем пытается записать все объекты в базу данных. В случае объектов Product это означает, что
            инфраструктура Entity Framework Core попытается записать объекты, которые уже были сохранены, что приведет к ошибке. Во избежание проблемы мы уведомляем Entity Framework
            Core о том, что объекты существуют и не должны сохраняться в базе данных до тех пор,
            пока они не будут модифицированы:
            В результате инфраструктура Entity Framework Core не будет пытаться записывать десериализированные объекты Product, которые ассоциированы с объектом Order
            */
            #endregion
            context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}
