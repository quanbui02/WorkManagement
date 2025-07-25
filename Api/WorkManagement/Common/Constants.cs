using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Work.API.Common
{
    public static class StatusType
    {
        public static int ToDo = 1;
        public static int InProgress = 2;
        public static int Done = 3;
    }

    public static class WmActionsCommon
    {
        public static int ThemCongViec = 1;
        public static int CapNhatTrangThaiCongViec = 2;
        public static int CapNhatTienDo = 3;
        public static int XoaCongViec = 4;
        public static int HoanThanhCongViec = 5;
        public static int CapNhatNguoiDuocGiao = 6;
        public static int XoaNguoiDuocGiao = 7;
        public static int StartDate = 8;
        public static int EndDate = 9;
        public static int Comment = 10;
    }

    // Phân quyền trong dự án
    public static class ProjectRole
    {
        public static int ThanhVien = 1;
        public static int TheoDoi = 2;
        public static int GiamSat = 3;
        public static int QuanLyDuAn = 4;
    }

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
        public static string Project = "Project";
        public static string Work = "Work";
    }

    public static class NotifyType
    {
        public static int Notify = 1;
        public static int Email = 2;
        public static int Update = 3;
        public static int Chat = 4;
    }

    public static class UsersType
    {
        public static int CTV = 1;
        public static int DN = 2;
        public static int Shop = 3;
        public static int NTD = 4;
    }

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
