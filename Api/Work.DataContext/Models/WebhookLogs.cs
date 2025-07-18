﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class WebhookLogs
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Token { get; set; }

    public string Url { get; set; }

    /// <summary>
    /// Payload
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    /// Id đơn hàng
    /// </summary>
    public string ObjectId { get; set; }

    /// <summary>
    /// Nhận
    /// </summary>
    public bool? IsRecipient { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// Kết quả respon
    /// </summary>
    public string Result { get; set; }

    /// <summary>
    /// App hay web
    /// </summary>
    public bool? IsApp { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }
}