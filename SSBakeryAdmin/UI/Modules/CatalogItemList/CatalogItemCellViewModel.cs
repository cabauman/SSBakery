using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public class CatalogItemCellViewModel : ICatalogItemCellViewModel
    {
        private readonly CatalogItem _model;

        public CatalogItemCellViewModel(CatalogItem model)
        {
            _model = model;
        }

        public CatalogItem Model => _model;

        public string Id => _model.Id;

        public string Name => _model.Name;

        public string Description => _model.Description;

        public string Price => _model.Price;

        public string ImageUrl
        {
            get => _model.ImageUrl; // ?? "Images/Stamp.png";
            set => _model.ImageUrl = value;
        }
    }
}
