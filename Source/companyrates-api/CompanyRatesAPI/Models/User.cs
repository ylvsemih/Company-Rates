using System.ComponentModel.DataAnnotations;

namespace CompanyRatesAPI.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool isAdmin { get; set; }

        public bool isCompany { get; set; }
    }
}