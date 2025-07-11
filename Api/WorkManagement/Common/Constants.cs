using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkManagement.Common
{
    public static class Constants
    {
        public static string TopicAll = "dapfood";
        public static string TopicProduct = "product_";
        public static string TopicGroup = "group_";
        public static string TopicRole = "role_";
        public static string TopicRoleCTV = "ctv";
        public static string TopicRoleDN = "dn";
        public static string TopicRoleKeToan = "kt";
        public static string TopicRoleQuanTri = "quantri";
        public static string TopicRoleGroupLeader = "groupleader";
        public static string TopicRoleCSKH = "cskh";
    }

    /// <summary>
    /// Nhóm notify
    /// </summary>
    public static class NotifyGroupName
    {
        public static string Order = "Order";
        public static string Product = "Product";
        public static string Feedback = "Feedback";
        public static string Chat = "Chat";
        public static string Feed = "Feed";
        public static string Profile = "Profile";
        public static string OrdersMessage = "OrdersMessage";
    }

    public static class NotifyType
    {
        public static int Notify = 1;
        public static int Email = 2;
        public static int Update = 3;
        public static int Chat = 4;
    }

    public static class ShopInOutType
    {
        public static int Input = 1; // nhập hàng
        public static int Output = 2; // xuất hàng
        public static int Transfer = 3; // điều chuyển từ kho A -> B
        public static int Remove = 4; // Hủy hàng
    }

    /// <summary>
    /// Loại sản phẩm: 1 Sản phẩm, 2 Combo, 3 Quà tặng
    /// </summary>
    public static class ProductType
    {
        public static short SanPham = 1; // bán hàng, mặc định
        public static short Combo = 2; // có sản phẩm hoặc khuyến mãi bên trong
        public static short QuaTang = 3; // không để bán, chỉ để add combo hoặc tặng promotion
    }

    public static class StatusApprove
    {
        public static int GuiDuyet = 1;
        public static int DaDuyet = 2;
        public static int KhongDuyet = 3;
    }

    public static class ActionsConstants
    {
        public static int UpdateInfor = 11;
        public static int ShipOrder = 12;
        public static int ShipActions = 13;
        public static int ShopAction = 14;

    }

    public static class UsersType
    {
        public static int CTV = 1;
        public static int DN = 2;
        public static int Shop = 3;
        public static int NTD = 4;
    }

    public static class StatusAhamove
    {
        public static string IDLE = "IDLE";
        public static string ASSIGNING = "ASSIGNING";
        public static string ACCEPTED = "ACCEPTED";
        public static string INPROCESS = "IN PROCESS";
        public static string COMPLETED = "COMPLETED";
        public static string CANCELLED = "CANCELLED";

    }

    // Loại giao dịch
    public static class DealType
    {
        public static int Thuong = 1;
        public static int HoanLai = 2;
        public static int NapDiem = 3;
        public static int RutDiem = 4;
        public static int HoanDiemRut = 5;
        public static int CongDiemDonHangDN = 6;
        public static int TruDiemLenDonTrucTiep = 7;
        //public static int DongBang = 8;
        public static int DapFoodNhanPhiDichVuTuDN = 9;
        public static int DNTraThuongChoCTV = 10;
        public static int DNTraPhiChoDapFood = 11;
        public static int ThueTNCN = 17;
        public static int MuaDomain = 18;
        public static int Transfer = 21;
        public static int ThuPhiSms = 25;
    }
    public static class StatementType
    {
        public static int DonHang = 1;
        public static int NapRut = 2;
        public static int Sms = 3;
    }
    // Trạng thái giao dịch 
    public static class StatementStatus
    {
        public static int ChuaXuLy = 0;
        public static int ThanhCong = 1;
        public static int DangXuLy = 2;
        public static int Huy = 3;
        public static int Loi = 4;
    }
    //Trạng thái nội dung quảng cáo
    public static class ContentStatus
    {
        public static int CaNhan = 0;
        public static int GuiDuyet = 1;
        public static int DaDuyet = 2;
        public static int TuChoi = 3;
    }
    //Trạng thái đăng ký sản phẩm
    public static class ProductRegStatus
    {
        public static int ChoDuyet = 0;
        public static int DaDuyet = 1;
        public static int Huy = 2;
        public static int TuChoi = 3;
    }
    //Trạng thái đăng ký nhóm
    public static class GroupRegStatus
    {
        public static int ChoDuyet = 0;
        public static int DaDuyet = 1;
        public static int Huy = 2;
        public static int TuChoi = 3;
        public static int MoiVaoNhom = 4;
        public static int TuChoiMoiVaoNhom = 5;
        public static int Xoa = 6;
    }
    // Trạng thái giao dịch 
    public static class PointStatus
    {
        public static int ChuaXuLy = 0;
        public static int DangXuLy = 1;
        public static int Huy = 2;
        public static int KTDuyetThanhCong = 3;
        public static int KTDuyetThatBai = 4;
        public static int AdminDuyetThanhCong = 5;
        public static int AdminDuyetThatBai = 6;
        public static int Loi = 7;
        public static int ThanhCong = 8;
    }

    public static class EnumOrderStatus
    {
        public static int DaXacNhan = 1;
        public static int DangChuanBiHang = 2;
        public static int HoanGiaoHang = 3;
        public static int DaChuanBiHangXong = 20;
        public static int DaLayHang = 21;
        public static int KhongLayDuocHang = 22;
        public static int DangGiaoHang = 30;
        public static int DaGiaoHang = 31;
        public static int KhongGiaoDuoc = 33;
        public static int YeuCauGiaoLai = 34;
        public static int DangHoan = 40;
        public static int DaHoan = 41;
        public static int ShipDangLayHang = 23;
        public static int HuyDon = 999;
        public static int ChuaXacNhan = 1000;
    }

    public static class PayStatus
    {
        public static int Nothing = 0;
        public static int ThanhCong = 1;
        public static int ThatBai = 2;
    }

    public static class SmsType
    {
        public static int RutTien = 1;
        public static int DangKyTaiKhoan = 2;
        public static int DangKyHoacDangNhapNhanh = 8;
        public static int KhoiPhucMatKhau = 3;
        public static int UpdateBank = 4;
        public static int Domainregister = 5;
        public static int XacNhanDonHang = 6;
        public static int CamOnMuaHang = 7;
    }

    public static class PaymentChannel
    {
        public static int Bapi = 1;
        public static int TienMat = 0;
        public static int Alepay = 2;
        public static int Momo = 2;
        public static int Zalo = 3;
        public static int VnPay = 4;
    }
    public static class DomainType
    {
        public static int VN = 1;
        public static int International = 2;
        public static int Free = 3;
    }

    public static class PromotionType
    {
        public static int TongTien = 1;
        public static int SanPham = 2;
        public static int PhiShip = 5;
        public static int GioiThieu = 6;
        public static int Code = 3;
        public static int GiamTrucTiepTrenDonHang = 4;
        public static int TichLuy = 7;
        public static int ThuongTichLuyDoanhSo = 8;
    }

    public static class PromotionMode
    {
        public static int PhanTram = 1;
        public static int Diem = 2;
        public static int QuaTang = 3;
    }

    public static class GiftType
    {
        public static int FreeShip = 1;
        public static int Voucher = 2;
        public static int Product = 3;
    }


    public static class TagType
    {
        public static int News = 1;
        public static int Product = 2;
    }

    public class OmiCallEnvironment
    {
        //path api
        public static string Auth_url = "/auth?apiKey=";     //xác thực

        public static string Agent_invite_url = "/agent/invite";     //thêm mới nhân viên
        public static string Agent_list_url = "/agent/list";               //danh sách nhân vien
        public static string Agent_delete_url = "/agent/delete?identify_info=";     //Xóa nhân viên

        public static string Contacts_add_url = "/contacts/add";             //tạo mới khách hàng
        public static string Contacts_list_url = "/contacts/list";     //tìm kiếm khách hàng
        public static string Contacts_update_url = "/contacts/update";     //Sửa thông tin khách hàng
        public static string Contacts_delete_url = "/contacts/delete/";     //xóa khách hàng
        public static string Contacts_add_more_url = "/contacts/add-more";     //tìm kiếm khách hàng
        public static string Contacts_get = "/contacts/get";     //tìm kiếm khách hàng

        public static string Call_transaction_list_url = "/call_transaction/list?";             //danh sách lịch sử cuộc gọi
        public static string Call_transaction_report_url = "/call_transaction/report?";     //thống kê cuộc gọi
        public static string Call_transaction_detail_url = "/call_transaction/detail/";     //chi tiết cuộc gọi
        public static string Call_transaction_change_url = "/call_transaction/change/";     //cập nhật thông tin cuộc gọi

        //mật khẩu mặc định khi tạo người dùng mới
        public static string Omicall_AgentInvite_Password = "@1Qaz2wsx";
    }

    //public static class ActionsOrderType
    //{
    //    public static int ConfirmedOrder = 1;     // Chốt đơn
    //    public static int CancelOrder = 2;  // Hủy đơn
    //    public static int CreateLogisticsOrder = 3;   // Đăng đơn
    //    public static int CancelLogisticsOrder = 4;   // Hủy đăng đơn
    //    public static int ChangeContactCode = 5;  // Đổi mã vận đơn
    //    public static int SyncLogisticStatus = 6; // Cập nhật trạng thái giao hàng mới
    //    public static int CancelContactForBusinesses = 7; // Doanh nghiệp hủy chốt đơn

    //}

    public class TokenType
    {
        public static int Header = 1;
        public static int Bearer = 2;
    }
    public class ServiceHttpResult
    {
        public bool isSuccess { get; set; }
        public string result { get; set; }
        public string error { get; set; }
        public int code { get; set; }

        public ServiceHttpResult()
        {

        }
        public ServiceHttpResult(int code, string data)
        {
            this.code = code;
            this.isSuccess = false;
            if (code >= 200 && code < 300)
            {
                this.isSuccess = true;
                this.result = data;
            }
            else
            {
                this.error = data;
            }

        }
    }

}
