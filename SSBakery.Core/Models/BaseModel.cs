using System;
using GameCtor.Repository;
using Newtonsoft.Json;

namespace SSBakery.Models
{
    public class BaseModel : IModel
    {
        [JsonIgnore]
        public string Id { get; set; }
    }
}
