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
        private long providerMsgIdNum = 1;
        private const string providerMsgReferenceIdPrefix = "message-";
        public Document GenerateDocument(string providerUserToken)
        { 
            string providerMsgReferenceId = providerMsgReferenceIdPrefix + providerMsgIdNum;
            DocumentType documentType = documentTypes[random.Next(documentTypes.Length)];
            string docDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            Customer customer = new Customer("customer name", "Address comes here", "Anytown", "0500500500", "customer@email.com");
            return new Document(providerUserToken, providerMsgIdNum++.ToString(), providerMsgReferenceId, documentType, customer, docDate);
        }
    }
}
