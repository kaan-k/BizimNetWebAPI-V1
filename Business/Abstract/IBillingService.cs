using Core.Utilities.Results;
using Entities.Concrete.Payments;
using Entities.Concrete.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBillingService
    {
        IDataResult<Billing> Add(BillingDto billingDto);
        IDataResult<Billing> RecievePay(int billId, int amount);

        IResult Update(Billing billing);
        IResult Delete(int id);
        IDataResult<Billing> GetById(int id);
        IDataResult<List<Billing>> GetAll();
    }
}
