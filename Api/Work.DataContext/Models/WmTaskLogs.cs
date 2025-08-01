﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class WmTaskLogs
{
    public int Id { get; set; }

    /// <summary>
    /// Người chủ trì
    /// </summary>
    public int? IdAssignee { get; set; }

    public int? IdProject { get; set; }

    public int? IdProjectCol { get; set; }

    public int? IdTask { get; set; }

    /// <summary>
    /// Hành động
    /// </summary>
    public int? IdAction { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Phần trăm công việc
    /// </summary>
    public int? Percent { get; set; }

    /// <summary>
    /// 1. ToDo, 2. InProgress, 3. Done
    /// </summary>
    public int? IdStatus { get; set; }

    /// <summary>
    /// Mô tả hành động
    /// </summary>
    public string Description { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public virtual WmActions IdActionNavigation { get; set; }

    public virtual WmTasks IdTaskNavigation { get; set; }
}