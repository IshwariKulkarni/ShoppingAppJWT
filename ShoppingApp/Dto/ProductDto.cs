using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Dto
{
    public class ProductDto
    {
        [Required(ErrorMessage = "Required")]
        public string ProductName { get; set; }
        //[Required(ErrorMessage = "Required")]
        //public string ProductCategory { get; set; }
        [Required(ErrorMessage = "Required")]
        public float ProductPrice { get; set; }
        [Required(ErrorMessage = "Required")]
        public int ProductQuantity { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryId { get; set; }
    }
}
