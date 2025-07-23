using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace WorkManagement.Helper
{
    public static class JwtKeyHelper
    {
        public static RsaSecurityKey LoadPrivateKey(string path)
        {
            var privateKeyText = File.ReadAllText(path);
            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyText.ToCharArray());
            return new RsaSecurityKey(rsa);
        }

        public static RsaSecurityKey LoadPublicKey(string path)
        {
            var publicKeyText = File.ReadAllText(path);
            var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyText.ToCharArray());
            return new RsaSecurityKey(rsa);
        }
    }
}
