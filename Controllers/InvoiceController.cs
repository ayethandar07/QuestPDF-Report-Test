using Bogus;
using Microsoft.AspNetCore.Mvc;
using QuestPdfReportTest.Models;
using QuestPdfReportTest.Services;

namespace QuestPdfReportTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly InvoiceRenderingService _invoiceRenderingService;

    public InvoiceController(InvoiceRenderingService invoiceRenderingService)
    {
        _invoiceRenderingService = invoiceRenderingService;
    }

    [HttpGet]
    public ActionResult GeneratePdf()
    {
        var invoice = new Faker<Invoice>()
            .RuleFor(i => i.InvoiceDate, f => f.Date.Recent(30))
            .RuleFor(i => i.InvoiceNumber, f => f.Random.Number(10000, 99999).ToString())
            .Generate();
        invoice.Client = new Faker<Client>()
            .RuleFor(i => i.ClientName, f => f.Company.ToString())
            .RuleFor(i => i.ClientAddress, f => f.Address.FullAddress())
            .Generate();
        invoice.InvoiceItems = [];
        for (int i= 0; i < 15; i++)
        {
            invoice.InvoiceItems.Add(new Faker<InvoiceItem>()
                .RuleFor(i => i.Description, f => f.Commerce.ProductName())
                .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(i => i.UnitPrice, f => decimal.Parse(f.Commerce.Price()))
                .Generate());
        }

        var document = _invoiceRenderingService.GenerateInvoicePdf(invoice);
        return File(document, "application/pdf", "invoice.pdf");
    }
}
