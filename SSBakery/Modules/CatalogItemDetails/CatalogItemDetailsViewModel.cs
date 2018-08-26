using RxNavigation;
using SSBakery.Models;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class CatalogItemDetailsViewModel : PageViewModel, ICatalogItemDetailsViewModel
    {
        private readonly CatalogItem _model;

        public CatalogItemDetailsViewModel(CatalogItem model, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            _model = model;
        }

        public CatalogItem CatalogItem
        {
            get { return _model; }
        }
    }
}
