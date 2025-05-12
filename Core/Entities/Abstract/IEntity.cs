using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Abstract
{
    public interface IEntity
    {
        public string Id { get; set; }
    }
}
