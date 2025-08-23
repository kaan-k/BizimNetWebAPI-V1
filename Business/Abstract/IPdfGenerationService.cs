using Entities.Concrete.Duty;
using Entities.Concrete.Offer;
using Entities.Concrete.Service;

public interface IPdfGeneratorService
{
    byte[] GenerateOfferPdf(Offer offer);
    byte[] GenerateServicingPdf(Servicing servicing);
    byte[] GenerateDailyDutiesPdf(List<Duty> duties, DateTime reportDate);


}
