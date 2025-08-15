using Core.Utilities.Results;
using Entities.Concrete.InstallationRequest;
using Entities.Concrete.InviteToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IInviteTokenService
    {
        IResult Add(InviteTokenCreateDto token);
        IResult Validate(InviteTokenValidationDto token);

    }
}
