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

        public CatalogItem Model
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

        public string Description
        {
            get { return _model.Description; }
        }

        public string Price
        {
            get { return _model.Price; }
        }

        public string ImageUrl
        {
            get { return _model.ImageUrl; }
        }
    }
}
