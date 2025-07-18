﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

/// <summary>
/// Danh mục đơn vị vận chuyển
/// </summary>
public partial class DeliveryCategories
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string EndPoint { get; set; }

    public bool? IsActived { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public virtual ICollection<SaleOrders> SaleOrders { get; set; } = new List<SaleOrders>();
}