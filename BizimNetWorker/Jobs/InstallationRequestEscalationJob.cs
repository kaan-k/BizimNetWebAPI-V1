using Business.Abstract;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizimNetWorker.Jobs
{
    public class InstallationRequestEscalationJob : IJob
    {
        private readonly IInstallationRequestService _installationRequestService;

        public InstallationRequestEscalationJob(IInstallationRequestService installationRequestService)
        {
            _installationRequestService = installationRequestService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var result = _installationRequestService.WorkerCalculateEscalation();
            return Task.FromResult(result);
        }
    }
}
