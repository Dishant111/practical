using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CustoomerToken.Services.Authentication
{
    public class EmployeeAuthenticationSchemaOption : AuthenticationSchemeOptions
    {
    }

    public class EmployeeAuthenticationHandler : AuthenticationHandler<EmployeeAuthenticationSchemaOption>
    {
        public EmployeeAuthenticationHandler(
            IOptionsMonitor<EmployeeAuthenticationSchemaOption> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(
                options,
                logger,
                encoder,
                clock)
        {

        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Logger.LogInformation("Authentication Started");

            var principle = new ClaimsPrincipal();
            
            principle.AddIdentity(new ClaimsIdentity(new List<Claim>() {
                new Claim("EmployeeTypeClaim","EmployeeTypeClaim"),
                new Claim(ClaimTypes.Role,"emp")
            }));

            var ticket = new AuthenticationTicket(principle, "Employee");
            return AuthenticateResult.Fail("Uable to find user");

            return AuthenticateResult.Success(ticket);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Redirect("/Employee/login");

            return Task.CompletedTask;
        }
    }
}
