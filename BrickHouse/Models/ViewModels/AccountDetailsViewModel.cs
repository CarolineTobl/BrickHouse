using System.ComponentModel.DataAnnotations;

namespace BrickHouse.Models.ViewModels;

public class AccountDetailsViewModel
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Gender { get; set; }

    [Required]
    //[DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    [Required]
    public string Country { get; set; }

    // This will not be a form field but will be set in the controller before saving to the database
    public string AspNetUserId { get; set; }
}
