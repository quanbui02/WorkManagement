﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class LessonDetails
{
    public int Id { get; set; }

    public int? IdLession { get; set; }

    public int? IdPartLession { get; set; }

    /// <summary>
    /// thứ tự hiện thị
    /// </summary>
    public int? DisplayLesson { get; set; }

    public string Name { get; set; }

    public string ImageLessionDetail { get; set; }

    public string Description { get; set; }

    public string DescriptionDetail { get; set; }

    public string LessonVideo { get; set; }

    public double? TimeVideo { get; set; }

    public string LessonFile { get; set; }

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

    public virtual PartLessions IdPartLessionNavigation { get; set; }

    public virtual ICollection<NoteCourse> NoteCourse { get; set; } = new List<NoteCourse>();
}