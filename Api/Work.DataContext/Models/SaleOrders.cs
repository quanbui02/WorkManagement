﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Work.DataContext.Models;

public partial class SaleOrders
{
    public int Id { get; set; }

    public int? IdParent { get; set; }

    /// <summary>
    /// Đơn ref của sale
    /// </summary>
    public int? IdUserSale { get; set; }

    /// <summary>
    /// Id cha của đơn tách
    /// </summary>
    public int IdClient { get; set; }

    public int UserId { get; set; }

    /// <summary>
    /// UserId Sub
    /// </summary>
    public int? IdUserSup { get; set; }

    public int IdRef { get; set; }

    /// <summary>
    /// Hình thức giao hàng
    /// </summary>
    public int? IdDelivery { get; set; }

    /// <summary>
    /// Mã đơn
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Mã đơn giao vận
    /// </summary>
    public string CodeShip { get; set; }

    /// <summary>
    /// Mã đơn giao vận
    /// </summary>
    public string ShipStatus { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public int? IdProvince { get; set; }

    public int? IdDistrict { get; set; }

    public int? IdWard { get; set; }

    public string Address { get; set; }

    public string Note { get; set; }

    /// <summary>
    /// Tổng tiền hàng
    /// </summary>
    public double? Total { get; set; }

    /// <summary>
    /// Phí vận chuyển
    /// </summary>
    public double? Ship { get; set; }

    /// <summary>
    /// Phí vận chuyển
    /// </summary>
    public double? ShipRoot { get; set; }

    /// <summary>
    /// Giảm giá
    /// </summary>
    public double? Discount { get; set; }

    /// <summary>
    /// Tổng tiền đơn
    /// </summary>
    public double? TotalBill { get; set; }

    /// <summary>
    /// Tiền đặt cọc
    /// </summary>
    public double? Deposit { get; set; }

    /// <summary>
    /// Thưởng từ sản phẩm
    /// </summary>
    public double? ProductReward { get; set; }

    /// <summary>
    /// Thưởng từ đơn trả tiền trước
    /// </summary>
    public double? PrepayReward { get; set; }

    /// <summary>
    /// Thưởng chương trình
    /// </summary>
    public double? PromotionReward { get; set; }

    /// <summary>
    /// Tổng quà tặng
    /// </summary>
    public double? TotalGift { get; set; }

    /// <summary>
    /// Tổng tiền thưởng
    /// </summary>
    public double? TotalReward { get; set; }

    /// <summary>
    /// Tổng tiền thưởng giới thiệu
    /// </summary>
    public double? TotalRewardReferral { get; set; }

    /// <summary>
    /// Phí dapfood
    /// </summary>
    public double? SystemFee { get; set; }

    /// <summary>
    /// Theo chương trình km nào
    /// </summary>
    public int? IdPromotion { get; set; }

    /// <summary>
    /// Đơn chốt
    /// </summary>
    public bool? IsVerified { get; set; }

    /// <summary>
    /// 1: Thành công, 2: Hủy, 0: Nothing
    /// </summary>
    public int? PayStatus { get; set; }

    /// <summary>
    /// Đã trừ tiền đơn trả trước của ctv
    /// </summary>
    public bool? IsPaid { get; set; }

    /// <summary>
    /// Đã trả thưởng cho ctv
    /// </summary>
    public bool? IsPaidReward { get; set; }

    /// <summary>
    /// Trạng thái tác nghiệp hiện tại
    /// </summary>
    public int? IdAction { get; set; }

    /// <summary>
    /// Trạng thái giao hàng hiện tại
    /// </summary>
    public int? IdStatus { get; set; }

    /// <summary>
    /// Trạng thái giao hàng hiện tại
    /// </summary>
    public int? LogisticId { get; set; }

    /// <summary>
    /// Trạng thái giao hàng hiện tại
    /// </summary>
    public string TranportId { get; set; }

    /// <summary>
    /// Trạng thái bán hàng
    /// </summary>
    public string Reason { get; set; }

    /// <summary>
    /// Thanh toán luôn
    /// </summary>
    public bool? IsPrepay { get; set; }

    /// <summary>
    /// Lên đơn trên dapfood
    /// </summary>
    public bool? IsManual { get; set; }

    /// <summary>
    /// Kênh thanh toán: 1 điểm dapfood, 2 ngân lượng
    /// </summary>
    public int? PaymentChannel { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public bool? IsRated { get; set; }

    /// <summary>
    /// Ngày mong muốn nhận hàng
    /// </summary>
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// Khách hàng tự chốt qua link
    /// </summary>
    public bool? IsCustomerConfirmed { get; set; }

    /// <summary>
    /// Id Sale Admin
    /// </summary>
    public int? IdUserSo { get; set; }

    public int? IdPromotionSale { get; set; }

    /// <summary>
    /// 1: đơn hàng định mức, 2: đơn đặt hàng sản xuất
    /// </summary>
    public int? SaleOrderType { get; set; }

    /// <summary>
    /// Quãng đường
    /// </summary>
    public double? Distance { get; set; }

    public double? Lat { get; set; }

    public double? Lng { get; set; }

    public string FullAddress { get; set; }

    public bool? ReceiveAtShop { get; set; }

    public bool? IsInvoiced { get; set; }

    public int? IdInvoice { get; set; }

    public bool? IsInvoice { get; set; }

    public int? IdPromotionSaleShip { get; set; }

    public string UtmSource { get; set; }

    public string Voucher { get; set; }

    public double? DiscountShip { get; set; }

    public string BankCode { get; set; }

    /// <summary>
    /// Đơn đặt hàng trước
    /// </summary>
    public bool? IsPreOrder { get; set; }

    /// <summary>
    /// Điểm thưởng tích lũy: tiền hàng * phí hoa hồng trả của sàn
    /// </summary>
    public double? Points { get; set; }

    public int? IdPromotionReward { get; set; }

    public int? IdUserAddress { get; set; }

    public virtual SaleActions IdActionNavigation { get; set; }

    public virtual DeliveryCategories IdDeliveryNavigation { get; set; }

    public virtual Districts IdDistrictNavigation { get; set; }

    public virtual PromotionSales IdPromotionRewardNavigation { get; set; }

    public virtual PromotionSales IdPromotionSaleNavigation { get; set; }

    public virtual PromotionSales IdPromotionSaleShipNavigation { get; set; }

    public virtual Provinces IdProvinceNavigation { get; set; }

    public virtual SaleStatus IdStatusNavigation { get; set; }

    public virtual Users IdUserSaleNavigation { get; set; }

    public virtual Users IdUserSoNavigation { get; set; }

    public virtual Users IdUserSupNavigation { get; set; }

    public virtual Wards IdWardNavigation { get; set; }

    public virtual ICollection<OrderPromotionSales> OrderPromotionSales { get; set; } = new List<OrderPromotionSales>();

    public virtual ICollection<PromotionSalesGiftcode> PromotionSalesGiftcode { get; set; } = new List<PromotionSalesGiftcode>();

    public virtual ICollection<SaleOrderActions> SaleOrderActions { get; set; } = new List<SaleOrderActions>();

    public virtual ICollection<SaleOrderDetails> SaleOrderDetails { get; set; } = new List<SaleOrderDetails>();

    public virtual ICollection<SaleOrderStatus> SaleOrderStatus { get; set; } = new List<SaleOrderStatus>();

    public virtual Users User { get; set; }

    public virtual ICollection<WarehouseHistories> WarehouseHistories { get; set; } = new List<WarehouseHistories>();
}