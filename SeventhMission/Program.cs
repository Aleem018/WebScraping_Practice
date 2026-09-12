using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

HttpClient client = new HttpClient();
string json = await client.GetStringAsync("https://api.example.com/data");
// 2. Fetch and Deserialize

// This one line replaces all your XPath logic
ApiResponse? data = JsonSerializer.Deserialize<ApiResponse>(json);

// 1. Create classes that mirror the JSON structure exactly
public class ApiResponse 
{
    [JsonPropertyName("results")]
    public List<CompanyData> Results { get; set; }
}

public class CompanyData 
{
    [JsonPropertyName("company")]
    public string Company { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}



// data.Results is now a fully populated List<CompanyData> ready to be exported