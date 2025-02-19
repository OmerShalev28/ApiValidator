using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public enum VatOption
{
    NotInclude,
    Include,
    Free
}

public enum DiscountValueType
{
    Percentage,
    DirectAmount
}
public class DocumentItems
{
    [JsonPropertyName("paymentItems")]
    public List<PaymentItem> PaymentItems { get; set; }

    [JsonPropertyName("roundTotalSum")]
    public bool RoundTotalSum { get; set; }

    public DocumentItems()
    {
        PaymentItems = new List<PaymentItem>();
        PaymentItems.Add(new PaymentItem());
        RoundTotalSum = true;
    }
}

public class PaymentItem
{
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("pricePerUnit")]
    public decimal PricePerUnit { get; set; }

    [JsonPropertyName("vatOption")]
    public VatOption VatOption { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    public PaymentItem()
    {
        Quantity = 1;
        PricePerUnit = 0;
        VatOption = VatOption.Include;
        Description = "Item description";
    }
}

