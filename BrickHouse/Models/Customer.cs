using System;
using System.Collections.Generic;

namespace BrickHouse.Models;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string CountryOfResidence { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public double Age { get; set; }
}
