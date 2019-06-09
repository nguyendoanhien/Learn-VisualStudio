using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    public class EncryptHelper
    {
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray;
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
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
            var cTransform = tdes.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return System.Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        //tạo mã kích hoạt bằng cách sử dụng hàm random sau đó mã hóa chuỗi random này đi
        public static string RandomCode(int numberCode)
        {
            var activeCode = "";
            var rd = new Random((int) DateTime.Now.Ticks);
            ;
            var key = "0123456789";
            var sb = new StringBuilder();
            for (var i = 0; i < numberCode; i++) sb.Append(key.Substring(rd.Next(0, key.Length - 1), 1));
            activeCode = sb.ToString();
            return activeCode;
        }
    }
}