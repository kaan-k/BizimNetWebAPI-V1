using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class EmployeeAccessToken
    {
        public int EmployeeId { get; set; }
        public string Token { get; set; }
        public string Expiration { get; set; }
    }
}
