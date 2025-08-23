using Entities.Concrete.Offer;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using System.Globalization;
using Entities.Concrete.Service;
using Entities.Concrete.Duty;
using Business.Abstract;
using DataAccess.Abstract;

public class PdfGeneratorManager : IPdfGeneratorService
{
    private readonly ICustomerDal _customerDal;
    private readonly IBusinessUserDal _businessUserDal;
    public PdfGeneratorManager(ICustomerDal customerDal, IBusinessUserDal businessUserDal)
    {
        _customerDal = customerDal;
        _businessUserDal = businessUserDal;
    }
    public byte[] GenerateDailyDutiesPdf(List<Duty> duties, DateTime reportDate)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        duties = duties?.Where(d => d != null).ToList() ?? new List<Duty>();

        // ---- Performans: müşteri ve kullanıcıları tek seferde çek (N+1 engel)
        var customerIds = duties.Where(d => !string.IsNullOrEmpty(d.CustomerId))
                                .Select(d => d.CustomerId)
                                .Distinct()
                                .ToList();
        var customers = _customerDal
            .GetAll(c => customerIds.Contains(c.Id))
            .ToDictionary(c => c.Id, c => c);

        var employeeIds = duties.Where(d => !string.IsNullOrEmpty(d.CompletedBy))
                                .Select(d => d.CompletedBy)
                                .Distinct()
                                .ToList();
        var employees = _businessUserDal
            .GetAll(e => employeeIds.Contains(e.Id))
            .ToDictionary(e => e.Id, e => e);

        // ---- Hücre stilleri (excel benzeri)
        Func<IContainer, IContainer> Cell = c => c
            .Border(0.75f)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(6);

        Func<IContainer, IContainer> HeaderCell = c => c
            .Background(Colors.Grey.Lighten3)
            .Border(0.75f)
            .BorderColor(Colors.Grey.Lighten1)
            .Padding(8);

        var tr = new CultureInfo("tr-TR");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                // === Header ===
                page.Header().Column(col =>
                {
                    col.Item().Text("Ary Yazılım A.Ş.")
                        .FontSize(18).Bold().FontColor(Colors.Blue.Medium);

                    col.Item().Text("Günlük Görev Tamamlama Raporu")
                        .FontSize(22).Bold();

                    col.Item().Text(reportDate.ToString("dd MMMM yyyy", tr))
                        .FontSize(14).FontColor(Colors.Grey.Darken2);

                    col.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                });

                // === Content ===
                page.Content().PaddingVertical(20).Column(content =>
                {
                    content.Spacing(15);

                    // Özet kutusu
                    content.Item().Border(1).BorderColor(Colors.Grey.Lighten1)
                           .Padding(12).Background(Colors.Grey.Lighten5).Column(summary =>
                           {
                               summary.Spacing(5);
                               summary.Item().Text($"Bugün Tamamlanan Görev: {duties.Count}").Bold();
                           });

                    // Boşsa bilgilendir
                    if (duties.Count == 0)
                    {
                        content.Item().Padding(20).AlignCenter().Text("Bugün için tamamlanan görev bulunamadı.")
                               .FontColor(Colors.Grey.Darken2);
                        return;
                    }

                    // Excel benzeri tablolu görünüm
                    content.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2); // Görev Adı
                            columns.RelativeColumn(1); // Öncelik
                            columns.RelativeColumn(2); // Müşteri
                            columns.RelativeColumn(2); // Tamamlayan
                            columns.RelativeColumn(2); // Tamamlanma
                        });

                        // header (her sayfada tekrar etsin)
                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderCell).Text("Görev Adı").Bold().AlignCenter();
                            header.Cell().Element(HeaderCell).AlignCenter().Text("Öncelik").Bold().AlignCenter();
                            header.Cell().Element(HeaderCell).Text("Müşteri").Bold().AlignCenter();
                            header.Cell().Element(HeaderCell).Text("Tamamlayan").Bold().AlignCenter();
                            header.Cell().Element(HeaderCell).AlignCenter().Text("Tamamlanma").Bold().AlignCenter();
                        });

                        var i = 0; // zebra

                        foreach (var duty in duties)
                        {
                            customers.TryGetValue(duty.CustomerId ?? "", out var customer);
                            employees.TryGetValue(duty.CompletedBy ?? "", out var employee);

                            // zebra satır arka planı
                            Func<IContainer, IContainer> Zebra = c =>
                                (i++ % 2 == 0) ? c.Background(Colors.Grey.Lighten5) : c;

                            table.Cell().Element(Zebra).Element(Cell).AlignCenter()
                                 .Text(duty.Name ?? "-").AlignCenter();

                            table.Cell().Element(Zebra).Element(Cell).AlignCenter()
                                 .Text(duty.Priority ?? "-").AlignCenter();

                            table.Cell().Element(Zebra).Element(Cell).AlignCenter()
                                 .Text(customer?.CompanyName ?? "-").AlignCenter();

                            var empName = (employee == null)
                                ? "-"
                                : string.Join(" ", new[] { employee.FirstName }
                                                      .Where(s => !string.IsNullOrWhiteSpace(s)));
                            table.Cell().Element(Zebra).Element(Cell)
                                 .Text(string.IsNullOrWhiteSpace(empName) ? "-" : empName).AlignCenter();

                            table.Cell().Element(Zebra).Element(Cell).AlignCenter()
                                 .Text(duty.CompletedAt?.ToString("dd.MM.yyyy HH:mm", tr) ?? "-").AlignCenter();
                        }
                    });
                });

                // === Footer ===
                page.Footer().AlignCenter().Column(col =>
                {
                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    col.Item().Text(text =>
                    {
                        text.Span("Ary Yazılım A.Ş.").Bold();
                        text.Span(" | Günlük Rapor Sistemi | ");
                        text.Span("www.aryyazilim.com");
                    });
                    col.Item().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });
        });

        return document.GeneratePdf();
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
                                deviceCol.Item().Text($"• {deviceId}").FontColor(Colors.Grey.Darken2);
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
