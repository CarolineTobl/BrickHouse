using System.ComponentModel.DataAnnotations.Schema;

namespace BrickHouse.Models
{
    public class CustomerRecommendations
    {
        public string Recommendation { get; set; }

        public int RecommendationCount { get; set; }

        public string BecauseYouLiked { get; set; }

        public int CustomerId { get; set; }
    }
}
