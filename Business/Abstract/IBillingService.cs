using Core.Utilities.Results;
using Entities.Concrete.Payment;
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
        IDataResult<Billing> RecievePay(string billId, int amount);

        IResult Update(Billing billing, string id);
        IResult Delete(string id);
        IDataResult<Billing> GetById(string id);
        IDataResult<List<Billing>> GetAll();
    }
}
