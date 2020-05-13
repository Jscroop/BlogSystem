using System;
using System.Security.Cryptography;
using System.Text;

namespace BlogSystem.Common.Helpers
{
    public static class Md5Helper
    {
        public static string Md5Encrypt(string password)
        {
            //判断非空
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                return string.Empty;
            }

            var pwd = String.Empty;
            using (MD5 md5 = MD5.Create())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(password);
                byte[] newBuffer = md5.ComputeHash(buffer);
                foreach (var item in newBuffer)
                {
                    pwd = string.Concat(pwd, item.ToString("X2"));
                }
            }
            return pwd;
        }
    }
}
