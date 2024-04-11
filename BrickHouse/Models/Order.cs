using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrickHouse.Models;

public partial class Order
{
    [Key]
    public int TransactionId { get; set; }

    public int CustomerId { get; set; }

    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now.Date);

    public string DayOfWeek { get; set; } = DateTime.Now.ToString("ddd");

    public byte Time { get; set; } = (byte)DateTime.Now.Hour;

    public string EntryMode { get; set; } = "CVC";

    public double Amount { get; set; }

    public string TypeOfTransaction { get; set; } = "Online";

    public string CountryOfTransaction { get; set; } = null!;

    public string ShippingAddress { get; set; } = null!;

    public string Bank { get; set; } = null!;

    public string TypeOfCard { get; set; } = null!;

    public int Fraud { get; set; }
}
