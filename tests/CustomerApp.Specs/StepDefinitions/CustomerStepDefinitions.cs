using CustomerApp.Specs.StepDefinitions.Common;
using FluentAssertions;
using System.Net.Http.Headers;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace CustomerApp.Specs.StepDefinitions;

[Binding]
[Scope(Feature = "Customer")]
public sealed class CustomerStepDefinitions : BaseSteps
{
    public CustomerStepDefinitions(ScenarioContext scenarioContext) : base(scenarioContext)
    {
    }

    [When("use JWT token to get info")]
    public async Task WhenGetInfoWithToken()
    {
        var jwtToken = ScenarioContext[ContextKeys.JwtToken].ToString();
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        ScenarioContext[ContextKeys.Response] = await client.GetAsync("api/customer");
    }

    [When("get info without JWT token")]
    public async Task WhenGetInfoWithoutToken()
    {
        ScenarioContext[ContextKeys.Response] = await CreateClient().GetAsync("api/customer");
    }

    [When($"use JWT token to update first name to {WordRegex}")]
    public async Task WhenUpdateFirstNameWithToken(string firstName)
    {
        var jwtToken = ScenarioContext[ContextKeys.JwtToken].ToString();
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        ScenarioContext[ContextKeys.Response] = await client.PatchAsync("api/customer",
            ToJson($@"{{""firstName"": ""{firstName}""}}"));
    }

    [When($"update first name to {WordRegex} without JWT token")]
    public async Task WhenUpdateFirstNameWithoutToken(string firstName)
    {
        ScenarioContext[ContextKeys.Response] = await CreateClient().PatchAsync("api/customer", ToJson($@"{{""firstName"": ""{firstName}""}}"));
    }

    [When($"use JWT token to update password to {WordRegex}")]
    public async Task WhenUpdatePasswordWithToken(string password)
    {
        var jwtToken = ScenarioContext[ContextKeys.JwtToken].ToString();
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        ScenarioContext[ContextKeys.Response] = await client.PutAsync("api/customer/password",
            ToJson($@"{{""password"": ""{password}""}}"));
    }

    [When($"update password to {WordRegex} without JWT token")]
    public async Task WhenUpdatePasswordWithoutToken(string password)
    {
        ScenarioContext[ContextKeys.Response] = await CreateClient().PutAsync("api/customer/password",
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