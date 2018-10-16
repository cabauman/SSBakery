namespace SSBakeryAdmin.UI.Modules
{
    public class CatalogItemCellViewModel : ICatalogItemCellViewModel
    {
        private readonly SSBakery.Models.CatalogItem _model;

        public CatalogItemCellViewModel(SSBakery.Models.CatalogItem model)
        {
            _model = model;
        }

        public string Id => _model.Id;

        public string Name => _model.Name;
    }
}
