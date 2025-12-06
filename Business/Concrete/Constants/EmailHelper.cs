using Entities.Concrete.Customers;
using Entities.Concrete.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete.Constants
{
    public class EmailHelper
    {
        public static EmailConfiguration config(string customerEmail)
        {

            return new EmailConfiguration()
                {
                SmtpServer = "smtp.gmail.com",
                Port = 587,
                From = "kaannkale@gmail.com",
                Username = "kaannkale@gmail.com",
                Password = "pkho hrxk adwx oxkf ",
                To = new List<string> { customerEmail }
            };
        } 
    }
}
