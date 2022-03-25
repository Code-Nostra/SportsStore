using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models.ViewModels
{
    public class PagingInfo//Модель представления
    {
        //Всего элемнтов
        public int TotalItem { get; set; }
        //Элементов на странице
        public int ItemsPerPage { get; set; }
        //Текущая страница
        public int CurrentPage { get; set; }
        //Всего страниц
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItem / ItemsPerPage);
    }
}
