﻿using Core.Entities.Abstract;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Stock
{
    public class StockAddDto:IDto
    {
        public DeviceType DeviceType { get; set; }
    }
}
