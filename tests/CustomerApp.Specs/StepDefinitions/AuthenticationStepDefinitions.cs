using FluentAssertions;
using MailKit;
using MailKit.Net.Imap;
using MimeKit.Text;
using System.Text;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace CustomerApp.Specs.StepDefinitions
{
    internal static class ContextKeys
    {
        internal const string Email = "email";
        internal const string Response = "response";
        internal const string Password = "password";
        internal const string ResetPasswordToken = "resetPasswordToken";
    }

    [Binding]
    public sealed class AuthenticationStepDefinitions
    {
        readonly ScenarioContext _scenarioContext;
        readonly HttpClient _client = new() { BaseAddress = new Uri(Settings.BaseAddress) };

        public AuthenticationStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("a registered customer")]
        public async Task GivenRegisterCustomer()
        {
            _scenarioContext[ContextKeys.Email] = GetRandomEmail();
            _scenarioContext[ContextKeys.Password] = "P@sSw0rD!";

            _scenarioContext[ContextKeys.Response] = await _client.PostAsync("api/register", 
                new StringContent($@"{{
                  ""firstName"": ""Spec"",
                  ""lastName"": ""FLOW"",
                  ""birthdate"": ""2000-01-01"",
                  ""email"": ""{_scenarioContext[ContextKeys.Email]}"",
                  ""password"": ""{_scenarioContext[ContextKeys.Password]}"",
                  ""address"": {{
                    ""street"": ""1 avenue du lac"",
                    ""city"": ""Annecy"",
                    ""postCode"": 74000,
                    ""country"": ""France""
                  }}
                }}", Encoding.UTF8, "application/json"));
        }

        static string GetRandomEmail()
        {
            var random = new Random();
            string randomString = "";
            Enumerable
                .Range(1, 30)
                .ToList()
                .ForEach(_ => randomString += Convert.ToChar(random.Next(0, 26) + 97));

            return $"{randomString}@gmail.com";
        }

        [When("the customer logins")]
        public async Task WhenCustomerLogins()
        {
            _scenarioContext[ContextKeys.Response] = await _client.PostAsync("api/login",
                new StringContent($@"{{
                  ""email"": ""{_scenarioContext[ContextKeys.Email]}"",
                  ""password"": ""{_scenarioContext[ContextKeys.Password]}""
                }}", Encoding.UTF8, "application/json"));
        }

        [When("the customer calls forgot password feature")]
        public async Task WhenForgotPassword()
        {
            _scenarioContext[ContextKeys.Response] = await _client.PostAsync("api/forgot-password",
                new StringContent($@"{{""email"": ""{_scenarioContext[ContextKeys.Email]}""}}", Encoding.UTF8, "application/json"));
        }

        [Then("a successful response is returned")]
        public void ThenSuccessfulResponse()
        {
            var response = (HttpResponseMessage)_scenarioContext[ContextKeys.Response];
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Then("a token is generated")]
        public async Task ThenTokenGenerated()
        {
            var response = (HttpResponseMessage)_scenarioContext[ContextKeys.Response];
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
            lastEmail.To.ToString().Should().Be(_scenarioContext[ContextKeys.Email].ToString());
            _scenarioContext[ContextKeys.ResetPasswordToken] = lastEmail.GetTextBody(TextFormat.Text).Split(' ').Last();

            client.Disconnect(true);
        }

        [When("customer reset password")]
        public async Task WhenResetPassword()
        {
            var newPassword = "PassW0RD!!";
            _scenarioContext[ContextKeys.Response] = await _client.PostAsync("api/reset-password",
                new StringContent($@"{{
                  ""email"": ""{_scenarioContext[ContextKeys.Email]}"",
                  ""token"": ""{_scenarioContext[ContextKeys.ResetPasswordToken]}"",
                  ""password"": ""{newPassword}""
                }}", Encoding.UTF8, "application/json"));

            _scenarioContext[ContextKeys.Password] = newPassword;
        }
    }
}