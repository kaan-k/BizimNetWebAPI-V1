using Entities.Concrete.Duties; // ✅ Plural
using Entities.Concrete.Offers; // ✅ Plural
using Entities.Concrete.Services; // ✅ Plural
using System;
using System.Collections.Generic;

public interface IPdfGeneratorService
{
    byte[] GenerateServicingPdf(Servicing servicing);
    byte[] GenerateDailyDutiesPdf(List<Duty> duties, DateTime reportDate);
    byte[] GenerateDutiesByCustomerPdf(List<Duty> duties, DateTime reportDate);
    byte[] GenerateOfferPdf(OfferDto offer);
}