using Business.Abstract;
using Core.Utilities.Results;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizimNetWorker.Jobs
{
    public class OfferEscalationJob : IJob
    {
        private readonly IOfferService _offerService;

        public OfferEscalationJob(IOfferService offerService)
        {
            _offerService = offerService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("OfferEscalationJob başladı");
                //_offerService.WorkerCalculateEscelation();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA: OfferEscalationJob çöktü: {ex.Message}");
            }

            return Task.CompletedTask;

        }
    }
}
