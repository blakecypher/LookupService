using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Text.Json;
using backend;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace service.Tests;

[TestSubject(typeof(CreditDataController))]
public class LookupServiceTest
{
    private static readonly IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
    {
        { "credit_data_url", "https://infra.devskills.app/api/credit-data" },
        { "local_lookup_url", "http://localhost:5038/" }
    }).Build();

    private readonly string apiUrl = config["credit_data_url"];
    private readonly string localApiUrl = config["local_lookup_url"];

    [Fact]
    public async Task ProvidesFunctionalHealthcheck()
    {
        using var client = new HttpClient();
        var requestUrl = $"{localApiUrl}ping";
        var response = await client.GetAsync(requestUrl);
        Assert.Equal(200, (int)response.StatusCode);
    }

    [Fact]
    public async Task CanCorrectlyAggregateAndReturnEmmasCreditData()
    {
        using var client = new HttpClient();
        var requestUrl = $"{localApiUrl}credit-data/424-11-9327";
        var response = await client.GetAsync(requestUrl);
        Assert.Equal(200, (int)response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        var creditData = JsonSerializer.Deserialize<CreditData>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal("Emma", creditData.FirstName);
        Assert.Equal("Gautrey", creditData.LastName);
        Assert.Matches(@"\d{1,4} Westend Terrace$", creditData.Address);
        Assert.Equal(60668, creditData.AssessedIncome);
        Assert.Equal(11585, creditData.BalanceOfDebt);
        Assert.True(creditData.Complaints);
    }

    [Fact]
    public async Task CanCorrectlyAggregateAndReturnBillysCreditData()
    {
        using var client = new HttpClient();
        var requestUrl = $"{localApiUrl}credit-data/553-25-8346";
        var response = await client.GetAsync(requestUrl);
        Assert.Equal(200, (int)response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        var creditData = JsonSerializer.Deserialize<CreditData>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal("Billy", creditData.FirstName);
        Assert.Equal("Brinegar", creditData.LastName);
        Assert.Matches(@"\d{1,4} Providence Lane La Puente, CA 91744$", creditData.Address);
        Assert.Equal(89437, creditData.AssessedIncome);
        Assert.Equal(178, creditData.BalanceOfDebt);
        Assert.False(creditData.Complaints);
    }

    [Fact]
    public async Task CanCorrectlyAggregateAndReturnGailsCreditData()
    {
        using var client = new HttpClient();
        var requestUrl = $"{localApiUrl}credit-data/287-54-7823";
        var response = await client.GetAsync(requestUrl);
        Assert.Equal(200, (int)response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        var creditData = JsonSerializer.Deserialize<CreditData>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal("Gail", creditData.FirstName);
        Assert.Equal("Shick", creditData.LastName);
        Assert.Matches(@"\d{1,4} Rainbow Drive Canton, OH 44702$", creditData.Address);
        Assert.Equal(42301, creditData.AssessedIncome);
        Assert.Equal(23087, creditData.BalanceOfDebt);
        Assert.True(creditData.Complaints);
    }

    [Fact]
    public async Task CanHandleRequestsForNonExistentSSNs()
    {
        using var client = new HttpClient();
        var requestUrl = $"{localApiUrl}credit-data/000-00-0000";
        var response = await client.GetAsync(requestUrl);
        Assert.Equal(404, (int)response.StatusCode);
    }

    private class CreditData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int AssessedIncome { get; set; }
        public int BalanceOfDebt { get; set; }
        public bool Complaints { get; set; }
    }
}