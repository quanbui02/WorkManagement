﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class PromotionSalesGiftProducts
{
    public int Id { get; set; }

    public int? IdProduct { get; set; }

    public int? Quantity { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? IdPromotionSale { get; set; }

    public virtual Products IdProductNavigation { get; set; }

    public virtual PromotionSales IdPromotionSaleNavigation { get; set; }
}