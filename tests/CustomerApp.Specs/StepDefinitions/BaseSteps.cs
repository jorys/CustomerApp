using FluentAssertions;
using System.Text;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace CustomerApp.Specs.StepDefinitions;

public class BaseSteps
{
    protected const string DefaultPassword = "P@sSw0rD!";
    protected const string DefaultFirstName = "Jorys";
    protected const string WordRegex = "([!-~]+)";
    protected static readonly HttpClient Client = new() { BaseAddress = new Uri("http://localhost:5000") };

    protected readonly ScenarioContext ScenarioContext;

    public BaseSteps(ScenarioContext scenarioContext)
    {
        ScenarioContext = scenarioContext;
    }

    [Given($"a registered customer {WordRegex}")]
    public Task GivenRegisteredCustomerWithFirstName(string firstName)
    {
        return RegisterCustomer(firstName: firstName);
    }

    [Given($"a registered customer with password {WordRegex}")]
    public Task GivenRegisteredCustomerWithpassword(string password)
    {
        return RegisterCustomer(password: password);
    }

    [Given("a registered customer")]
    public Task GivenRegisteredCustomer()
    {
        return RegisterCustomer();
    }

    async Task RegisterCustomer(string password = DefaultPassword, string firstName = DefaultFirstName)
    {
        ScenarioContext[ContextKeys.Email] = GetRandomEmail();
        ScenarioContext[ContextKeys.Response] = await Client.PostAsync("api/register",
            ToJson($@"{{
                  ""firstName"": ""{firstName}"",
                  ""lastName"": ""GAILLARD"",
                  ""birthdate"": ""2000-01-01"",
                  ""email"": ""{ScenarioContext[ContextKeys.Email]}"",
                  ""password"": ""{password}"",
                  ""address"": {{
                    ""street"": ""1 avenue du lac"",
                    ""city"": ""Annecy"",
                    ""postCode"": 74000,
                    ""country"": ""France""
                  }}
                }}"));

        // Get JWT token
        var response = (HttpResponseMessage)ScenarioContext[ContextKeys.Response];
        var content = await response.Content.ReadAsStringAsync();
        var jsonObject = JsonDocument.Parse(content);

        ScenarioContext[ContextKeys.JwtToken] = jsonObject.RootElement.GetProperty("jwtToken");
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

    [When($"customer logins with password {WordRegex}")]
    public async Task WhenCustomerLogins(string password)
    {
        ScenarioContext[ContextKeys.Response] = await Client.PostAsync("api/login",
            ToJson($@"{{
                  ""email"": ""{ScenarioContext[ContextKeys.Email]}"",
                  ""password"": ""{password}""
                }}"));
    }

    [Then("successful response is returned")]
    public void ThenSuccessfulResponse()
    {
        var response = (HttpResponseMessage)ScenarioContext[ContextKeys.Response];
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Then("fail response is returned")]
    public void ThenFailResponse()
    {
        var response = (HttpResponseMessage)ScenarioContext[ContextKeys.Response];
        response.IsSuccessStatusCode.Should().BeFalse();
    }

    protected static HttpContent ToJson(string content)
    {
        return new StringContent(content, Encoding.UTF8, "application/json");
    }
}