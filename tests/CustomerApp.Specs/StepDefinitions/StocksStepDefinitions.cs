using CustomerApp.Specs.StepDefinitions.Common;
using FluentAssertions;
using System.Net.Http.Headers;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace CustomerApp.Specs.StepDefinitions;

[Binding]
[Scope(Feature = "Stocks")]
public sealed class StocksStepDefinitions : BaseSteps
{
    public StocksStepDefinitions(ScenarioContext scenarioContext) : base(scenarioContext)
    {
    }

    [When($"use JWT token to add (\\d+) {WordRegex}")]
    public async Task WhenAddItemsWithToken(int quantity, string itemName)
    {
        var jwtToken = ScenarioContext[ContextKeys.JwtToken].ToString();
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        ScenarioContext[ContextKeys.Response] = await client.PostAsync("api/stocks/add-items",
            ToJson($@"{{""itemName"": ""{itemName}"", ""quantity"": {quantity}}}"));
    }

    [When($"add (\\d+) {WordRegex} without JWT token")]
    public async Task WhenAddItemsWithoutToken(int quantity, string itemName)
    {
        ScenarioContext[ContextKeys.Response] = await CreateClient().PostAsync("api/stocks/add-items",
            ToJson($@"{{""itemName"": ""{itemName}"", ""quantity"": {quantity}}}"));
    }

    [When($"use JWT token to remove (\\d+) {WordRegex}")]
    public async Task WhenRemoveItemsWithToken(int quantity, string itemName)
    {
        var jwtToken = ScenarioContext[ContextKeys.JwtToken].ToString();
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        ScenarioContext[ContextKeys.Response] = await client.PostAsync("api/stocks/remove-items",
            ToJson($@"{{""itemName"": ""{itemName}"", ""quantity"": {quantity}}}"));
    }

    [When($"remove (\\d+) {WordRegex} without JWT token")]
    public async Task WhenRemoveItemsWithoutToken(int quantity, string itemName)
    {
        ScenarioContext[ContextKeys.Response] = await CreateClient().PostAsync("api/stocks/remove-items",
            ToJson($@"{{""itemName"": ""{itemName}"", ""quantity"": {quantity}}}"));
    }

    [When($"use JWT token to get stocks")]
    public async Task WhenGetStocksWithToken()
    {
        var jwtToken = ScenarioContext[ContextKeys.JwtToken].ToString();
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        ScenarioContext[ContextKeys.Response] = await client.GetAsync("api/stocks");
    }

    [When($"get stocks without JWT token")]
    public async Task WhenGetStocksWithoutToken()
    {
        ScenarioContext[ContextKeys.Response] = await CreateClient().GetAsync("api/stocks");
    }

    [Then($"stocks response contains (\\d+) {WordRegex}")]
    public async Task ThenCustomerResponseContainsFirstName(int quantity, string itemName)
    {
        var response = (HttpResponseMessage)ScenarioContext[ContextKeys.Response];
        var content = await response.Content.ReadAsStringAsync();
        var jsonObject = JsonDocument.Parse(content);

        var firstStock = jsonObject.RootElement.GetProperty("stocks")[0];
        firstStock.GetProperty("itemName").ToString().Should().Be(itemName);
        firstStock.GetProperty("quantity").ToString().Should().Be(quantity.ToString());
    }
}
