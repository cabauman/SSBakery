using Square.Connect.Model;
using SSBakery.UI.Common;
using SSBakery.UI.Navigation.Interfaces;

namespace SSBakery.UI.Modules
{
    public class CatalogItemDetailsViewModel : PageViewModel, ICatalogItemDetailsViewModel
    {
        private readonly CatalogObject _model;

        public CatalogItemDetailsViewModel(CatalogObject model, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            _model = model;
        }

        public CatalogObject CatalogObject
        {
            get { return _model; }
        }
    }
}
