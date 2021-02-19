using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Helper
{
    static public class StringParse
    {

        static public string UpperCaseFirstCharacter(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return string.Format(
                    "{0}{1}",
                    text.Substring(0, 1).ToUpper(),
                    text.Substring(1));
            }

            return text;
        }
    }
}
