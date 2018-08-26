using RxNavigation;
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

        public CatalogCategory Model
        {
            get { return _model; }
        }

        public string Id
        {
            get { return _model.Id; }
        }

        public string Name
        {
            get { return _model.Name; }
        }

        public string ImageUrl
        {
            get { return _model.ImageUrl; }
        }
    }
}
