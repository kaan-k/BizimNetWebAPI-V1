using Entities.Concrete.Offer;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using System.Globalization;
using Entities.Concrete.Service;

public class PdfGeneratorManager : IPdfGeneratorService
{

    public byte[] GenerateOfferPdf(Offer offer)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var trCulture = new CultureInfo("tr-TR");
        var createdAtText = offer.CreatedAt.HasValue
            ? offer.CreatedAt.Value.ToString("dd MMMM yyyy HH:mm", trCulture)
            : "-";

        var totalAmountText = string.Format(trCulture, "{0:C}", offer.TotalAmount);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                // Page
                page.Size(PageSizes.A4);
                page.Margin(50);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                // Header
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

                        col.Item().PaddingTop(5)
                            .LineHorizontal(1)
                            .LineColor(Colors.Grey.Lighten2);
                    });
                });

                // Content
                page.Content().PaddingVertical(25).Column(column =>
                {
                    column.Spacing(20);

                    // Offer Info Box
                    column.Item()
                        .Border(1).BorderColor(Colors.Grey.Lighten1)
                        .Padding(15)
                        .Background(Colors.Grey.Lighten5)
                        .Column(info =>
                        {
                            info.Spacing(5);

                            info.Item().Text("Teklif Başlığı").Bold();
                            info.Item().Text(offer.OfferTitle ?? string.Empty)
                                .FontSize(14)
                                .FontColor(Colors.Grey.Darken3);

                            info.Item().Text("Teklif Detayları").Bold();
                            info.Item().Text(offer.OfferDetails ?? string.Empty)
                                .FontColor(Colors.Grey.Darken2)
                                .LineHeight(1.4f);
                        });

                    // Technical Details (3 columns)
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Müşteri ID").Bold();
                            col.Item().Text(offer.CustomerId ?? "-")
                                .FontColor(Colors.Grey.Darken2);
                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Personel ID").Bold();
                            col.Item().Text(offer.EmployeeId ?? "-")
                                .FontColor(Colors.Grey.Darken2);
                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Oluşturulma Tarihi").Bold();
                            col.Item().Text(createdAtText)
                                .FontColor(Colors.Grey.Darken2);
                        });
                    });

                    // Price Highlight Box
                    column.Item()
                        .Background(Colors.White)
                        .Border(1).BorderColor(Colors.Green.Medium)
                        .Padding(15)
                        .AlignCenter()
                        .Column(col =>
                        {
                            col.Item().Text("Toplam Teklif Tutarı")
                                .FontSize(14)
                                .Bold()
                                .FontColor(Colors.Green.Darken2);

                            col.Item().Text(totalAmountText)
                                .FontSize(22)
                                .Bold()
                                .FontColor(Colors.Green.Medium);
                        });

                    // Signature area
                    column.Item().PaddingTop(30).Column(col =>
                    {
                        col.Item().Text("Yetkili İmza").Bold();
                        col.Item().PaddingTop(40).LineHorizontal(1);
                    });
                });

                // Footer
                page.Footer()
                    .AlignCenter()
                    .Column(col =>
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


    // using QuestPDF.Fluent;
    // using QuestPDF.Helpers;
    // using QuestPDF.Infrastructure;
    // using System.Globalization;

    public byte[] GenerateServicingPdf(Servicing servicing)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var tr = new CultureInfo("tr-TR");

        string name = servicing.Name ?? "—";
        string trackingId = servicing.TrackingId ?? "—";
        string statusText = servicing.Status?.ToString() ?? "—";
        string lastAction = string.IsNullOrWhiteSpace(servicing.LastAction) ? "—" : servicing.LastAction;

        string lastActionDate = servicing.LastActionDate.HasValue
            ? servicing.LastActionDate.Value.ToString("dd MMMM yyyy HH:mm", tr)
            : "—";

        string createdAt = servicing.CreatedAt.ToString("dd MMMM yyyy HH:mm", tr);

        // Status color mapping (adjust to your taste)
        string StatusBg(string s) => s?.ToLowerInvariant() switch
        {
            "beklemede" or "pending" => Colors.Grey.Lighten4,
            "devam ediyor" or "inprogress" or "processing" => Colors.Blue.Lighten4,
            "tamamlandı" or "completed" or "done" => Colors.Green.Lighten4,
            "iptal" or "canceled" => Colors.Red.Lighten4,
            _ => Colors.Grey.Lighten4
        };

        string StatusFg(string s) => s?.ToLowerInvariant() switch
        {
            "beklemede" or "pending" => Colors.Grey.Darken2,
            "devam ediyor" or "inprogress" or "processing" => Colors.Blue.Darken2,
            "tamamlandı" or "completed" or "done" => Colors.Green.Darken2,
            "iptal" or "canceled" => Colors.Red.Darken2,
            _ => Colors.Grey.Darken2
        };

        var brandPrimary = Colors.Blue.Medium;     // header band
        var cardBorder = Colors.Grey.Lighten2;     // card borders
        var textMuted = Colors.Grey.Darken2;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                // Page
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                // Header (band)
                page.Header().PaddingBottom(15).Element(h =>
                {
                    h.Background(brandPrimary).Padding(16).Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("ARY YAZILIM A.Ş.")
                                .FontSize(16).Bold().FontColor(Colors.White);

                            col.Item().Text("SERVİS FORMU")
                                .FontSize(22).Bold().FontColor(Colors.White);
                        });

                        row.AutoItem().Column(col =>
                        {
                            // tracking pill
                            col.Item().PaddingTop(4).PaddingLeft(10).PaddingRight(10)
                                .Background(Colors.White)
                                .PaddingVertical(6).PaddingHorizontal(10)
                                .Border(0.5f).BorderColor(Colors.Grey.Lighten2)
                                .AlignCenter()
                                .Text($"Takip: {trackingId}")
                                .FontSize(11).Bold().FontColor(brandPrimary);
                        });
                    });
                });

                // Content
                page.Content().Column(column =>
                {
                    column.Spacing(16);

                    // Info Card (2 columns)
                    column.Item().Border(1).BorderColor(cardBorder).Background(Colors.Grey.Lighten5).Padding(14).Column(info =>
                    {
                        info.Spacing(10);

                        info.Item().Text("Servis Bilgileri").Bold().FontSize(13);

                        info.Item().Row(r =>
                        {
                            r.RelativeItem().Column(c =>
                            {
                                c.Item().Text(t =>
                                {
                                    t.Span("Servis Başlığı: ").Bold();
                                    t.Span(name).FontColor(textMuted);
                                });
                                c.Item().Text(t =>
                                {
                                    t.Span("Durum: ").Bold();
                                    t.Span(statusText).FontColor(StatusFg(statusText));
                                });
                            });

                            r.RelativeItem().Column(c =>
                            {
                                c.Item().Text(t =>
                                {
                                    t.Span("Oluşturulma: ").Bold();
                                    t.Span(createdAt).FontColor(textMuted);
                                });
                                c.Item().Text(t =>
                                {
                                    t.Span("Son İşlem Tarihi: ").Bold();
                                    t.Span(lastActionDate).FontColor(textMuted);
                                });
                            });
                        });
                    });

                    // Devices Table
                    column.Item().Column(devSec =>
                    {
                        devSec.Spacing(8);
                        devSec.Item().Text("İlgili Cihazlar").Bold().FontSize(13);

                        if (servicing.DeviceIds != null && servicing.DeviceIds.Any())
                        {
                            devSec.Item().Table(table =>
                            {
                                table.ColumnsDefinition(cols =>
                                {
                                    cols.ConstantColumn(28);  // #
                                    cols.RelativeColumn();     // Device Id
                                });

                                // Header row
                                table.Header(header =>
                                {
                                    header.Cell().Element(CellHeader).Text("#").Bold();
                                    header.Cell().Element(CellHeader).Text("Cihaz ID").Bold();

                                    static IContainer CellHeader(IContainer c) =>
                                        c.DefaultTextStyle(x => x.SemiBold())
                                         .PaddingVertical(6).PaddingHorizontal(8)
                                         .Background(Colors.Grey.Lighten3)
                                         .BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                                });

                                var index = 1;
                                foreach (var id in servicing.DeviceIds)
                                {
                                    var even = (index % 2 == 0);
                                    table.Cell().Element(CellBody(even)).Text(index.ToString());
                                    table.Cell().Element(CellBody(even)).Text(id ?? "—").FontColor(textMuted);
                                    index++;

                                    static Func<IContainer, IContainer> CellBody(bool evenRow) => c =>
                                        c.PaddingVertical(6).PaddingHorizontal(8)
                                         .Background(evenRow ? Colors.Grey.Lighten5 : Colors.White)
                                         .BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten3);
                                }
                            });
                        }
                        else
                        {
                            devSec.Item().Text("Cihaz bilgisi bulunmamaktadır.")
                                 .Italic().FontColor(Colors.Grey.Darken1);
                        }
                    });

                    // Last Action (highlight card)
                    column.Item().Border(1).BorderColor(cardBorder).Padding(12).Background(Colors.Grey.Lighten5)
                        .Column(sec =>
                        {
                            sec.Spacing(6);
                            sec.Item().Text("Son Yapılan İşlem").Bold().FontSize(13);
                            sec.Item().Text(lastAction).FontColor(textMuted).LineHeight(1.4f);
                        });

                    // Signatures
                    column.Item().PaddingTop(18).Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(26).Text("Teknisyen İmzası").Bold();
                            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        });

                        row.ConstantItem(40); // spacer

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(26).Text("Müşteri İmzası").Bold();
                            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        });
                    });
                });

                // Footer with page numbers + contact
                page.Footer().Column(col =>
                {
                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    col.Item().PaddingTop(6).Row(r =>
                    {
                        r.RelativeItem().Text(t =>
                        {
                            t.Span("Ary Yazılım A.Ş.").Bold();
                            t.Span(" | Servis Takip Sistemi | ");
                            t.Span("www.aryyazilim.com");
                        });

                        r.AutoItem().Text(t =>
                        {
                            t.Span("Sayfa ");
                            t.CurrentPageNumber();
                            t.Span("/");
                            t.TotalPages();
                        });
                    });
                });
            });
        });

        return document.GeneratePdf();
    }


}
