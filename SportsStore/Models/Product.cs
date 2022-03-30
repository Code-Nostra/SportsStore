using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ProductID { get; set; }
        [Required(ErrorMessage ="Pleas enter a product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Pleas enter a description name")]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue,ErrorMessage ="Pleas enter a positiv price")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please specify a category")]
        public string Category { get; set; }
    }
}
