using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string CategoryName { get; set; }
    }
}
