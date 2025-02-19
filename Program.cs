using ApiValidator.generator;
using ApiValidator.model;
using System;
using System.Text.Json;

class Program
{
    public static void Main()
    {
        DocumentGenerator generator = new DocumentGenerator();
        Document document = generator.GenerateDocument("350ff4d2-18cc-4e61-879c-d3b13db76a0d");
        // Serialize to JSON
        string json = JsonSerializer.Serialize(document, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(json);

        // Deserialize from JSON
        Document deserialized = JsonSerializer.Deserialize<Document>(json);
        Console.WriteLine(deserialized.ProviderUserToken);
        Console.WriteLine(deserialized.ProviderMsgId);
        Console.WriteLine(deserialized.ProviderMsgReferenceId);
        Console.WriteLine(deserialized.DocumentType);
        Console.WriteLine(deserialized.Customer.Name);
        Console.WriteLine(deserialized.Customer.Address);
        Console.WriteLine(deserialized.Customer.City);
        Console.WriteLine(deserialized.Customer.Phone);
        Console.WriteLine(deserialized.Customer.Email);
        Console.WriteLine(deserialized.DocDate);
    }
}