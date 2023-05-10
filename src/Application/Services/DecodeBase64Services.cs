using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public static class DecodeBase64Services
    {
        public static string DecodeBase64(string plainText)
        {
            var text = plainText.Replace("data:text/plain;base64,", String.Empty);
            var valueBytes = System.Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(valueBytes);
        }
    }
}
