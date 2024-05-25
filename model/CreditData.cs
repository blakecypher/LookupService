using System.Text.Json.Serialization;

namespace model;

public class CreditData
{
    [JsonIgnore]
    public string? Ssn { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public int? AssessedIncome { get; set; }
    public int? BalanceOfDebt { get; set; }
    public bool? Complaints { get; set; }
}