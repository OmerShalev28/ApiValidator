using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public string ProviderUserToken { get; set; }
        public string ProviderMsgId { get; set; }
        public string ProviderMsgReferenceId { get; set; }
        public DocumentType DocumentType { get; set; }
        public Customer Customer { get; set; }
        public string DocDate { get; set; }

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
