using ApiValidator.generator;
using ApiValidator.model;
using log4net;
using log4net.Config;
using System;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly string _baseUrl = "https://test.smartbee.co.il/api/v1/";
    private static readonly string providerUserToken = "350ff4d2-18cc-4e61-879c-d3b13db76a0d";
    private static readonly string bearerToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjYxY2Q4Nzc3OTQ3OWVmODQ1YjNmZGJj" +
        "YyIsIm5iZiI6MTczOTk2MzIyNCwiZXhwIjoxNzQwNTY4MDI0LCJpYXQiOjE3Mzk5NjMyMjR9.HA8PAjAmj5oVQoPT0hDo-lG2xIzDQDevmDZ3X9p1qs4";
    private static readonly ILog logger = LogManager.GetLogger(typeof(Program));
    private static readonly Dictionary<int, string> errorMessages =
        new Dictionary<int, string>
        {
            {95,"Request with the same providerMsgId already in queue." },
            {96,"" },
            {97, "providerMsgId is invalid." },
            {98,"Invalid UserId provided" },
            {99, "General Error"}
        };

    public static async Task Main()
    {
        XmlConfigurator.Configure(new FileInfo("log4net.config"));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        try
        {

            string responseContent = await CreateDocumentApiCall();

            int resultCodeId = GetResultCodeIdFromResp(responseContent);

            if (resultCodeId == 101)
            {
                string requestId = GetDocumentResult(responseContent);

                string documentId = await DocumentCreationStatusPolling(requestId);

                string documentsListResponseContent = await FetchDocumentsListApiCall();

                ValidateDocumentExists(documentId, documentsListResponseContent);
            }
            else
            {
                logger.Error("Document creation request failed with resultCodeId: " + resultCodeId + " " + errorMessages[resultCodeId]);
                return;
            }
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
            return;
        }

    }

    public static async Task<string> CreateDocumentApiCall()
    {
        logger.Info("Generating document");
        DocumentGenerator generator = new DocumentGenerator();
        Document document = generator.GenerateDocument(providerUserToken);
        string json = JsonSerializer.Serialize(document, new JsonSerializerOptions { WriteIndented = true });
        logger.Info("Generated document: " + json);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        logger.Info("Sending document creation request");
        HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "documents/create", content);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Document creation request failed with status: " + response.StatusCode);
            }

            else
            {
                using (JsonDocument doc = JsonDocument.Parse(responseContent))
                {
                    JsonElement root = doc.RootElement;
                    if (!root.TryGetProperty("errors", out JsonElement errors))
                    {
                        throw new Exception("Document creation request ended with status: " + response.StatusCode);
                    }
                    else
                    {
                        throw new Exception("Document creation request ended with status: " + response.StatusCode + errors.ToString());
                    }
                }
            }

        }

        logger.Info("Received response: " + GetFormattedRespContent(responseContent));
        return responseContent;
    }

    public static string GetFormattedRespContent(string respContent)
    {
        string responseFormatted = "";
        using (JsonDocument doc = JsonDocument.Parse(respContent))
        {
            JsonElement root = doc.RootElement;
            responseFormatted = JsonSerializer.Serialize(root, new JsonSerializerOptions { WriteIndented = true });
        }
        return responseFormatted;
    }

    public static int GetResultCodeIdFromResp(string responseContent)
    {
        int resultCodeId;

        using (JsonDocument doc = JsonDocument.Parse(responseContent))
        {
            JsonElement root = doc.RootElement;
            resultCodeId = root.GetProperty("resultCodeId").GetInt32();
        }

        return resultCodeId;
    }

    public static string GetDocumentResult(string createResponseContent)
    {
        string requestId = "";

        using (JsonDocument doc = JsonDocument.Parse(createResponseContent))
        {
            JsonElement root = doc.RootElement;
            requestId = root.GetProperty("result").GetString();
        }

        return requestId;
    }

    public static async Task<string> DocumentCreationStatusPolling(string requestId)
    {
        string responseContent = "";
        logger.Info("Polling document creation status");
        for (int i = 0; i < 5; i++)
        {
            logger.Info("Polling attempt: " + (i + 1));
            Thread.Sleep(1000);
            int resultCodeId;
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "documents/" + requestId);
            responseContent = await response.Content.ReadAsStringAsync();
            resultCodeId = GetResultCodeIdFromResp(responseContent);
            if (resultCodeId != 1 && resultCodeId != 102)
            {
                throw new Exception("Document creation failed due to: " + errorMessages[resultCodeId] + GetValidationErrorsFromResp(responseContent));
            }
            else if (resultCodeId == 102)
            {
                break;
            }
        }
        logger.Info("Document creation sucsseded: " + GetFormattedRespContent(responseContent));
        return GetDocumentIdFromResp(responseContent);
    }

    public static string GetValidationErrorsFromResp(string responseContent)
    {
        string validationErrors = "";
        using (JsonDocument doc = JsonDocument.Parse(responseContent))
        {
            JsonElement root = doc.RootElement;
            return root.GetProperty("validationErrors").ToString();
        }
    }

    public static string GetDocumentIdFromResp(string responseContent)
    {
        string documentId;
        using (JsonDocument doc = JsonDocument.Parse(responseContent))
        {
            JsonElement root = doc.RootElement;
            documentId = root.GetProperty("result").GetProperty("documentId").GetString();
        }
        return documentId;
    }

    public static async Task<string> FetchDocumentsListApiCall()
    {
        var data = new { providerUserToken };
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        logger.Info("Sending documents list fetch request: " + json);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await _httpClient.PostAsync(_baseUrl + "documents/search", content);
        string responseContent = await responseMessage.Content.ReadAsStringAsync();
        logger.Info("Fetched list of all documents." );
        return responseContent;
    }

    public static void ValidateDocumentExists(string documentId, string jsonResponse)
    {
        JsonDocument doc = JsonDocument.Parse(jsonResponse);
        JsonElement root = doc.RootElement;


        JsonElement itemsArray = root.GetProperty("result").GetProperty("items");
        int totalItemCount = root.GetProperty("result").GetProperty("totalItemCount").GetInt32();
        logger.Info("Total number of documents fetched: " + totalItemCount);

        foreach (JsonElement item in itemsArray.EnumerateArray())
        {
            if (item.GetProperty("id").GetString() == documentId)
            {
                logger.Info("Document with id: " + documentId + " found in the list");
                logger.Info("Document details: " + GetFormattedRespContent(item.ToString()));
                return;
            }
        }
        logger.Warn("Document with id: " + documentId + " not found in the list");
    }

}