﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class Domains
{
    public int Id { get; set; }

    /// <summary>
    /// Loại
    /// </summary>
    public int? Type { get; set; }

    public string Name { get; set; }

    public double? PricePromotion { get; set; }

    public double? Price { get; set; }

    public double? Renew { get; set; }

    public double? Transfer { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? Sort { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }
}