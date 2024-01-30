using System;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;
using IContainer = QuestPDF.Infrastructure.IContainer;

namespace BisleriumCafe.Data
{
    // Class responsible for generating PDF reports
    public class PDFGenerator : IDocument
    {
        // The model representing the data for the report
        public Reports ReportObj;

        // Constructor that initializes the model
        public PDFGenerator(Reports model)
        {
            ReportObj = model;
        }

        // Compose method for building the PDF document
        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(10));
                    page.Header().Element(ComposePDFHeader);
                    page.Content().Element(ComposePDFContent);
                });
        }

        // ComposePDFHeader method for building the header of the PDF
        void ComposePDFHeader(IContainer container)
        {
            string pdfTitle = $"Bislerium Cafe {ReportObj.ReportType} Transaction Report - ({ReportObj.ReportDate})";

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"{pdfTitle}").Style(TextStyle.Default.FontSize(24).Bold().Underline());

                    column.Item().Text(text =>
                    {
                        text.Span("Generation date: ").Medium();
                        text.Span($"{DateTime.Now}").Medium();
                    });
                });
            });
        }

        // ComposePDFContent method for building the content of the PDF
        void ComposePDFContent(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {
                column.Item().Element(ComposeSalesTransactionsTableTitle);
                column.Item().PaddingTop(10).Element(ComposeSalesTransactionsTableContent);
            });
        }

        // Sales Transactions Table Title
        // ComposeSalesTransactionsTableTitle method for building the title of the sales transactions table
        void ComposeSalesTransactionsTableTitle(IContainer container)
        {
            string title = $"{ReportObj.ReportType} Sales Transaction Report - ({ReportObj.ReportDate})";

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"{title}").Style(TextStyle.Default.FontSize(22).Bold());

                    column.Item().PaddingTop(10).Text(text =>
                    {
                        text.Span("Total Revenue: ").FontSize(18);
                        text.Span($"Rs. {ReportObj.Revenue}").FontSize(18);
                    });
                });
            });
        }

        // ComposeSalesTransactionsTableContent method
        // ComposeSalesTransactionsTableContent method for building the content of the sales transactions table
        void ComposeSalesTransactionsTableContent(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(20);
                    columns.ConstantColumn(120);
                    columns.ConstantColumn(90);
                    columns.ConstantColumn(80);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("No.");
                    header.Cell().Element(CellStyle).Text("Customer");
                    header.Cell().Element(CellStyle).Text("Contact");
                    header.Cell().Element(CellStyle).Text("Staff");
                    header.Cell().Element(CellStyle).Text("Total Price");
                    header.Cell().Element(CellStyle).Text("Discount");
                    header.Cell().Element(CellStyle).Text("Grand Total");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.Bold().FontSize(14)).PaddingVertical(10).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });
                    
                foreach (var order in ReportObj.OrdersList)
                {
                    table.Cell().Element(CellStyle).Text((ReportObj.OrdersList.IndexOf(order) + 1).ToString());
                    table.Cell().Element(CellStyle).Text(order.CustomerName);
                    table.Cell().Element(CellStyle).Text(order.CustomerContact);
                    table.Cell().Element(CellStyle).Text(order.StaffUsername);
                    table.Cell().Element(CellStyle).Text($"Rs.{order.Total}");
                    table.Cell().Element(CellStyle).Text($"Rs.{order.Discount}");
                    table.Cell().Element(CellStyle).Text($"Rs.{order.Total - order.Discount}");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(10);
                    }
                }
            });
        }   
    }
}
