﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class LogSms
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Phone { get; set; }

    /// <summary>
    /// Mã OTP
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Mô tả
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 1: Rút tiền, 2: Đăng ký tài khoản, 3: Quên mật khẩu
    /// </summary>
    public int? Type { get; set; }

    public bool? IsActive { get; set; }

    /// <summary>
    /// Trạng thái nhà mạng trả về
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    /// Trạng thái nhà mạng trả về
    /// </summary>
    public string StatusDesc { get; set; }

    /// <summary>
    /// Lỗi nhà mạng trả về
    /// </summary>
    public string Error { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? Sms { get; set; }

    public int? Cost { get; set; }

    public string TranId { get; set; }
}