using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public class CatalogCategoryCellViewModel : ICatalogCategoryCellViewModel
    {
        private readonly SSBakery.Models.CatalogCategory _catalogCategory;

        public CatalogCategoryCellViewModel(SSBakery.Models.CatalogCategory catalogCategory)
        {
            _catalogCategory = catalogCategory;
        }

        public string CateogryId
        {
            get { return _catalogCategory.Id; }
        }

        public string Name
        {
            get { return _catalogCategory.Name; }
        }
    }
}