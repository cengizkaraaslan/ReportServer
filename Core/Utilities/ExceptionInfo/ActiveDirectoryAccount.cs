using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.ExceptionInfo
{
    public class ActiveDirectoryAccount : Exception
    {
        public ActiveDirectoryAccount(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
