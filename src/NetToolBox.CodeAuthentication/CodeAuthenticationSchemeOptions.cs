using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;

namespace NetToolBox.CodeAuthentication
{
    public sealed class CodeAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public List<string> ValidCodes { get; set; } = null!;
    }
}
