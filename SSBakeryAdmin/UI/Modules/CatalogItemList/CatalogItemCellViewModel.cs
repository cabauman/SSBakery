namespace SSBakeryAdmin.UI.Modules
{
    public class CatalogItemCellViewModel : ICatalogItemCellViewModel
    {
        private readonly SSBakery.Models.CatalogItem _catalogItem;

        public CatalogItemCellViewModel(SSBakery.Models.CatalogItem catalogItem)
        {
            _catalogItem = catalogItem;
        }

        public string ItemId
        {
            get { return _catalogItem.Id; }
        }

        public string Name
        {
            get { return _catalogItem.Name; }
        }
    }
}