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
        private const string providerMsgReferenceIdPrefix = "message-";
        public Document GenerateDocument(string providerUserToken)
        {
            int providerMsgIdNum = random.Next(1000, 100000);
            string providerMsgReferenceId = providerMsgReferenceIdPrefix + providerMsgIdNum;
            DocumentType documentType = DocumentType.Invoice;
            //DocumentType documentType = documentTypes[random.Next(documentTypes.Length)];
            string docDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string dueDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            Customer customer = new Customer("Omer Shalev", "Address comes here", "Anytown", "0500500500", "customer@email.com");
            DocumentItems documentItems = new DocumentItems();
            Document doc= new Document(providerUserToken, providerMsgIdNum.ToString(), providerMsgReferenceId, documentType, customer, dueDate, documentItems, docDate);
            return doc;
        }
    }
}
