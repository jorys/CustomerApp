using FluentAssertions;
using System.Text;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace CustomerApp.Specs.StepDefinitions;

public class BaseSteps
{
    protected readonly HttpClient Client = new() { BaseAddress = new Uri(Settings.BaseAddress) };
    protected readonly ScenarioContext ScenarioContext;

    public BaseSteps(ScenarioContext scenarioContext)
    {
        ScenarioContext = scenarioContext;
    }

    [Given("a registered customer with random email")]
    public async Task GivenRegisterCustomer()
    {
        ScenarioContext[ContextKeys.Email] = GetRandomEmail();
        ScenarioContext[ContextKeys.Password] = "P@sSw0rD!";

        ScenarioContext[ContextKeys.Response] = await Client.PostAsync("api/register",
            new StringContent($@"{{
                  ""firstName"": ""Jorys"",
                  ""lastName"": ""GAILLARD"",
                  ""birthdate"": ""2000-01-01"",
                  ""email"": ""{ScenarioContext[ContextKeys.Email]}"",
                  ""password"": ""{ScenarioContext[ContextKeys.Password]}"",
                  ""address"": {{
                    ""street"": ""1 avenue du lac"",
                    ""city"": ""Annecy"",
                    ""postCode"": 74000,
                    ""country"": ""France""
                  }}
                }}", Encoding.UTF8, "application/json"));

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

    [Then("a successful response is returned")]
    public void ThenSuccessfulResponse()
    {
        var response = (HttpResponseMessage)ScenarioContext[ContextKeys.Response];
        response.IsSuccessStatusCode.Should().BeTrue();
    }
}