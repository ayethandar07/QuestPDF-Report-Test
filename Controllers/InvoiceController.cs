using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        return Ok();
    }
}
