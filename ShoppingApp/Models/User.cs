using ShoppingApp.Resource;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [ForeignKey("UserRole")]
        public int RoleId { get; set; }
        public UserRole UserRole { get; set; }
        //public ICollection<Booking> Bookings { get; set; }
    }
}
