using BrickHouse.Models;

public class CheckoutViewModel
{
    public string SelectedBank { get; set; }
    public IEnumerable<string> UniqueBanks { get; set; }

    public string SelectedCardType { get; set; }
    public IEnumerable<string> UniqueCardTypes { get; set; }

    public string SelectedCountryOfTransaction { get; set; }
    public IEnumerable<string> UniqueCountriesOfTransaction { get; set; }

    public string SelectedShippingAddress { get; set; }
    public IEnumerable<string> UniqueShippingAddresses { get; set; }

}
