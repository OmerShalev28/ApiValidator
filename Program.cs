using ApiValidator.generator;
using ApiValidator.model;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly string _baseUrl = "https://test.smartbee.co.il/api/v1/";
    private static readonly string bearerToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjYxY2Q4Nzc3OTQ3OWVmODQ1YjNmZGJj"+
        "YyIsIm5iZiI6MTczOTk2MzIyNCwiZXhwIjoxNzQwNTY4MDI0LCJpYXQiOjE3Mzk5NjMyMjR9.HA8PAjAmj5oVQoPT0hDo-lG2xIzDQDevmDZ3X9p1qs4";
    public static async Task Main()
    {
        DocumentGenerator generator = new DocumentGenerator();
        Document document = generator.GenerateDocument("350ff4d2-18cc-4e61-879c-d3b13db76a0d");
        string json = JsonSerializer.Serialize(document, new JsonSerializerOptions { WriteIndented = true });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "documents/create", content);
        string responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent);
    }
}