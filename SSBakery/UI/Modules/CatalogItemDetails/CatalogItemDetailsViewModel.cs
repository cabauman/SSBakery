using GameCtor.RxNavigation;
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

        public CatalogItem Model => _model;

        public string Id => _model.Id;

        public string Name => _model.Name;

        public string Description => _model.Description;

        public string Price => _model.Price;

        public string ImageUrl => _model.ImageUrl;
    }
}
