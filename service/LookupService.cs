using System.Text.Json;
using backend.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using model;

namespace service;

public class LookupService(
    IConfiguration config,
    IMemoryCache resultsCache, 
    HttpClient httpClient) : ILookupService
{
    private readonly string? baseUrl = config["credit_data_results_url"];
    private readonly GenericCache<CreditData?> resultsCache = new(resultsCache, config);
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = new CamelCaseToSnakeCaseNamingPolicy()
    };

    public async Task<CreditData?> GetOrFetchCreditData(string ssn)
    {
        return await resultsCache.GetOrCreate("CreditData-"+ ssn, () => GetCreditData(ssn));
    }

    public async Task<CreditData?> GetCreditData(string ssn)
    {
        var creditData = new CreditData();
        
        var personalDetails = await GetPersonalDetails(ssn);
        creditData.FirstName = personalDetails?.FirstName;
        creditData.LastName = personalDetails?.LastName;
        creditData.Address = personalDetails?.Address;
        
        var assessedIncome = await GetAssessedIncome(ssn);
        creditData.AssessedIncome = assessedIncome?.AssessedIncome;
        
        var debtDetails = await GetDebtDetails(ssn);
        creditData.BalanceOfDebt = debtDetails?.BalanceOfDebt;
        creditData.Complaints = debtDetails?.Complaints;
        
        return creditData;
    }

    private async Task<AssessedIncomeDetails?> GetAssessedIncome(string ssn)
    {
        var url = $"{baseUrl}/assessed-income/{ssn}";

        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AssessedIncomeDetails>(content, serializerOptions);
        }

        return new AssessedIncomeDetails();
    }

    private async Task<DebtDetails?> GetDebtDetails(string ssn)
    {
        var url = $"{baseUrl}/debt/{ssn}";

        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DebtDetails>(content, serializerOptions);
        }

        return new DebtDetails();
    }

    private async Task<PersonalDetails?> GetPersonalDetails(string ssn)
    {
        var url = $"{baseUrl}/personal-details/{ssn}";

        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PersonalDetails>(content, serializerOptions);
        }

        return new PersonalDetails();
    }
}

public interface ILookupService
{
    Task<CreditData?> GetOrFetchCreditData(string ssn);
    Task<CreditData?> GetCreditData(string ssn);
}