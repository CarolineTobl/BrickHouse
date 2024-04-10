using System.ComponentModel.DataAnnotations;
namespace BrickHouse.Models
{
    public class TwoFactor
    {
        [Required]
        public string TwoFactorCode { get; set; }
    }
}