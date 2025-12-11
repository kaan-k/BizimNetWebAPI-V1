using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Tables
{
    public enum TableStatus
    {
        Free = 0,
        Busy = 1,      // Occupied / Dolu
        Reserved = 2   // Rezerve
    }
}
