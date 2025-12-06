using Core.DataAccess; // ✅ Where IEntityRepository is
using Entities.Concrete.InstallationRequests; // ✅ Plural Namespace
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    // Inherit from IEntityRepository (Generic)
    public interface IInstallationRequestDal : IEntityRepository<InstallationRequest>
    {
        List<InstallationRequest> GetAllInstallationRequestDetails();
    }
}