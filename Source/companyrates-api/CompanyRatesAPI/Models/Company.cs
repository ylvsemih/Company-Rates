using System.ComponentModel.DataAnnotations;

namespace CompanyRatesAPI.Models
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string LogoUrl { get; set; }
        public bool Verified { get; set; }

        public int TotalRating { get; set; }

        public User UserFK { get; set; }

        public int User_FK { get; set; }
    }
}