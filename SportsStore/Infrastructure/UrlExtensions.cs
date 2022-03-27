using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
    public static class UrlExtensions
    {
        /*
        Расширяющий метод PathAndQuery () работает с классом HttpRequest, используемый в ASP.NET Core для описания HTTP-запроса. Расширяющий метод генерирует
        URL, по которому браузер будет возвращаться после обновления корзины, принимая
        во внимание строку запроса, если она есть.
        */
        public static string PathAndQuery(this HttpRequest request) =>
            request.QueryString.HasValue ? $"{request.Path}{request.QueryString}"
            : request.Path.ToString();
    }
}
