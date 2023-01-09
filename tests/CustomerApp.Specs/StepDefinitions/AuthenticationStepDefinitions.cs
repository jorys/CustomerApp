using CustomerApp.Specs.StepDefinitions.Common;
using FluentAssertions;
using MimeKit.Text;
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

        [When("call forgot password on generated email")]
        public async Task WhenForgotPassword()
        {
            ScenarioContext[ContextKeys.Response] = await Client.PostAsync("api/forgot-password",
                ToJson($@"{{""email"": ""{ScenarioContext[ContextKeys.Email]}""}}"));
        }

        [Then("JWT token is generated")]
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
            var emailAddress = ScenarioContext[ContextKeys.Email].ToString();
            emailAddress.Should().NotBeNullOrEmpty();

            using var client = new TestImapClient();
            var emailFound = await client.GetFirstOrDefaultMessage(emailAddress, maxMessagesToFetch: 3);
            emailFound.Should().NotBeNull();

            var token = emailFound.GetTextBody(TextFormat.Text).Split(' ').Last();
            token.Should().NotBeEmpty();

            ScenarioContext[ContextKeys.ResetPasswordToken] = token;
        }

        [When($"use email token to reset password to {WordRegex}")]
        public async Task WhenUseTokenToResetPassword(string password)
        {
            ScenarioContext[ContextKeys.Response] = await Client.PostAsync("api/reset-password",
                ToJson($@"{{
                  ""email"": ""{ScenarioContext[ContextKeys.Email]}"",
                  ""token"": ""{ScenarioContext[ContextKeys.ResetPasswordToken]}"",
                  ""password"": ""{password}""
                }}"));
        }

        [When($"use invalid token to reset password to {WordRegex}")]
        public async Task WhenResetPasswordWithInvalidToken(string password)
        {
            ScenarioContext[ContextKeys.Response] = await Client.PostAsync("api/reset-password",
                ToJson($@"{{
                  ""email"": ""{ScenarioContext[ContextKeys.Email]}"",
                  ""token"": ""invalid-token"",
                  ""password"": ""{password}""
                }}"));
        }
    }
}
