﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class InvoiceDetails
{
    public int Id { get; set; }

    public int? IdVninvoiceDetail { get; set; }

    public int? Index { get; set; }

    /// <summary>
    /// Tác nghiệp trước
    /// </summary>
    public decimal? DiscountAmountBeforeTax { get; set; }

    /// <summary>
    /// Tác nghiệp trước
    /// </summary>
    public decimal? DiscountPercentBeforeTax { get; set; }

    public decimal? PaymentAmount { get; set; }

    public string ProductCode { get; set; }

    public string ProductId { get; set; }

    public string ProductName { get; set; }

    public int? ProductType { get; set; }

    public string UnitName { get; set; }

    public string UnitId { get; set; }

    public decimal? UnitPrice { get; set; }

    public int? RoundingUnit { get; set; }

    public int? Quantity { get; set; }

    public decimal? Amount { get; set; }

    public int? VatPercent { get; set; }

    public decimal? VatAmount { get; set; }

    public string Note { get; set; }

    public bool? HideUnitPrice { get; set; }

    public bool? HideUnit { get; set; }

    public bool? HideQuantity { get; set; }

    public int? IdInvoice { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Invoices IdInvoiceNavigation { get; set; }
}