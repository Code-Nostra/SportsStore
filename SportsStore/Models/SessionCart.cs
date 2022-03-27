using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SportsStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class SessionCart:Cart
    {
        #region Пояснение
        /*
        Класс SessionCart является производным от класса Cart и переопределяет методы Additem (), RemoveLine () и Clear (), так что они вызывают базовые реализации и затем сохраняют обновленное состояние в сеансе, используя расширяющие
        методы интерфейса ISession, которые были определены в главе 9. Статический метод GetCart () — это фабрика для создания объектов SessionCart и снабжения их
        объектом реализации ISession, чтобы они могли себя сохранять.
        Получение объекта реализации ISession несколько затруднено. Мы должны получить экземпляр службы IHttpContextAccessor, который предоставит доступ
        к объекту HttpContext, а тот в свою очередь — к объекту реализации ISession.
        Такой непрямой подход требуется из-за того, что сеанс не предоставляется как обычная служба.
        */
        #endregion
        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
        [JsonIgnore]
        public ISession Session { get; set; }
        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("Cart", this);
        }
        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson("Cart", this);
        }
        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}
