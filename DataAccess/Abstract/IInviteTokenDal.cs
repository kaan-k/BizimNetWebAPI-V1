using Core.DataAccess.MongoDB;
using Entities.Concrete.InviteToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IInviteTokenDal:IMongoRepository<InviteToken>
    {
    }
}
