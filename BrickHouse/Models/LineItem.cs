using System;
using System.Collections.Generic;

namespace BrickHouse.Models;

public partial class LineItem
{
    public int TransactionId { get; set; }

    public int ProductId { get; set; }

    public byte Qty { get; set; }

    public byte Rating { get; set; }
}
