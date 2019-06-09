using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    public class DecryptHelper
    {
        public static string Decrypt(string cipherString)
        {
            byte[] keyArray;
            var toEncryptArray = System.Convert.FromBase64String(cipherString);
            var key = "sayonara";
            var hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Encoding.UTF8.GetString(resultArray);
        }
    }
}