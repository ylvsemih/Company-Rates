using System.ComponentModel.DataAnnotations;

namespace CompanyRatesAPI.Models
{
    public class VoteReview
    {
        [Key]
        public int VoteReviewID { get; set; }

        public int Value { get; set; }
        public User UserFK { get; set; }

        public int User_FK { get; set; }

        public Review ReviewFK { get; set; }

        public int Review_FK { get; set; }
    }
}