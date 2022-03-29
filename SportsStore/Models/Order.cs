using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class Order
    {
        
        [BindNever]//предотвращает предоставление пользователем значений для снабженных этим атрибутом свойств в HTTP-запросе
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int OrderID { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }
        [Required(ErrorMessage = "Please enter a name")]

        //Стаут отправки
        [BindNever]
        public bool Shipped { get; set; }
        // Введите имя
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the first address line")]
        // Введите первую строку адреса
        public string Linel { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        [Required(ErrorMessage = "Please enter a city name")]
        // Введите название города
        public string City { get; set; }
        [Required(ErrorMessage = "Please enter a state name")]
        // Введите название штата
        public string State { get; set; }
        public string Zip { get; set; }
        [Required(ErrorMessage = "Please enter a country name")]
        // Введите название страны
        public string Country { get; set; }
        public bool GiftWrap { get; set; }

    }
}
