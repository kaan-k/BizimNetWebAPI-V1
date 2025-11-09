using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Settings
{
    public class AgGridSettingsDto:IDto
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
