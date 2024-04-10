using System;
using System.Collections.Generic;

namespace BrickHouse.Models;

public partial class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; } = null!;
    public short Year { get; set; }
    public short NumParts { get; set; }
    public short Price { get; set; }
    public string ImgLink { get; set; } = null!;
    public string? PrimaryColor { get; set; } = null!;
    public string? SecondaryColor { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public string? PrimaryCategory { get; set; }
    public  string? SecondaryCategory { get; set;}
    public string? TertiaryCategory { get; set; }

}
