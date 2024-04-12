using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BrickHouse.Models
{
    public class CustomerRecommendation
    {
        [Key]
        public string Recommendation { get; set; }
        public int RecommendationCount { get; set; }
        public double Mean_Rating { get; set; }
        public string BecauseYouLiked { get; set; }
        [Key]
        public int CustomerId { get; set; }
    }
}
