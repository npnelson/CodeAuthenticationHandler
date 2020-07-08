using Microsoft.Extensions.Configuration;
using NetToolBox.CodeAuthentication;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeAuthenticationServiceCollectionExtensions
    {
        private const string DefaultCodeAuthenticationName = "CodeAuthentication";
        /// <summary>
        /// Adds CodeAuthenticationHandler (i.e. ?code=(password))
        /// </summary>
        /// <param name="services"></param>
        /// <param name="codeAuthenticationName">The name you want to use for the authentication scheme</param>
        /// <param name="configurationSection">A configuration section containing a list of the valid codes for this scheme</param>
        public static void AddCodeAuthentication(this IServiceCollection services, string codeAuthenticationName, IConfigurationSection configurationSection)
        {
            services.AddAuthentication(codeAuthenticationName).AddScheme<CodeAuthenticationSchemeOptions, CodeAuthenticationHandler>(codeAuthenticationName, options => options.ValidCodes = configurationSection.Get<List<string>>());
        }

        /// <summary>
        /// Adds CodeAuthenticationHandler (i.e. ?code=(password)) with the default scheme name of "CodeAuthentication"
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection">A configuration section containing a list of the valid codes for this scheme</param>
        public static void AddCodeAuthentication(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            services.AddAuthentication(DefaultCodeAuthenticationName).AddScheme<CodeAuthenticationSchemeOptions, CodeAuthenticationHandler>(DefaultCodeAuthenticationName, options => options.ValidCodes = configurationSection.Get<List<string>>());
        }
    }
}
