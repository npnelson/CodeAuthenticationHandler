using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NetToolBox.CodeAuthentication;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace CodeAuthenticationHandler.Tests
{
    public sealed class CodeAuthenticationHandlerTests
    {
        [Theory]
        [InlineData("invalidCode", false)]
        [InlineData("validcode1", true)]
        [InlineData("validcode2", true)]
        public async Task ValidateCodesTest(string code, bool succeeded)
        {
            var fixture = new CodeAuthenticationTestFixture(new List<string> { "validcode1", "validcode2" });
            var handler = fixture.Handler;
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString($"?code={code}");
            await handler.InitializeAsync(new AuthenticationScheme("CodeAuthentication", null, typeof(NetToolBox.CodeAuthentication.CodeAuthenticationHandler)), context);
            var result = await handler.AuthenticateAsync();
            result.Succeeded.Should().Be(succeeded);
        }

    }

    public sealed class CodeAuthenticationTestFixture
    {
        private readonly Mock<IOptionsMonitor<CodeAuthenticationSchemeOptions>> _options = new Mock<IOptionsMonitor<CodeAuthenticationSchemeOptions>>();
        private readonly Mock<ILoggerFactory> _loggerFactory = new Mock<ILoggerFactory>();
        private readonly Mock<UrlEncoder> _encoder = new Mock<UrlEncoder>();
        private readonly Mock<ISystemClock> _clock = new Mock<ISystemClock>();

        public NetToolBox.CodeAuthentication.CodeAuthenticationHandler Handler;
        public CodeAuthenticationTestFixture(List<string> validCodes)
        {
            var options = new CodeAuthenticationSchemeOptions { ValidCodes = validCodes };
            _options.Setup(x => x.Get(It.IsAny<string>())).Returns(options);
            var logger = new Mock<ILogger<NetToolBox.CodeAuthentication.CodeAuthenticationHandler>>();
            _loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            Handler = new NetToolBox.CodeAuthentication.CodeAuthenticationHandler(_options.Object, _loggerFactory.Object, _encoder.Object, _clock.Object);
        }
    }
}
