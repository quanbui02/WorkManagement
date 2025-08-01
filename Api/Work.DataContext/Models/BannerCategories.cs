﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class BannerCategories
{
    public int Id { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// 0: Chờ duyệt, 1: đã duyệt, 2: hủy, 3 từ chối
    /// </summary>
    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public virtual ICollection<Banners> Banners { get; set; } = new List<Banners>();
}