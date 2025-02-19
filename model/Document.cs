using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace ApiValidator.model
{
    public enum DocumentType
    {
        InvoiceReceipt,
        Receipt, 
        ReceiptRefund,
        Invoice, 
        RefundInvoice, 
        DealInvoice, 
        PriceProposal, 
        OrderConfirmation, 
        ShippingCertificate, 
        DonationReceipt, 
        ReturnCertificate
    }
    public class Document
    {
        [JsonPropertyName("providerUserToken")]
        public string ProviderUserToken { get; set; }

        [JsonPropertyName("providerMsgId")]
        public string ProviderMsgId { get; set; }

        [JsonPropertyName("providerMsgReferenceId")]
        public string ProviderMsgReferenceId { get; set; }

        [JsonPropertyName("docType")]
        public DocumentType DocumentType { get; set; }

        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        [JsonPropertyName("docDate")]
        public string DocDate { get; set; }

        [JsonConstructor]
        public Document(string providerUserToken, string providerMsgId, string providerMsgReferenceId,
                        DocumentType documentType, Customer customer, string docDate)
        {
            ProviderUserToken = providerUserToken;
            ProviderMsgId = providerMsgId;
            ProviderMsgReferenceId = providerMsgReferenceId;
            DocumentType = documentType;
            Customer = customer;
            DocDate = docDate;
        }
    }
}
