﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class ProductProvinces
{
    public int Id { get; set; }

    public int? IdProduct { get; set; }

    public int? IdProvince { get; set; }

    public bool? IsActived { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Products IdProductNavigation { get; set; }

    public virtual Provinces IdProvinceNavigation { get; set; }
}