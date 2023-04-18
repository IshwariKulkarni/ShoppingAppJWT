using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string ProductName { get; set; }

        //[Required(ErrorMessage = "Required")]
        //public string ProductCategory { get; set; }

        [Required(ErrorMessage = "Required")]
        public float ProductPrice { get; set; }

        [Required(ErrorMessage = "Required")]
        public int ProductQuantity { get; set; }

        public string ProductDescription { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
