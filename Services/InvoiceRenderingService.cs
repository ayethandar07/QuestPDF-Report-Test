using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPdfReportTest.Models;

namespace QuestPdfReportTest.Services;

public class InvoiceRenderingService
{
    public InvoiceRenderingService()
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
    }

    public byte[] GenerateInvoicePdf(Invoice invoice)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                page.PageColor(Colors.White);

                page.Header()
                .Row(row =>
                {
                    row.RelativeItem()
                    .Column(column =>
                    {
                        column.Item()
                            .Text("Company NAME")
                            .FontFamily("Arial")
                            .FontSize(20)
                            .Bold();
                        column.Item()
                            .Text("Company Address")
                            .FontFamily("Arial");
                    });

                    row.RelativeItem()
                    .ShowOnce()
                    .Text("INVOICE")
                    .AlignRight()
                    .FontFamily("Arial")
                    .ExtraBlack()
                    .FontSize(30);
                });

                page.Content()
                .PaddingTop(50)
                .Column(column =>
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(column2 =>
                        {
                            column2.Item()
                            .Text("Bill To:")
                            .Bold();

                            column2.Item()
                            .Text(invoice.Client?.ClientName)
                            .FontFamily("Arial")
                            .FontSize(15)
                            .Bold();
                        });

                        row.RelativeItem().Column(column2 =>
                        {
                            column2.Item()
                            .Text($"Invoice #: {invoice.InvoiceNumber}")
                            .AlignRight()
                            .Bold();

                            column2.Item()
                            .Text($"Date: {invoice.InvoiceDate:dd-MM-yyyy}")
                            .AlignRight();
                        });
                    });

                    column.Item().PaddingTop(50).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40); // No
                            columns.RelativeColumn(); // Description
                            columns.ConstantColumn(50); // Quantity
                            columns.ConstantColumn(60); // Unit Price
                            columns.ConstantColumn(70); // Total
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("#");
                            header.Cell().Text("Description");
                            header.Cell().Text("Qty");
                            header.Cell().Text("Price");
                            header.Cell().Text("Total");

                            header.Cell()
                            .ColumnSpan(5)
                            .PaddingVertical(5)
                            .BorderBottom(1)
                            .BorderColor(Colors.Black);
                        });

                        if (invoice.InvoiceItems != null)
                        {
                            for (var i = 0; i < invoice.InvoiceItems.Count; i++)
                            {
                                var backgroundColor = i % 2 == 0 ?
                                Color.FromHex("#ffffff") :
                                Color.FromHex("#f0f0f0");

                                var invoiceItem = invoice.InvoiceItems[i];
                                table.Cell().Background(backgroundColor).Padding(4).Text($"{i + 1}");
                                table.Cell().Background(backgroundColor).Padding(4).Text($"{invoiceItem.Description}");
                                table.Cell().Background(backgroundColor).Padding(4).Text($"{invoiceItem.Quantity}");
                                table.Cell().Background(backgroundColor).Padding(4).Text($"{invoiceItem.UnitPrice}");
                                table.Cell().Background(backgroundColor).Padding(4).Text($"{invoiceItem.Quantity * invoiceItem.UnitPrice}");
                            }

                            table.Cell()
                            .ColumnSpan(5)
                            .PaddingVertical(5)
                            .BorderBottom(1)
                            .BorderColor(Colors.Black);

                            table.Cell().ColumnSpan(4).Text("Total").Bold().AlignRight();
                            table.Cell().AlignRight().Text(invoice.InvoiceItems.Sum(x => (x.UnitPrice * x.Quantity)).ToString());
                        }
                    });

                    column.Item().Column(column =>
                    {
                        column.Item()
                        .PaddingTop(30)
                        .Text("Thank you for your business.")
                        .FontFamily("Arial")
                        .FontSize(15)
                        .Bold();
                    });
                });

                page.Footer()
                .Column(column =>
                {
                    column.Item()
                    .PaddingVertical(10)
                    .Text(text =>
                    {
                        text.Span("Page ");
                        text.CurrentPageNumber();
                        text.Span(" of ");
                        text.TotalPages();
                        text.AlignCenter();
                    });
                });
            });
        });

        // QuestPDF Companion
        //document.ShowInCompanion();

        return document.GeneratePdf();
    }
}
