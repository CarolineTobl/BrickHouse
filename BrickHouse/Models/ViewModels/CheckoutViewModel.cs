using System.Text.Json.Serialization;
using BrickHouse.Infrastructure;
using BrickHouse.Models;

public class CheckoutViewModel
{ 
    // Values for dropdown lists
    public IEnumerable<string> UniqueBanks { get; set; }
    public IEnumerable<string> UniqueCardTypes { get; set; }
    public IEnumerable<string> UniqueCountriesOfTransaction { get; set; }
    public IEnumerable<string> UniqueShippingAddresses { get; set; }
    
    // Empty objects for submission
    public Order Order { get; set; }
    public Cart Cart { get; set; }
}
