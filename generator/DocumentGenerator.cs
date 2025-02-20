using ApiValidator.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiValidator.generator
{
    public class DocumentGenerator
    {
        private static Random random = new Random();
        private static DocumentType[] documentTypes = (DocumentType[])Enum.GetValues(typeof(DocumentType));
        private static readonly string[] names = { "John Doe", "Jane Doe", "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Heidi" , ""};
        private static readonly string[] adresses = { "1234 Elm St", "5678 Oak St", "91011 Pine St", "131415 Maple St", "161718 Cedar St", "192021 Birch St", "222324 Spruce St", "252627 Fir St", "282930 Pine St", "313233 Elm St" };
        private static readonly string[] cities = { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia", "San Antonio", "San Diego", "Dallas", "San Jose" };

        private const string providerMsgReferenceIdPrefix = "message-";
        public Document GenerateDocument(string providerUserToken)
        {
            int providerMsgIdNum = random.Next(1000, 100000);
            string providerMsgReferenceId = providerMsgReferenceIdPrefix + providerMsgIdNum;
            DocumentType documentType = documentTypes[random.Next(documentTypes.Length)];

            string docDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string dueDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

            string customerName = names[random.Next(names.Length)];
            string customerAddress = adresses[random.Next(adresses.Length)];
            string customerCity = cities[random.Next(cities.Length)];
            string customerPhone = GetRandomPhoneNumber();
            Customer customer = new Customer(customerName, customerAddress, customerCity, customerPhone);

            DocumentItems documentItems = new DocumentItems();
            Document doc = new Document(providerUserToken, providerMsgIdNum.ToString(), providerMsgReferenceId, documentType, customer, dueDate, documentItems, docDate);
            return doc;
        }

        // Generates a random 10-digit phone number as a string.
        private string GetRandomPhoneNumber()
        {
            char[] digits = new char[10];
            for (int i = 0; i < 10; i++)
            {
                digits[i] = (char)('0' + random.Next(10));
            }
            return new string(digits);
        }
    }
}
