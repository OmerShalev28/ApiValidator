using ApiValidator.generator;
using ApiValidator.model;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly string _baseUrl = "https://test.smartbee.co.il/api/v1/";
    private static readonly string bearerToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjYxY2Q4Nzc3OTQ3OWVmODQ1YjNmZGJj" +
        "YyIsIm5iZiI6MTczOTk2MzIyNCwiZXhwIjoxNzQwNTY4MDI0LCJpYXQiOjE3Mzk5NjMyMjR9.HA8PAjAmj5oVQoPT0hDo-lG2xIzDQDevmDZ3X9p1qs4";
    public static async Task Main()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        string responseContent = await CreateDocumentApiCall();
        string requestId = GetDocumentResult(responseContent);
        string documentId = await DocumentCreationStatusPolling(requestId);
        string documentsListResponseContent = await FetchDocumentsListApiCall();

        bool flag = ValidateDocumentExists(documentId, documentsListResponseContent);

    }

    public static async Task<string> CreateDocumentApiCall()
    {
        DocumentGenerator generator = new DocumentGenerator();
        Document document = generator.GenerateDocument("350ff4d2-18cc-4e61-879c-d3b13db76a0d");

        string json = JsonSerializer.Serialize(document, new JsonSerializerOptions { WriteIndented = true });
        var content = new StringContent(json, Encoding.UTF8, "application/json");


        HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "documents/create", content);
        string responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent);
        return responseContent;
    }

    public static string GetDocumentResult(string createResponseContent)
    {
        string requestId = "";
        using (JsonDocument doc = JsonDocument.Parse(createResponseContent))
        {
            JsonElement root = doc.RootElement;
            requestId = root.GetProperty("result").GetString();
            Console.WriteLine(requestId);
        }
        return requestId;
    }

    public static async Task<string> DocumentCreationStatusPolling(string requestId)
    {
        string responseContent="";
        for (int i = 0; i < 5; i++)
        {
            Thread.Sleep(1000);
            int resultCodeId;
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "documents/" + requestId);
            responseContent = await response.Content.ReadAsStringAsync();
            using (JsonDocument doc = JsonDocument.Parse(responseContent))
            {
                JsonElement root = doc.RootElement;
                resultCodeId = root.GetProperty("resultCodeId").GetInt32();
                if (resultCodeId !=1 && resultCodeId!=102)
                    throw new Exception("Document creation failed");
                else if(resultCodeId == 102)
                {
                    break;
                }
            }
            Console.WriteLine(responseContent);
        }
        Console.WriteLine(responseContent);
        using (JsonDocument doc = JsonDocument.Parse(responseContent))
        {
            JsonElement root = doc.RootElement;
            JsonElement result = root.GetProperty("result");
            string documentId = result.GetProperty("documentId").GetString();
            return documentId;
        }
    }

    public static async Task<string> FetchDocumentsListApiCall()
    {
        var data = new { providerUserToken = "350ff4d2-18cc-4e61-879c-d3b13db76a0d" };
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await _httpClient.PostAsync(_baseUrl + "documents/search", content);
        string responseContent = await responseMessage.Content.ReadAsStringAsync();
        return responseContent;
    }

    public static bool ValidateDocumentExists(string documentId, string jsonResponse)
    {
        using JsonDocument doc = JsonDocument.Parse(jsonResponse);
        JsonElement root = doc.RootElement;

        if (root.TryGetProperty("result", out JsonElement result) && result.TryGetProperty("items", out JsonElement itemsArray))
        {
            List<string> ids = new List<string>();

            foreach (JsonElement item in itemsArray.EnumerateArray())
            {
                if (item.TryGetProperty("id", out JsonElement idElement))
                {
                    ids.Add(idElement.GetString());
                }
            }

            Console.WriteLine("Extracted IDs:");
            foreach (string id in ids)
            {
                Console.WriteLine(id);
            }

            if(ids.Contains(documentId))
            {
                Console.WriteLine("Document exists in the list");
                return true;
            }
            else
            {
                Console.WriteLine("Document does not exist in the list");
                return false;
            }
        }

        return false;

    }
}