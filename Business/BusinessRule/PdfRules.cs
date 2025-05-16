using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.BusinessRule
{
    public static class PdfRules
    {
        public static IResult IsPdfExtensionRight(string fileName)
        {
            if (fileName.ToLower().EndsWith(".pdf"))
            {
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
