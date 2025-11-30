using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete.Duty;
using Entities.Concrete.Offer;
using Entities.Concrete.Service;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class PdfGeneratorManager : IPdfGeneratorService
{
    private readonly ICustomerDal _customerDal;
    private readonly IBusinessUserDal _businessUserDal;

    // --- THEME COLORS ---
    private readonly string ColorPrimary = "#203B46";      // Dark Teal (Brand)
    private readonly string ColorAccent = "#26A69A";       // Bright Teal (Highlights)
    private readonly string ColorLight = "#F8FAFC";        // Very Light Blue/Gray (Backgrounds)
    private readonly string ColorText = "#1E293B";         // Slate (Main Text)
    private readonly string ColorMuted = "#64748B";        // Muted Gray (Secondary Text)
    private readonly string ColorBorder = "#E2E8F0";       // Light Border

    public PdfGeneratorManager(ICustomerDal customerDal, IBusinessUserDal businessUserDal)
    {
        _customerDal = customerDal;
        _businessUserDal = businessUserDal;
    }

    // =================================================================================================
    // 1. DAILY DUTIES REPORT (General Overview)
    // =================================================================================================
    public byte[] GenerateDailyDutiesPdf(List<Duty> duties, DateTime reportDate)
    {
        SetupLicense();
        var tr = new CultureInfo("tr-TR");

        duties = duties?.Where(d => d != null).ToList() ?? new List<Duty>();

        var customerIds = duties.Where(d => !string.IsNullOrEmpty(d.CustomerId)).Select(d => d.CustomerId).Distinct().ToList();
        var customers = _customerDal.GetAll(c => customerIds.Contains(c.Id)).ToDictionary(c => c.Id, c => c);

        var employeeIds = duties.Where(d => !string.IsNullOrEmpty(d.CompletedBy)).Select(d => d.CompletedBy).Distinct().ToList();
        var employees = _businessUserDal.GetAll(e => employeeIds.Contains(e.Id)).ToDictionary(e => e.Id, e => e);

        var completedCount = duties.Count(d => d.Status == "Tamamlandı");
        var pendingCount = duties.Count - completedCount;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                SetupPage(page);

                // HEADER
                page.Header().Element(e => ComposeHeader(e, "GÜNLÜK GÖREV RAPORU", reportDate.ToString("dd MMMM yyyy, dddd", tr)));

                // CONTENT
                page.Content().PaddingVertical(20f).Column(col =>
                {
                    col.Spacing(20f);

                    // KPI Cards
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Element(c => ComposeStatCard(c, "Toplam", duties.Count.ToString(), "#4F46E5"));
                        row.Spacing(10f);
                        row.RelativeItem().Element(c => ComposeStatCard(c, "Tamamlanan", completedCount.ToString(), "#10B981"));
                        row.Spacing(10f);
                        row.RelativeItem().Element(c => ComposeStatCard(c, "Bekleyen", pendingCount.ToString(), "#EF4444"));
                    });

                    if (duties.Count == 0)
                    {
                        col.Item().PaddingTop(30f).AlignCenter().Text("Bugün için kayıt bulunamadı.").FontSize(12f).FontColor(ColorMuted).Italic();
                        return;
                    }

                    // Data Table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(3f); // Task
                            c.RelativeColumn(2f); // Customer
                            c.RelativeColumn(2f); // Staff
                            c.RelativeColumn(1.5f); // Priority
                            c.RelativeColumn(1.5f); // Time
                        });

                        table.Header(h =>
                        {
                            h.Cell().Element(CellStyleHeader).Text("GÖREV ADI");
                            h.Cell().Element(CellStyleHeader).Text("MÜŞTERİ");
                            h.Cell().Element(CellStyleHeader).Text("PERSONEL");
                            h.Cell().Element(CellStyleHeader).AlignCenter().Text("ÖNCELİK");
                            h.Cell().Element(CellStyleHeader).AlignRight().Text("SAAT");
                        });

                        for (int i = 0; i < duties.Count; i++)
                        {
                            var duty = duties[i];
                            customers.TryGetValue(duty.CustomerId ?? "", out var customer);
                            employees.TryGetValue(duty.CompletedBy ?? "", out var employee);

                            IContainer CellStyleRow(IContainer c) => (i % 2 == 0) ? CellStyleEven(c) : CellStyleOdd(c);

                            table.Cell().Element(CellStyleRow).Text(duty.Name).SemiBold().FontColor(ColorText);
                            table.Cell().Element(CellStyleRow).Text(customer?.CompanyName ?? "-").FontSize(9f).FontColor(ColorMuted);
                            table.Cell().Element(CellStyleRow).Text(employee?.FirstName ?? "-").FontSize(9f).FontColor(ColorMuted);
                            table.Cell().Element(CellStyleRow).AlignCenter().Element(c => ComposePriorityBadge(c, duty.Priority));

                            // FIX: Apply Timezone Shift
                            var timeStr = ToTrTime(duty.CompletedAt)?.ToString("HH:mm") ?? "-";
                            table.Cell().Element(CellStyleRow).AlignRight().Text(timeStr).FontFamily("Arial").FontSize(10f);
                        }
                    });
                });

                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    // =================================================================================================
    // 2. DUTIES BY CUSTOMER REPORT (Grouped "Invoice" Style)
    // =================================================================================================
    public byte[] GenerateDutiesByCustomerPdf(List<Duty> duties, DateTime reportDate)
    {
        SetupLicense();
        var tr = new CultureInfo("tr-TR");

        duties = duties?.Where(d => d != null).ToList() ?? new List<Duty>();

        var customerIds = duties.Where(d => !string.IsNullOrEmpty(d.CustomerId)).Select(d => d.CustomerId).Distinct().ToList();
        var customers = _customerDal.GetAll(c => customerIds.Contains(c.Id)).ToDictionary(c => c.Id, c => c);

        var employeeIds = duties.Where(d => !string.IsNullOrEmpty(d.CompletedBy)).Select(d => d.CompletedBy).Distinct().ToList();
        var employees = _businessUserDal.GetAll(e => employeeIds.Contains(e.Id)).ToDictionary(e => e.Id, e => e);

        var dutiesByCustomer = duties.GroupBy(d => d.CustomerId);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                SetupPage(page);
                page.Header().Element(e => ComposeHeader(e, "HİZMET DÖKÜMÜ RAPORU", reportDate.ToString("dd MMMM yyyy", tr)));

                page.Content().PaddingVertical(10f).Column(col =>
                {
                    col.Spacing(30f);

                    if (!duties.Any())
                    {
                        col.Item().PaddingTop(50f).AlignCenter().Text("Kayıt bulunamadı.").FontSize(12f).FontColor(ColorMuted);
                        return;
                    }

                    foreach (var group in dutiesByCustomer)
                    {
                        customers.TryGetValue(group.Key ?? "", out var customer);
                        var customerName = customer?.CompanyName ?? "Bilinmeyen Müşteri";
                        var count = group.Count();

                        col.Item().Column(c =>
                        {
                            c.Item().Background(ColorLight).Border(1f).BorderColor(ColorBorder).Padding(12f).Row(r =>
                            {
                                r.RelativeItem().Column(info =>
                                {
                                    info.Item().Text("MÜŞTERİ").FontSize(8f).Bold().FontColor(ColorAccent);
                                    info.Item().Text(customerName).FontSize(12f).Bold().FontColor(ColorPrimary);
                                });

                                r.AutoItem().Column(info =>
                                {
                                    info.Item().AlignRight().Text("TOPLAM İŞLEM").FontSize(8f).Bold().FontColor(ColorAccent);
                                    info.Item().AlignRight().Text($"{count} Adet").FontSize(12f).Bold().FontColor(ColorText);
                                });
                            });

                            c.Item().PaddingTop(5f).Table(table =>
                            {
                                table.ColumnsDefinition(cols =>
                                {
                                    cols.RelativeColumn(4f);
                                    cols.RelativeColumn(2f);
                                    cols.RelativeColumn(2f);
                                    cols.RelativeColumn(1.5f);
                                });

                                table.Header(h =>
                                {
                                    h.Cell().Element(CellStyleHeader).Text("YAPILAN İŞLEM / AÇIKLAMA");
                                    h.Cell().Element(CellStyleHeader).Text("PERSONEL");
                                    h.Cell().Element(CellStyleHeader).Text("TARİH");
                                    h.Cell().Element(CellStyleHeader).AlignRight().Text("DURUM");
                                });

                                foreach (var duty in group)
                                {
                                    employees.TryGetValue(duty.CompletedBy ?? "", out var emp);
                                    var empName = emp?.FirstName ?? "-";

                                    // FIX: Apply Timezone Shift
                                    var dateStr = ToTrTime(duty.CompletedAt)?.ToString("dd.MM.yyyy", tr) ?? "-";
                                    var timeStr = ToTrTime(duty.CompletedAt)?.ToString("HH:mm", tr) ?? "-";

                                    IContainer RowStyle(IContainer x) => x.BorderBottom(1f).BorderColor(ColorBorder).PaddingVertical(8f).PaddingHorizontal(4f);

                                    table.Cell().Element(RowStyle).Column(cx => {
                                        cx.Item().Text(duty.Name).FontSize(10f).SemiBold().FontColor(ColorText);
                                        if (!string.IsNullOrEmpty(duty.Description))
                                        {
                                            cx.Item().Text(duty.Description).FontSize(9f).FontColor(ColorMuted);
                                        }
                                    });

                                    table.Cell().Element(RowStyle).Text(empName).FontSize(10f).FontColor(ColorText);

                                    table.Cell().Element(RowStyle).Column(cx => {
                                        cx.Item().Text(dateStr).FontSize(10f).FontColor(ColorText);
                                        cx.Item().Text(timeStr).FontSize(8f).FontColor(ColorMuted);
                                    });

                                    table.Cell().Element(RowStyle).AlignRight().Text(txt => {
                                        if (duty.Status == "Tamamlandı")
                                            txt.Span("TAMAMLANDI").FontSize(9f).Bold().FontColor(ColorPrimary);
                                        else
                                            txt.Span("AÇIK").FontSize(9f).Bold().FontColor("#F59E0B");
                                    });
                                }
                            });
                        });
                    }
                });

                page.Footer().Element(ComposeFooter);
            });
        });
        return document.GeneratePdf();
    }

    // =================================================================================================
    // 3. SERVICING REPORT (Form Layout)
    // =================================================================================================
    public byte[] GenerateServicingPdf(Servicing servicing)
    {
        SetupLicense();
        var tr = new CultureInfo("tr-TR");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                SetupPage(page);
                page.Header().Element(e => ComposeHeader(e, "TEKNİK SERVİS FORMU", $"Takip No: {servicing.TrackingId}"));

                page.Content().PaddingVertical(20f).Column(col =>
                {
                    col.Spacing(20f);

                    // Info Box
                    col.Item().Border(1f).BorderColor(ColorBorder).Padding(15f).Column(info =>
                    {
                        info.Spacing(10f);

                        // FIX: Apply Timezone Shift to Dates
                        var createdDate = ToTrTime(servicing.CreatedAt).ToString("g", tr);
                        var lastActionDate = ToTrTime(servicing.LastActionDate)?.ToString("g", tr) ?? "-";

                        info.Item().Row(r => {
                            r.RelativeItem().Column(c => { c.Item().Text("Servis Başlığı").FontSize(9f).FontColor(ColorMuted).Bold(); c.Item().Text(servicing.Name).FontSize(12f).SemiBold(); });
                            r.RelativeItem().AlignRight().Column(c => { c.Item().Text("Tarih & Saat").FontSize(9f).FontColor(ColorMuted).Bold(); c.Item().Text(createdDate).FontSize(12f); });
                        });

                        info.Item().LineHorizontal(1f).LineColor(ColorBorder);

                        info.Item().Row(r => {
                            r.RelativeItem().Column(c => { c.Item().Text("Durum").FontSize(9f).FontColor(ColorMuted).Bold(); c.Item().Text(servicing.Status.ToString()).FontSize(11f).FontColor(ColorPrimary); });
                            r.RelativeItem().AlignRight().Column(c => { c.Item().Text("Son İşlem").FontSize(9f).FontColor(ColorMuted).Bold(); c.Item().Text(lastActionDate).FontSize(11f); });
                        });
                    });

                    // Devices
                    col.Item().Column(dev => {
                        dev.Spacing(5f);
                        dev.Item().Text("İLGİLİ CİHAZLAR").FontSize(10f).Bold().FontColor(ColorAccent);
                        if (servicing.DeviceIds != null && servicing.DeviceIds.Any())
                        {
                            foreach (var d in servicing.DeviceIds) dev.Item().PaddingLeft(10f).Text($"• {d}").FontSize(10f);
                        }
                        else
                        {
                            dev.Item().Text("Cihaz belirtilmedi.").FontSize(10f).Italic().FontColor(ColorMuted);
                        }
                    });

                    // Actions
                    col.Item().Column(act => {
                        act.Spacing(5f);
                        act.Item().Text("YAPILAN İŞLEMLER").FontSize(10f).Bold().FontColor(ColorAccent);
                        act.Item().Background(ColorLight).Padding(10f).Text(servicing.LastAction).FontSize(10f);
                    });

                    // Signatures
                    col.Item().PaddingTop(40f).Row(r => {
                        r.RelativeItem().Column(c => { c.Spacing(40f); c.Item().Text("Teknisyen İmza").FontSize(10f).Bold().AlignCenter(); c.Item().LineHorizontal(1f); });
                        r.ConstantItem(40f);
                        r.RelativeItem().Column(c => { c.Spacing(40f); c.Item().Text("Müşteri Onay").FontSize(10f).Bold().AlignCenter(); c.Item().LineHorizontal(1f); });
                    });
                });
                page.Footer().Element(ComposeFooter);
            });
        });
        return document.GeneratePdf();
    }

    // =================================================================================================
    // 4. OFFER REPORT (Detailed Proposal)
    // =================================================================================================
    //public byte[] GenerateOfferPdf(Offer offer)
    //{
    //    SetupLicense();
    //    var tr = new CultureInfo("tr-TR");

    //    // FIX: Apply Timezone Shift
    //    var createdDate = ToTrTime(offer.CreatedAt ?? DateTime.Now).ToString("dd.MM.yyyy", tr);

    //    var document = Document.Create(container =>
    //    {
    //        container.Page(page =>
    //        {
    //            SetupPage(page);
    //            page.Header().Element(e => ComposeHeader(e, "RESMİ TEKLİF BELGESİ", $"Tarih: {createdDate}"));

    //            page.Content().PaddingVertical(20f).Column(col =>
    //            {
    //                col.Spacing(20f);

    //                col.Item().Border(1f).BorderColor(ColorBorder).Padding(20f).Background(ColorLight).Column(c => {
    //                    c.Spacing(10f);
    //                    c.Item().Text("Teklif Başlığı").FontSize(10f).FontColor(ColorAccent).Bold();
    //                    c.Item().Text(offer.OfferTitle).FontSize(14f).SemiBold().FontColor(ColorText);

    //                    c.Item().PaddingTop(10f).Text("Detaylar").FontSize(10f).FontColor(ColorAccent).Bold();
    //                    c.Item().Text(offer.OfferDetails).FontSize(10f).LineHeight(1.4f).FontColor(ColorText);
    //                });

    //                col.Item().Row(r => {
    //                    r.RelativeItem().Column(c => { c.Item().Text("Müşteri ID").Bold().FontSize(9f); c.Item().Text(offer.CustomerId).FontSize(10f); });
    //                    r.RelativeItem().Column(c => { c.Item().Text("Personel ID").Bold().FontSize(9f); c.Item().Text(offer.EmployeeId).FontSize(10f); });
    //                });

    //                col.Item().Background("#F0FDF4").Border(1f).BorderColor("#4ADE80").Padding(15f).AlignCenter().Column(c => {
    //                    c.Item().Text("TOPLAM TUTAR").FontSize(10f).Bold().FontColor("#15803D");
    //                    c.Item().Text($"{offer.TotalAmount:C}").FontSize(20f).Bold().FontColor("#166534");
    //                });

    //                col.Item().PaddingTop(40f).AlignRight().Width(150f).Column(c => {
    //                    c.Spacing(40f);
    //                    c.Item().Text("Yetkili İmza / Kaşe").FontSize(10f).Bold().AlignCenter();
    //                    c.Item().LineHorizontal(1f);
    //                });
    //            });
    //            page.Footer().Element(ComposeFooter);
    //        });
    //    });
    //    return document.GeneratePdf();
    //}

    // =================================================================================================
    // 5. OFFER / SALES INFO FORM (Using User's Offer Entity)
    // =================================================================================================
    public byte[] GenerateOfferPdf(OfferDto offer)
    {
        SetupLicense();
        var tr = new CultureInfo("tr-TR");

        // 1. Fetch Data
        var customer = _customerDal.Get(c => c.Id == offer.CustomerId);
        var companyName = customer?.CompanyName ?? "Bilinmeyen Müşteri";
        var companyPhone = customer?.PhoneNumber ?? "-"; // Assuming PhoneNumber exists on Customer entity
        var authPerson = "Yetkili Belirtilmedi"; // Customer entity might need an AuthorizedPerson field

        // 2. Calculate Totals available in the Offer entity
        // Since OfferItemDto only has Sales Price, Cost is assumed 0 for the visual layout
        decimal totalSales = offer.TotalAmount;
        decimal totalCost = 0; // Data not available in Offer entity
        decimal totalVat = totalSales * 0.20m; // Assuming 20% VAT standard, can be adjusted
        decimal netProfit = totalSales - totalCost; // Purely sales revenue since cost is unknown

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                SetupPage(page);

                // Replicate the "Red/Teal Border" layout
                page.Content().Border(2f).BorderColor(ColorPrimary).Padding(20f).Column(col =>
                {
                    col.Spacing(15f);

                    // --- HEADER SECTION ---
                    col.Item().Column(c =>
                    {
                        c.Item().Text("TEKLİF BİLGİ FORMU").FontSize(16f).ExtraBold().FontColor(ColorText);
                        // Using substring of ObjectId for a shorter "Record No" look
                    });

                    // --- INFO BLOCK (Key-Value) ---
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(100f); // Label width
                            columns.RelativeColumn();     // Value width
                        });

                        void InfoRow(string label, string value, bool isStriped = false)
                        {
                            table.Cell().Element(c => isStriped ? c.Background(ColorLight) : c)
                                 .PaddingVertical(4f).PaddingHorizontal(2f)
                                 .Text(label).Bold().FontSize(9f).FontColor(ColorText);

                            table.Cell().Element(c => isStriped ? c.Background(ColorLight) : c)
                                 .PaddingVertical(4f).PaddingHorizontal(2f)
                                 .Text(value).FontSize(9f).FontColor(ColorText);
                        }

                        // Mapping Offer + Customer Data to the Form
                        InfoRow("Firma", $"{companyName}\n{companyPhone}", true);
                        InfoRow("Firma Yetkilisi", authPerson);
                        InfoRow("Tarihi - Tipi", $"{ToTrTime(offer.CreatedAt)?.ToString("dd.MM.yyyy") ?? "-"} / Teklif");
                        InfoRow("Personel", "Sistem", true); // Personnel info not in Offer entity
                        InfoRow("Ödeme", offer.OfferTitle); // Using Title as Payment/Main desc
                        InfoRow("Genel Açıklama", offer.Description);
                        InfoRow("Para birimi - Kur", "TL - KDV Dahil", true);
                    });

                    // --- ITEMS TABLE ---
                    col.Item().PaddingTop(10f).Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(3f);   // Donanım
                            c.RelativeColumn(0.8f); // Adet (Increased from 0.5f to fix layout crash)
                            c.RelativeColumn(1.2f); // Brm Maliyet (Empty)
                            c.RelativeColumn(1.2f); // Brm Satış
                            c.RelativeColumn(1.2f); // Maliyet (Empty)
                            c.RelativeColumn(1.2f); // Satış
                            c.RelativeColumn(1f);   // Teslim
                        });



                        // Header
                        table.Header(h =>
                        {
                            h.Cell().Element(CellStyleTableBorder).Text("Donanım").Bold();
                            h.Cell().Element(CellStyleTableBorder).AlignCenter().Text("Adet").Bold();
                            h.Cell().Element(CellStyleTableBorder).AlignRight().Text("Brm Maliyet").Bold();
                            h.Cell().Element(CellStyleTableBorder).AlignRight().Text("Brm Satış").Bold();
                            h.Cell().Element(CellStyleTableBorder).AlignRight().Text("Maliyet").Bold();
                            h.Cell().Element(CellStyleTableBorder).AlignRight().Text("Satış").Bold();
                            h.Cell().Element(CellStyleTableBorder).AlignCenter().Text("Teslim").Bold();
                        });

                        // Rows
                        if (offer.items != null)
                        {
                            foreach (var item in offer.items)
                            {
                                table.Cell().Element(CellStyleTableBorder).Text(item.StockName);
                                table.Cell().Element(CellStyleTableBorder).AlignCenter().Text(item.Quantity.ToString());

                                // Cost columns are "-" because OfferItemDto lacks cost data
                                table.Cell().Element(CellStyleTableBorder).AlignRight().Text("-");

                                table.Cell().Element(CellStyleTableBorder).AlignRight().Text(((decimal)item.UnitPrice).ToString("N2", tr));

                                table.Cell().Element(CellStyleTableBorder).AlignRight().Text("-");

                                table.Cell().Element(CellStyleTableBorder).AlignRight().Text(((decimal)item.TotalPrice).ToString("N2", tr));
                                table.Cell().Element(CellStyleTableBorder).AlignCenter().Text("-");
                            }
                        }

                        // Table Footer (Totals)
                        table.Footer(f =>
                        {
                            f.Cell().ColumnSpan(4).Element(CellStyleTableBorder).AlignRight().Text("Toplamlar:").Bold();
                            f.Cell().Element(CellStyleTableBorder).AlignRight().Text("-"); // Total Cost Unknown
                            f.Cell().Element(CellStyleTableBorder).AlignRight().Text(totalSales.ToString("N2", tr)).Bold();
                            f.Cell().Element(CellStyleTableBorder);
                        });
                    });

                    // --- SUMMARY BOX (Orange Box) ---
                    col.Item().Width(250f).Background("#FFF7ED") // Light Orange
                       .Padding(10f)
                       .Table(table =>
                       {
                           table.ColumnsDefinition(c =>
                           {
                               c.RelativeColumn(2f);
                               c.RelativeColumn(1f);
                           });

                           void SummaryRow(string label, string value, bool isRed = false)
                           {
                               table.Cell().PaddingBottom(5).Text(label + " :").FontSize(9f).Bold().FontColor(ColorText);
                               string valColor = isRed ? "#DC2626" : "#EA580C";
                               table.Cell().PaddingBottom(5).AlignRight().Text(value).FontSize(9f).Bold().FontColor(valColor);
                           }

                           SummaryRow("Toplam Satış (TL)", totalSales.ToString("N2", tr));
                           SummaryRow("Donanım Maliyet(TL)", "-"); // Data missing
                           SummaryRow("Kdv Toplamı (TL)", totalVat.ToString("N2", tr)); // Calculated 20% assumption
                           SummaryRow("Kdv Toplamı(Maliyet)", "-"); // Data missing

                           // Divider
                           table.Cell().ColumnSpan(2).PaddingVertical(5).LineHorizontal(1f).LineColor("#FDBA74");

                           // Net Result
                           table.Cell().Text("NET (TL):").FontSize(10f).ExtraBold().FontColor(ColorText);
                           table.Cell().AlignRight().Text(totalSales.ToString("N2", tr)).FontSize(10f).ExtraBold().FontColor("#DC2626");
                       });
                });

                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    // Helper for table borders (Keep inside the class)
    private IContainer CellStyleTableBorder(IContainer container)
    {
        return container
            .Border(1f)
            .BorderColor(ColorBorder)
            .Padding(4f) // Reduced padding complexity to ensure fit
                         // Removed .PaddingHorizontal(6f) to prevent padding stacking (4+6=10) which caused the crash
            .DefaultTextStyle(x => x.FontSize(9f));
    }





    // =================================================================================================
    //                                     PRIVATE HELPERS
    // =================================================================================================

    private void SetupLicense()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    // *** NEW HELPER: Turkey Time Converter (+3 Hours) ***
    private DateTime ToTrTime(DateTime date) => date.AddHours(3);
    private DateTime? ToTrTime(DateTime? date) => date?.AddHours(3);

    private void SetupPage(PageDescriptor page)
    {
        page.Margin(40f);
        page.Size(PageSizes.A4);
        page.DefaultTextStyle(x => x.FontSize(10f).FontFamily("Arial"));
        page.PageColor(Colors.White);
    }

    private void ComposeHeader(IContainer container, string title, string subtitle)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("ARY YAZILIM").FontSize(20f).ExtraBold().FontColor(ColorPrimary);
                });

                row.RelativeItem().AlignRight().Column(c =>
                {
                    c.Item().Text(title).FontSize(14f).Bold().FontColor(ColorAccent);
                    c.Item().Text(subtitle).FontSize(10f).FontColor(ColorText);
                });
            });
            col.Item().PaddingTop(10f).LineHorizontal(2f).LineColor(ColorPrimary);
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().PaddingBottom(5f).LineHorizontal(1f).LineColor(ColorBorder);
            col.Item().Row(row =>
            {
                row.RelativeItem().Text("www.aryyazilim.com | info@aryyazilim.com").FontSize(8f).FontColor(ColorMuted);
                row.RelativeItem().AlignRight().Text(x =>
                {
                    x.Span("Sayfa ");
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
        });
    }

    private void ComposeStatCard(IContainer container, string label, string value, string colorHex)
    {
        container
            .Border(1f).BorderColor(ColorBorder)
            .Background(Colors.White)
            .Padding(10f)
            .Column(col =>
            {
                col.Item().Text(label).FontSize(9f).FontColor(ColorMuted).Medium();
                col.Item().Text(value).FontSize(18f).Bold().FontColor(colorHex);
            });
    }

    private void ComposePriorityBadge(IContainer container, string priority)
    {
        var color = priority?.ToLower() switch
        {
            "acil" => "#FEF2F2",
            "yüksek" => "#FFF7ED",
            "orta" => "#EFF6FF",
            _ => "#F1F5F9"
        };
        var text = priority?.ToLower() switch
        {
            "acil" => "#DC2626",
            "yüksek" => "#EA580C",
            "orta" => "#2563EB",
            _ => "#475569"
        };

        container.Background(color).PaddingVertical(2f).PaddingHorizontal(8f)
            .Text(priority?.ToUpper() ?? "-").FontSize(8f).Bold().FontColor(text);
    }

    private IContainer CellStyleHeader(IContainer container)
    {
        return container
            .BorderBottom(1f).BorderColor(ColorPrimary)
            .Background(ColorLight)
            .Padding(6f)
            .PaddingBottom(8f);
    }

    private IContainer CellStyleOdd(IContainer container) => container.BorderBottom(1f).BorderColor(ColorBorder).Padding(6f);
    private IContainer CellStyleEven(IContainer container) => container.Background(ColorLight).BorderBottom(1f).BorderColor(ColorBorder).Padding(6f);
}