using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Resource
{
    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
