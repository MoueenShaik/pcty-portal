using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCTYLibrary.Constants
{
    public class AuthSchemeConstants
    {
        public const string CustomAuthScheme = "Bearer";
        public const string CustomToken = $"{CustomAuthScheme} (?<token>.*)";
    }
}
