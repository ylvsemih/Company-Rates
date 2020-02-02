using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyRatesAPI.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }

        public string Text { get; set; }
        public string Category { get; set; }

        public User UserFK { get; set; }

        public int User_FK { get; set; }

        public Company CompanyFK { get; set; }

        public int Company_FK { get; set; }

        public DateTime DateTimeAdded { get; set; }

        public int TotalRating { get; set; }
    }
}