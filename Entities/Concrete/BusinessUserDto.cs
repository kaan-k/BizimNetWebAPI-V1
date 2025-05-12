using Core.Entities.Abstract;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class BusinessUserDto:IDto
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyAddress { get; set; }
    }
}
