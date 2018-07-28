using System;

namespace SSBakery.Models
{
    public class CatalogItem : IModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
