using System;

namespace SSBakery.Models
{
    public class CatalogCategory : IModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}
