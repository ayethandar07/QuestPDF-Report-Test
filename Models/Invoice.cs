namespace QuestPdfReportTest.Models;

public class Invoice
{
    public string? InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public Client? Client { get; set; }
    public List<InvoiceItem>? InvoiceItems { get; set; }
}
