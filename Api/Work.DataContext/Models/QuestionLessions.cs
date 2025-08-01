﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class QuestionLessions
{
    public int Id { get; set; }

    /// <summary>
    /// Bài giảng
    /// </summary>
    public int? IdLession { get; set; }

    /// <summary>
    /// Tóm tắt
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    /// nội dung câu hỏi, trả lời
    /// </summary>
    public string ContentQuestion { get; set; }

    /// <summary>
    /// Sắp xếp thứ tự
    /// </summary>
    public int? Sort { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public virtual Lessons IdLessionNavigation { get; set; }
}