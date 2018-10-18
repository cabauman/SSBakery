using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public class CatalogCategoryCellViewModel : ICatalogCategoryCellViewModel
    {
        private readonly CatalogCategory _model;

        public CatalogCategoryCellViewModel(CatalogCategory model)
        {
            _model = model;
        }

        public string Id => _model.Id;

        public string Name => _model.Name;

        public string ImageUrl
        {
            get => _model.ImageUrl ?? @"Images/Stamp.png";
            set => _model.ImageUrl = value;
        }
    }
}
