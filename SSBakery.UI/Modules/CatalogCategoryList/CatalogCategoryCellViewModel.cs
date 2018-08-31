using GameCtor.RxNavigation;
using SSBakery.Models;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class CatalogCategoryCellViewModel : ViewModelBase, ICatalogCategoryCellViewModel
    {
        private CatalogCategory _model;

        public CatalogCategoryCellViewModel(CatalogCategory model, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            _model = model;
        }

        public CatalogCategory Model => _model;

        public string Id => _model.Id;

        public string Name => _model.Name;

        public string ImageUrl => _model.ImageUrl;
    }
}
