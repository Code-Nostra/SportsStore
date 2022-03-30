using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [UIHint("password")]
        //применения атрибута asp-for внутри элемента input представления Razor, предназначенного для входа, вспомогательная функция дескриптора установит атрибут
        //type в password; таким образом, вводимый пользователем текст не будет виден на экране
        public string Password { get; set; }//в случае
        public string ReturnUrl { get; set; } = "/";
    }
}
