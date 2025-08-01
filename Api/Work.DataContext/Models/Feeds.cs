﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class Feeds
{
    public int Id { get; set; }

    /// <summary>
    /// CTV
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Mã sản phẩm
    /// </summary>
    public int? IdProduct { get; set; }

    public string Content { get; set; }

    /// <summary>
    /// Ảnh
    /// </summary>
    public string Images { get; set; }

    /// <summary>
    /// Lượt like
    /// </summary>
    public int? Like { get; set; }

    /// <summary>
    /// Lượt Comment
    /// </summary>
    public int? Comment { get; set; }

    public string TitleLink { get; set; }

    public string DescriptionLink { get; set; }

    public string ImageLink { get; set; }

    public string UrlLink { get; set; }

    /// <summary>
    /// Sắp xếp
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// Ghim lên đầu
    /// </summary>
    public bool? Pin { get; set; }

    public bool? IsShowVideoLink { get; set; }

    public bool? IsActived { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public virtual ICollection<Comments> Comments { get; set; } = new List<Comments>();

    public virtual Products IdProductNavigation { get; set; }

    public virtual ICollection<Likes> Likes { get; set; } = new List<Likes>();

    public virtual Users User { get; set; }
}