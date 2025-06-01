using Entities.Concrete.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMailManager
    {
        void SendMail(EmailConfiguration config, EMailContent content);
    }

}
