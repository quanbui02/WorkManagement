﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class NewsCategories
{
    public int Id { get; set; }

    public string Icon { get; set; }

    public int? IdParent { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// Ảnh đại diện
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Ảnh đại diện
    /// </summary>
    public string Avatar { get; set; }

    public string TitleSeo { get; set; }

    public string DescriptionSeo { get; set; }

    public string DetailHeaderSeo { get; set; }

    public string DetailFooterSeo { get; set; }

    /// <summary>
    /// 0: Chờ duyệt, 1: đã duyệt, 2: hủy, 3 từ chối
    /// </summary>
    public bool? IsPrivate { get; set; }

    /// <summary>
    /// 0: Chờ duyệt, 1: đã duyệt, 2: hủy, 3 từ chối
    /// </summary>
    public bool? IsActive { get; set; }

    public int? Sort { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();
}