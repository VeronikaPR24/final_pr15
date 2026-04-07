using System;
using System.Collections.Generic;

namespace final_pr15.Models;

public partial class ProductTag
{
    public int ProductId { get; set; }

    public int TagId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
