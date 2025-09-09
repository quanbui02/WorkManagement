using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Work.DataContext.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Linq;
using Newtonsoft.Json.Serialization;
using System.Net;
using HtmlAgilityPack;

namespace WorkManagement.Common
{
    public static class Lib
    {
        static Random random = new Random();
        public static async Task<dynamic> MethodPost(string url, string token, object data = null, Method method = Method.Post, Dictionary<string, string> headerdata = null)
        {
            //var dataJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            var dataJson = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            //var dataJson = JObject.FromObject(data);

            var client = new RestClient(new RestClientOptions(url)
            {
                Timeout = null
            });

            var request = new RestRequest { Method = method };
            if (!string.IsNullOrEmpty(token))
            {
                if (token.Contains("Bearer "))
                    request.AddHeader("Authorization", token);
                else
                    request.AddHeader("Authorization", "Bearer " + token);
            }
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            foreach (KeyValuePair<string, string> kv in headerdata)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    request.AddHeader(WebUtility.UrlEncode(kv.Key), WebUtility.UrlEncode(kv.Value));
                }
            }
            request.AddParameter("application/json; charset=utf-8", dataJson, ParameterType.RequestBody);
            var response = await client.ExecuteAsync(request);

            return response.Content;
        }
        public static async Task<dynamic> MethodPostResponsePdf(string url, string token, object data = null, Dictionary<string, string> headerdata = null, Method method = Method.Post)
        {
            //var dataJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            var dataJson = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            //var dataJson = JObject.FromObject(data);

            var client = new RestClient(new RestClientOptions(url)
            {
                Timeout = null
            });

            var request = new RestRequest { Method = method };
            if (!string.IsNullOrEmpty(token))
            {
                if (token.Contains("Bearer "))
                    request.AddHeader("Authorization", token);
                else
                    request.AddHeader("Authorization", "Bearer " + token);
            }
            request.AddHeader("Content-Type", "application/pdf");
            request.AddHeader("Content-Disposition", "attachment; filename=");
            request.AddHeader("Content-Transfer-Encoding", "binary");
            foreach (KeyValuePair<string, string> kv in headerdata)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    request.AddHeader(WebUtility.UrlEncode(kv.Key), WebUtility.UrlEncode(kv.Value));
                }
            }
            request.AddParameter("application/json; charset=utf-8", dataJson, ParameterType.RequestBody);
            var response = await client.ExecuteAsync(request);

            return response;
        }
        public static async Task<dynamic> MethodPostXF(string url, string token, Dictionary<string, string> data = null, Method method = Method.Post)
        {

            var client = new RestClient(new RestClientOptions(url)
            {
                Timeout = null
            });

            var request = new RestRequest { Method = method };
            if (!string.IsNullOrEmpty(token))
            {
                if (token.Contains("Bearer "))
                    request.AddHeader("Authorization", token);
                else
                    request.AddHeader("Authorization", "Bearer " + token);
            }
            //request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            foreach (KeyValuePair<string, string> kv in data)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    request.AddParameter(WebUtility.UrlEncode(kv.Key), WebUtility.UrlEncode(kv.Value));
                }
            }
            //request.AddParameter("application/json; charset=utf-8", dataJson, ParameterType.RequestBody);
            var response = await client.ExecuteAsync(request);
            return response.Content;
        }

        public static async Task<dynamic> RestRequest(string url)
        {
            var client = new RestClient(new RestClientOptions(url)
            {
                Timeout = null
            });

            var request = new RestRequest { Method = Method.Get };

            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            var response = await client.ExecuteAsync(request);
            return response.Content;
        }

        //public static void SendNotify(NotificationInfo data)
        //{
        //    if (string.IsNullOrEmpty(data.Message.Image))
        //        data.Message.Image = $"{Config.Services.File}/Images/logoFriends.png";

        //    var dataJson = JObject.FromObject(data);
        //    dataJson.Remove("Id");
        //    var client = new RestClient($"{Config.Services.Notification}/Notifications?key={Config.Key}") { Timeout = -1 };
        //    var request = new RestRequest { Method = Method.POST };
        //    //if (token.Contains("Bearer "))
        //    //    request.AddHeader("Authorization", token);
        //    //else
        //    //    request.AddHeader("Authorization", "Bearer " + token);
        //    request.AddHeader("Content-Type", "application/json; charset=utf-8");
        //    request.AddParameter("application/json; charset=utf-8", dataJson, ParameterType.RequestBody);
        //    var response = client.ExecuteAsync(request);
        //}
        public static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        //public static void PushToLog(object data)
        //{
        //    try
        //    {
        //        var dataJson = JObject.FromObject(data);
        //        var client = new RestClient($"{Config.ServiceInternal.Log}/logs?key={Config.KeyPublic}") { Timeout = -1 };
        //        var request = new RestRequest { Method = Method.POST };
        //        //if (token.Contains("Bearer "))
        //        //    request.AddHeader("Authorization", Config.DapFood.TokenDapFood);
        //        //else
        //        //    request.AddHeader("Authorization", "Bearer " + Config.DapFood.TokenDapFood);
        //        request.AddHeader("Content-Type", "application/json; charset=utf-8");
        //        request.AddParameter("application/json; charset=utf-8", dataJson, ParameterType.RequestBody);
        //        var response = client.ExecuteAsync(request);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("PushToLog Error: " + ex.Message);
        //    }
        //}

        //public static void PushToWebhookLogs(object data)
        //{
        //    try
        //    {
        //        var dataJson = JObject.FromObject(data);
        //        var client = new RestClient($"{Config.ServiceInternal.Log}/WebhookLogs?key={Config.KeyPublic}") { Timeout = -1 };
        //        var request = new RestRequest { Method = Method.POST };
        //        //if (token.Contains("Bearer "))
        //        //    request.AddHeader("Authorization", Config.DapFood.TokenDapFood);
        //        //else
        //        //    request.AddHeader("Authorization", "Bearer " + Config.DapFood.TokenDapFood);
        //        request.AddHeader("Content-Type", "application/json; charset=utf-8");
        //        request.AddParameter("application/json; charset=utf-8", dataJson, ParameterType.RequestBody);
        //        var response = client.ExecuteAsync(request);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("PushToWebhookLogs Error: " + ex.Message);
        //    }
        //}

        //public static void PushToOrderMessage(object data)
        //{
        //    try
        //    {
        //        var dataJson = JObject.FromObject(data);
        //        var client = new RestClient($"{Config.ServiceInternal.Log}/OrdersMessage?key={Config.KeyPublic}") { Timeout = -1 };
        //        var request = new RestRequest { Method = Method.POST };
        //        //if (token.Contains("Bearer "))
        //        //    request.AddHeader("Authorization", Config.DapFood.TokenDapFood);
        //        //else
        //        //    request.AddHeader("Authorization", "Bearer " + Config.DapFood.TokenDapFood);
        //        request.AddHeader("Content-Type", "application/json; charset=utf-8");
        //        request.AddParameter("application/json; charset=utf-8", dataJson, ParameterType.RequestBody);
        //        var response = client.ExecuteAsync(request);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("PushToOrdersMessage Error: " + ex.Message);
        //    }
        //}

        //public static string CreateLinkAffiliate(string link, int idSource, int idCtv)
        //{
        //    if (string.IsNullOrEmpty(link))
        //        return "";

        //    if (link.IndexOf('?') > 0)
        //    {
        //        link += "&utm_source=cc_" + idSource + "_" + idCtv;
        //    }
        //    else
        //    {
        //        link += "?utm_source=cc_" + idSource + "_" + idCtv;
        //    }
        //    return link;
        //}

        //public static string CreateLinkShare(string slug, int userId, int? idSource)
        //{
        //    return $"{slug}?utm_source=cc_{idSource}_{userId}";
        //    //return $"{Config.Services.Link}/{MyString.Slug(slug)}_{userId}-{idSource}";
        //}

        //public static string CreateLinkOrder(Users user, string slug, int idProduct)
        //{
        //    if (!string.IsNullOrEmpty(user.Domain) && user.DomainStatus == 2)
        //        return $"http://{user.Domain}/{MyString.Slug(slug)}_{idProduct}";
        //    else
        //        return $"{Config.Services.LinkOrder}/{user.UserId}/{MyString.Slug(slug)}_{idProduct}";
        //}

        //public static string LinkWebsiteShare(Users user, string slug, int idProduct)
        //{
        //    return $"{Config.Services.Website}/{MyString.Slug(slug)}_{idProduct}{(user != null && !string.IsNullOrEmpty(user.Code) ? "?r=" + user.Code : "")}";
        //}

        //public static UtmSourceModel GetValueUtmSource(string str)
        //{
        //    var data = str.Split('_');
        //    var obj = new UtmSourceModel()
        //    {
        //        IdClient = Convert.ToInt32(data[1]),
        //        UserId = Convert.ToInt32(data[2]),
        //    };
        //    return obj;
        //}

        public static string CreateOrdersCode(int idOrder, string codeClient)
        {
            return $"{codeClient}BP00{idOrder}";
        }

        //public static string urlImageAvatar(string id)
        //{
        //    return $"{Config.Services.File}/Files/image/a/{id}";//&size=Avatar
        //}

        //public static string urlImageMedium(string id)
        //{
        //    return $"{Config.Services.File}/Files/image/m/{id}";
        //}

        //public static string urlImageLarge(string id)
        //{
        //    return $"{Config.Services.File}/Files/image/l/{id}";
        //}

        //public static string urlImageOriginal(string id)
        //{
        //    return $"{Config.Services.File}/Files/image/o/{id}";
        //}

        public static string FormatNumber(double number)
        {
            try
            {
                return number.ToString("#,##0", new CultureInfo("vi-VN"));
            }
            catch
            {
                return "0";
            }
        }

        public static string FormatPhone(string phone)
        {
            try
            {
                //var regex = "^(\\d{10}|\\d{11}|\\d{12}|(\\+84\\d{10}))$";
                var regex = "^((84|0[3|5|7|8|9])+([0-9]{8})\b)$";
                var match = Regex.Match(phone, regex, RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    phone = phone.Substring(0, 4) + "***" + phone.Substring(7);
                }
                return phone;
            }
            catch
            {
                return phone;
            }
        }

        public static string CalculateTime(DateTime yourDate)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - yourDate.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "một giây trước" : ts.Seconds + " giây trước";

            if (delta < 2 * MINUTE)
                return "một phút trước";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " phút trước";

            if (delta < 90 * MINUTE)
                return "một giờ trước";

            if (delta < 24 * HOUR)
                return ts.Hours + " giờ trước";

            if (delta < 48 * HOUR)
                return "hôm qua";

            if (delta < 2 * DAY)
                return ts.Days + " ngày trước";

            return yourDate.ToString("HH:mm dd/MM/yyyy");
        }

        public static string ToMD5(string str)
        {
            //str += "Cc@Ps";
            var enc = Encoding.Unicode.GetEncoder();
            var unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);
            var sb = new StringBuilder();
            foreach (byte t in result)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }

        public static string GenerateVoucher(char[] keys, int lengthOfVoucher)
        {
            return Enumerable
                .Range(1, lengthOfVoucher) // for(i.. ) 
                .Select(k => keys[random.Next(0, keys.Length - 1)])  // generate a new random char 
                .Aggregate("", (e, c) => e + c); // join into a string
        }

        #region Omicall
        public static long getLongTime(DateTime dt)
        {
            long millis = (long)(dt.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return millis;
        }

        public static long getMilliSeconds()
        {
            long millis = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return millis;
        }

        public static DateTime convertLongtoDateTime(long unixDate)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddSeconds(unixDate).ToLocalTime();
            return date;
        }
        public static DateTime convertLongtoDateTimeMilliseconds(long unixDate)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
            return date;
        }


        #endregion

        public static string ConvertObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            });
        }

        //upload image base64
        //public static async Task<string> UploadFileImageForEditor(string htmlContent)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(htmlContent) && htmlContent.Contains("data:image") && htmlContent.Contains("base64"))
        //        {
        //            //if (((form.Id > 0 && obj.Detail != form.Detail) || (form.Id <= 0)))
        //            //{
        //            HtmlDocument doc = new HtmlDocument();
        //            doc.LoadHtml(htmlContent);
        //            var itemList = doc.DocumentNode.SelectNodes("//img");
        //            var lst = new List<object>();
        //            var url = Config.Services.File + "/files/uploadImageForEditor";
        //            foreach (HtmlNode node2 in itemList)
        //            {
        //                lst = new List<object>();
        //                var noteValue = node2.Attributes["src"].Value;
        //                if (noteValue.Contains("data:image/") && noteValue.Contains(";base64,"))
        //                {
        //                    var imgNodes = noteValue.Replace("data:image/", "").Split(";base64,");
        //                    var item = new
        //                    {
        //                        fileName = "Foods_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + (imgNodes[0] == "jpeg" ? ".jpg" : ".png"),
        //                        content = imgNodes[1],
        //                    };
        //                    lst.Add(item);
        //                    var data = await Lib.MethodPost(url, "", lst, headerdata: new Dictionary<string, string>());
        //                    if (data != null)
        //                    {
        //                        var re = JsonConvert.DeserializeObject<ProductImagesObject>(data);
        //                        if (re != null && re.status && re.data != null)
        //                        {
        //                            htmlContent = htmlContent.Replace(noteValue, Config.Services.File + "/Editors/" + re.data[0].PhysicalPath);
        //                        }
        //                    }
        //                }
        //                //}
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, new Exception(null));
        //    }
        //    return htmlContent;
        //}


        // Check email hợp lệ
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Check số điện thoại VN cơ bản (10 số, bắt đầu bằng 0)
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            var regex = new Regex(@"^(0[0-9]{9})$");
            return regex.IsMatch(phone);
        }

    }

}
