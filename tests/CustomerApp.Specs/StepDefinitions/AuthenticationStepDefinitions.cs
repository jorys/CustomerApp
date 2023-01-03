using FluentAssertions;
using MailKit;
using MailKit.Net.Imap;
using MimeKit.Text;
using System.Text;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace CustomerApp.Specs.StepDefinitions
{
    [Binding]
    [Scope(Feature = "Authentication")]
    public sealed class AuthenticationStepDefinitions : BaseSteps
    {
        public AuthenticationStepDefinitions(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [When("the customer logins")]
        public async Task WhenCustomerLogins()
        {
            ScenarioContext[ContextKeys.Response] = await Client.PostAsync("api/login",
                new StringContent($@"{{
                  ""email"": ""{ScenarioContext[ContextKeys.Email]}"",
                  ""password"": ""{ScenarioContext[ContextKeys.Password]}""
                }}", Encoding.UTF8, "application/json"));
        }

        [When("the customer calls forgot password feature")]
        public async Task WhenForgotPassword()
        {
            ScenarioContext[ContextKeys.Response] = await Client.PostAsync("api/forgot-password",
                new StringContent($@"{{""email"": ""{ScenarioContext[ContextKeys.Email]}""}}", Encoding.UTF8, "application/json"));
        }

        [Then("a token is generated")]
        public async Task ThenTokenGenerated()
        {
            var response = (HttpResponseMessage)ScenarioContext[ContextKeys.Response];
            var content = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonDocument.Parse(content);
            var token = jsonObject.RootElement.GetProperty("jwtToken");
            token.Should().NotBeNull();
        }

        [Then("an email with reset password token is sent")]
        public async Task ThenEmailWithResetTokenSent()
        {
            using var client = new ImapClient();
            client.Connect("localhost", 143);
            client.Authenticate(userName: "", password: "");

            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            var lastEmail = await inbox.GetMessageAsync(inbox.Count - 1);
            lastEmail.To.ToString().Should().Be(ScenarioContext[ContextKeys.Email].ToString());
            ScenarioContext[ContextKeys.ResetPasswordToken] = lastEmail.GetTextBody(TextFormat.Text).Split(' ').Last();

            client.Disconnect(true);
        }

        [When("customer reset password")]
        public async Task WhenResetPassword()
        {
            var newPassword = "PassW0RD!!";
            ScenarioContext[ContextKeys.Response] = await Client.PostAsync("api/reset-password",
                new StringContent($@"{{
                  ""email"": ""{ScenarioContext[ContextKeys.Email]}"",
                  ""token"": ""{ScenarioContext[ContextKeys.ResetPasswordToken]}"",
                  ""password"": ""{newPassword}""
                }}", Encoding.UTF8, "application/json"));

            ScenarioContext[ContextKeys.Password] = newPassword;
        }
    }
}