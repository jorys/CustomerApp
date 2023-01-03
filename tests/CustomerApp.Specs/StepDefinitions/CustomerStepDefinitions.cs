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

        [When("customer info is retrieved")]
        public async Task WhenGetCustomerInfo()
        {
            var jwtToken = ScenarioContext[ContextKeys.JwtToken].ToString();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            ScenarioContext[ContextKeys.Response] = await Client.GetAsync("api/customer");
        }


        [When("customer first name is updated to (.*)")]
        public async Task WhenCustomerFirstNameUpdated(string firstName)
        {
            var jwtToken = ScenarioContext[ContextKeys.JwtToken].ToString();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            ScenarioContext[ContextKeys.Response] = await Client.PatchAsync("api/customer",
                new StringContent($@"{{""firstName"": ""{firstName}""}}", Encoding.UTF8, "application/json"));
        }

        [Then("customer data contains correct email")]
        public async Task ThenCustomerDataContainsEmail()
        {
            var response = (HttpResponseMessage)ScenarioContext[ContextKeys.Response];
            var content = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonDocument.Parse(content);

            jsonObject.RootElement.GetProperty("email").ToString().Should().Be(ScenarioContext[ContextKeys.Email].ToString());
        }

        [Then("customer data contains first name (.*)")]
        public async Task ThenCustomerDataContainsFirstName(string firstName)
        {
            var response = (HttpResponseMessage)ScenarioContext[ContextKeys.Response];
            var content = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonDocument.Parse(content);

            jsonObject.RootElement.GetProperty("firstName").ToString().Should().Be(firstName);
        }
    }
}