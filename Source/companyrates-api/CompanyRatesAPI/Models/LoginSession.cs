using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyRatesAPI.Models
{
    public class LoginSession
    {
        [Key]
        public int LoginSessionID { get; set; }

        public string SessionKey { get; set; }
        public User UserFK { get; set; }

        public int User_FK { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }
    }
}