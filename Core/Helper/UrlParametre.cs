using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Helper
{
    public static class UrlParametre
    {
        public static Dictionary<string, string> ParametreleriGetir(string uri)
        {
            var matches = Regex.Matches(uri.Replace("undefined", ""), @"[\?&](([^&=]+)=([^&=#]*))", RegexOptions.Compiled);
            var keyValues = new Dictionary<string, string>(matches.Count);
            foreach (Match m in matches)
                keyValues.Add(Uri.UnescapeDataString(m.Groups[2].Value), Uri.UnescapeDataString(m.Groups[3].Value));
            return keyValues;
        }
    }
}
