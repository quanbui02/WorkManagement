using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

namespace WorkManagement.Common
{
    public class MyBase
    {
        public static List<string> ConvertStringToListString(string ltsSource, string pattern = ";")
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(ltsSource))
            {
                return list;
            }

            if (ltsSource.Contains(pattern))
            {
                list = Regex.Split(ltsSource, pattern).ToList();
            }
            else
            {
                list.Add(ltsSource);
            }

            return list;
        }

        public static List<int> ConvertStringToListInt(string ltsSource, string pattern = ";")
        {
            if (!string.IsNullOrEmpty(ltsSource))
            {
                return (from x in ltsSource.Split(pattern)
                        select int.Parse(x)).ToList();
            }

            return new List<int>();
        }

        public static List<int?> ConvertStringToListIntNull(string ltsSource, string pattern = ";")
        {
            if (!string.IsNullOrEmpty(ltsSource))
            {
                return ((IEnumerable<string>)ltsSource.Split(pattern)).Select((Func<string, int?>)((string x) => int.Parse(x))).ToList();
            }

            return new List<int?>();
        }

        public static string Encrypt(string toEncrypt, bool useHashing, string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
            byte[] key2;
            if (useHashing)
            {
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                key2 = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(key));
                mD5CryptoServiceProvider.Clear();
            }
            else
            {
                key2 = Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider obj = new TripleDESCryptoServiceProvider
            {
                Key = key2,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            byte[] array = obj.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            obj.Clear();
            return Convert.ToBase64String(array, 0, array.Length).Replace("/", "-").Replace("+", "_")
                .Replace("=", "");
        }

        public static string Decrypt(string cipherString, bool useHashing, string key)
        {
            byte[] array = Convert.FromBase64String(cipherString.Replace("-", "/").Replace("_", "+") + "=");
            byte[] key2;
            if (useHashing)
            {
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                key2 = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(key));
                mD5CryptoServiceProvider.Clear();
            }
            else
            {
                key2 = Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider obj = new TripleDESCryptoServiceProvider
            {
                Key = key2,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            byte[] bytes = obj.CreateDecryptor().TransformFinalBlock(array, 0, array.Length);
            obj.Clear();
            return Encoding.UTF8.GetString(bytes);
        }

        public static string ToMD5(string str)
        {
            Encoder encoder = Encoding.Unicode.GetEncoder();
            byte[] array = new byte[str.Length * 2];
            encoder.GetBytes(str.ToCharArray(), 0, str.Length, array, 0, flush: true);
            byte[] array2 = new MD5CryptoServiceProvider().ComputeHash(array);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array3 = array2;
            foreach (byte b in array3)
            {
                stringBuilder.Append(b.ToString("X2"));
            }

            return stringBuilder.ToString();
        }

        public static string ConvertObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });
        }
    }
}
