﻿using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class EmployeeDto:IDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public string DeparmentId { get; set; }
        public string Role { get; set; }
    }
}
