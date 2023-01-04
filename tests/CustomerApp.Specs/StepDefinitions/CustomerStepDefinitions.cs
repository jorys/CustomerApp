using FluentAssertions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace CustomerApp.Specs.StepDefinitions
{
    [Binding]
    [Scope(Feature = "Customer")]
    public sealed class CustomerStepDefinitions : BaseSteps
    {
        public CustomerStepDefinitions(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [When("use JWT token to get info")]
        public async Task WhenGetInfo()
        {
            var jwtToken = ScenarioContext[ContextKeys.JwtToken].ToString();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            ScenarioContext[ContextKeys.Response] = await Client.GetAsync("api/customer");
        }

        [When($"use JWT token to update first name to {WordRegex}")]
        public async Task WhenUpdateFirstName(string firstName)
        {
            var jwtToken = ScenarioContext[ContextKeys.JwtToken].ToString();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            ScenarioContext[ContextKeys.Response] = await Client.PatchAsync("api/customer",
                ToJson($@"{{""firstName"": ""{firstName}""}}"));
        }

        [When($"use JWT token to update password to {WordRegex}")]
        public async Task WhenUpdatePassword(string password)
        {
            var jwtToken = ScenarioContext[ContextKeys.JwtToken].ToString();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            ScenarioContext[ContextKeys.Response] = await Client.PutAsync("api/customer/password",
                ToJson($@"{{""password"": ""{password}""}}"));
        }

        [Then($"customer response contains first name {WordRegex}")]
        public async Task ThenCustomerResponseContainsFirstName(string firstName)
        {
            var response = (HttpResponseMessage)ScenarioContext[ContextKeys.Response];
            var content = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonDocument.Parse(content);

            jsonObject.RootElement.GetProperty("firstName").ToString().Should().Be(firstName);
        }
    }
}