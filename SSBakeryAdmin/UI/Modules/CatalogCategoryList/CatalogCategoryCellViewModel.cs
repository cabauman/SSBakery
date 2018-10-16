using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public class CatalogCategoryCellViewModel : ICatalogCategoryCellViewModel
    {
        private readonly CatalogCategory _catalogCategory;

        public CatalogCategoryCellViewModel(CatalogCategory catalogCategory)
        {
            _catalogCategory = catalogCategory;
        }

        public string CateogryId => _catalogCategory.Id;

        public string Name => _catalogCategory.Name;
    }
}
