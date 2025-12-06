using Entities.Concrete.Duties;
using Entities.Concrete.InstallationRequests;
using Entities.Concrete.Offer;
using Entities.Concrete.Offers;
using Entities.Concrete.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete.Constants
{
    public class PdfGeneratorHelper
    {
        public static string CreateServicingPdfStructure(Servicing servicingToAdd)
        {
            var now = DateTime.Now;
            var monthName = now.ToString("MMMM", new System.Globalization.CultureInfo("tr-TR"));
            var yearSuffix = now.Year % 100;
            var folderName = $"{monthName}-{yearSuffix}";
            var folderPath = Path.Combine("wwwroot","Uploads", "servisler", folderName);
            Directory.CreateDirectory(folderPath);
            var fileName = $"servis_ {servicingToAdd.Name}.pdf";
            var safeFileName = string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
            var filePath = Path.Combine(folderPath, safeFileName);

            return filePath;
        }

        public static string CreateOfferPdfStructure(OfferDto offer)
        {
            var now = DateTime.Now;
            var monthName = now.ToString("MMMM", new System.Globalization.CultureInfo("tr-TR"));
            var yearSuffix = now.Year % 100;
            var folderName = $"{monthName}-{yearSuffix}";
            var folderPath = Path.Combine("wwwroot","Uploads", "teklifler", folderName);
            Directory.CreateDirectory(folderPath);
            var fileName = $"teklif_{offer.OfferTitle}.pdf";
            var safeFileName = string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
            var filePath = Path.Combine(folderPath, safeFileName);

            return filePath;
        }
        public static string CreateDailyDutiesReportPdfStructure()
        {
            var now = DateTime.Now;
            var monthName = now.ToString("MMMM", new System.Globalization.CultureInfo("tr-TR"));
            var yearSuffix = now.Year % 100;
            var folderName = $"{monthName}-{yearSuffix}";
            var folderPath = Path.Combine("wwwroot", "Uploads", "raporlar", folderName);
            Directory.CreateDirectory(folderPath);
            var fileName = $"rapor_{DateTime.Today}.pdf";
            var safeFileName = string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
            var filePath = Path.Combine(folderPath, safeFileName);

            return filePath;
        }


    }
}
