using System;
using System.Security.Cryptography;
using System.Text;

namespace TechwuliArsenal.GoogleMapsWebServices.Models
{
    public struct GoogleSignedUrl
    {
        public static string Sign(string url, string keyString)
        {
            var encoding = new ASCIIEncoding();
            var usablePrivateKey = keyString.Replace("-", "+").Replace("_", "/");
            var privateKeyBytes = Convert.FromBase64String(usablePrivateKey);
            var uri = new Uri(url);
            var encodedPathAndQueryBytes = encoding.GetBytes(uri.LocalPath + uri.Query);

            var algorithm = new HMACSHA1(privateKeyBytes);
            var hash = algorithm.ComputeHash(encodedPathAndQueryBytes);

            var signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");
            return uri.Scheme + "://" + uri.Host + uri.LocalPath + uri.Query + "&signature=" + signature;
        }
    }
}
