using Entities.Concrete.Offer;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using System.Globalization;
using Entities.Concrete.Service;
using Business.Abstract;

public class PdfGeneratorManager : IPdfGeneratorService
{
    private readonly IDeviceService _deviceService;
    public PdfGeneratorManager(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }
    public byte[] GenerateOfferPdf(Offer offer)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                // === Header ===
                page.Header().Element(header =>
                {
                    header.Column(col =>
                    {
                        col.Item().Text("Ary Yazılım A.Ş.")
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Blue.Medium);

                        col.Item().Text("Resmi Teklif Belgesi")
                            .FontSize(24)
                            .Bold()
                            .FontColor(Colors.Black);

                        col.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });
                });

                // === Content ===
                page.Content().PaddingVertical(25).Column(column =>
                {
                    column.Spacing(20);

                    // Offer Info Box
                    column.Item().Border(1)
                                 .BorderColor(Colors.Grey.Lighten1)
                                 .Padding(15)
                                 .Background(Colors.Grey.Lighten5)
                                 .Column(info =>
                                 {
                                     info.Spacing(5);

                                     info.Item().Text("Teklif Başlığı").Bold();
                                     info.Item().Text(offer.OfferTitle).FontSize(14).FontColor(Colors.Grey.Darken3);

                                     info.Item().Text("Teklif Detayları");
                                     info.Item().Text(offer.OfferDetails)
                                                .FontColor(Colors.Grey.Darken2)
                                                .LineHeight(1.4f);
                                 });

                    // Technical Details
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Müşteri ID").Bold();
                            col.Item().Text(offer.CustomerId).FontColor(Colors.Grey.Darken2);
                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Personel ID").Bold();
                            col.Item().Text(offer.EmployeeId).FontColor(Colors.Grey.Darken2);
                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Oluşturulma Tarihi").Bold();
                            col.Item().Text(offer.CreatedAt?.ToString("dd MMMM yyyy HH:mm", new CultureInfo("tr-TR")))
                                     .FontColor(Colors.Grey.Darken2);
                        });
                    });

                    // Price Highlight Box
                    column.Item().Background(Colors.White)
                                 .Border(1)
                                 .BorderColor(Colors.Green.Medium)
                                 .Padding(15)
                                 .AlignCenter()
                                 .Column(col =>
                                 {
                                     col.Item().Text("Toplam Teklif Tutarı")
                                               .FontSize(14)
                                               .Bold()
                                               .FontColor(Colors.Green.Darken2);

                                     col.Item().Text($"{offer.TotalAmount:C}")
                                               .FontSize(22)
                                               .Bold()
                                               .FontColor(Colors.Green.Medium);
                                 });

                    // Optional: Signature area
                    column.Item().PaddingTop(30).Column(col =>
                    {
                        col.Item().Text("Yetkili İmza").Bold();
                        col.Item().PaddingTop(40).LineHorizontal(1);
                    });
                });

                // === Footer ===
                page.Footer().AlignCenter().Column(col =>
                {
                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    col.Item().Text(text =>
                    {
                        text.Span("Ary Yazılım A.Ş.").Bold();
                        text.Span(" | Otomatik Teklif Sistemi | ");
                        text.Span("www.aryyazilim.com");
                    });
                });
            });
        });

        return document.GeneratePdf();
    }

    public byte[] GenerateServicingPdf(Servicing servicing)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(50);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                // === Header ===
                page.Header().Element(header =>
                {
                    header.Column(col =>
                    {
                        col.Item().Text("ARY YAZILIM A.Ş.")
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Blue.Medium);

                        col.Item().Text("SERVİS FORMU")
                            .FontSize(24)
                            .Bold()
                            .FontColor(Colors.Black);

                        col.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });
                });

                // === Content ===
                page.Content().PaddingVertical(20).Column(column =>
                {
                    column.Spacing(20);

                    // Service Info Box
                    column.Item().Border(1)
                                 .BorderColor(Colors.Grey.Lighten1)
                                 .Padding(15)
                                 .Background(Colors.Grey.Lighten5)
                                 .Column(info =>
                                 {
                                     info.Spacing(5);

                                     info.Item().Text("Servis Başlığı").Bold();
                                     info.Item().Text(servicing.Name).FontSize(14).FontColor(Colors.Grey.Darken3);

                                     info.Item().Text("Takip Numarası").Bold();
                                     info.Item().Text(servicing.TrackingId).FontColor(Colors.Blue.Medium);
                                 });

                    // Device List
                    column.Item().Text("İlgili Cihazlar").Bold();
                    if (servicing.DeviceIds != null && servicing.DeviceIds.Any())
                    {
                        column.Item().Column(deviceCol =>
                        {
                            foreach (var deviceId in servicing.DeviceIds)
                            {
                                var name = _deviceService.GetNameById(deviceId);
                                deviceCol.Item().Text($"• {name.Data}").FontColor(Colors.Grey.Darken2);
                            }
                        });
                    }
                    else
                    {
                        column.Item().Text("Cihaz bilgisi bulunmamaktadır.").Italic();
                    }

                    // Status & Actions
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Durum").Bold();
                            col.Item().Text(servicing.Status.ToString()).FontColor(Colors.Grey.Darken2);
                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Son İşlem Tarihi").Bold();
                            col.Item().Text(servicing.LastActionDate?.ToString("dd MMMM yyyy HH:mm", new CultureInfo("tr-TR")) ?? "—")
                                     .FontColor(Colors.Grey.Darken2);
                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Oluşturulma").Bold();
                            col.Item().Text(servicing.CreatedAt.ToString("dd MMMM yyyy HH:mm", new CultureInfo("tr-TR")))
                                     .FontColor(Colors.Grey.Darken2);
                        });
                    });

                    column.Item().Text("Son Yapılan İşlem").Bold();
                    column.Item().Text(servicing.LastAction).FontColor(Colors.Grey.Darken3).LineHeight(1.4f);

                    // Signature Section
                    column.Item().PaddingTop(30).Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Teknisyen İmzası").Bold();
                            col.Item().PaddingTop(40).LineHorizontal(1);
                        });

                        row.ConstantItem(50); // spacer

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Müşteri İmzası").Bold();
                            col.Item().PaddingTop(40).LineHorizontal(1);
                        });
                    });
                });

                // === Footer ===
                page.Footer().AlignCenter().Column(col =>
                {
                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    col.Item().Text(text =>
                    {
                        text.Span("Ary Yazılım A.Ş.").Bold();
                        text.Span(" | Servis Takip Sistemi | ");
                        text.Span("www.aryyazılım.com");
                    });
                });
            });
        });

        return document.GeneratePdf();
    }
}
