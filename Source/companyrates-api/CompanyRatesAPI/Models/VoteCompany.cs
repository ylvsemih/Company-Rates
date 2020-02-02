using System.ComponentModel.DataAnnotations;

namespace CompanyRatesAPI.Models
{
    public class VoteCompany
    {
        [Key]
        public int VoteCompanyID { get; set; }

        public int Value { get; set; }
        public User UserFK { get; set; }

        public int User_FK { get; set; }

        public Company CompanyFK { get; set; }

        public int Company_FK { get; set; }
    }
}