using Business.Abstract;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace BizimNetWorker.Jobs
{
    public class DailyReportJob : IJob
    {
        private readonly IDutyService _dutyService;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        public DailyReportJob(IDutyService dutyService, IPdfGeneratorService pdfGeneratorService)
        {
            _dutyService = dutyService;
            _pdfGeneratorService = pdfGeneratorService;
        }
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();

            //var duties = _dutyService.GetTodaysDuties();

            //var result = _pdfGeneratorService.GenerateDailyDutiesPdf(duties.Data, DateTime.Today);
            //return Task.FromResult(result);

        }
    }
}
