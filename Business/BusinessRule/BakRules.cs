using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessRule
{
    public class BakRules
    {
        public static IResult IsBakExtensionRight(string fileName)
        {
            if (fileName.ToLower().EndsWith(".bak"))
            {
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
