using GameCtor.RxNavigation;
using SSBakery.Models;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class CatalogItemCellViewModel : ViewModelBase, ICatalogItemCellViewModel
    {
        private CatalogItem _model;

        public CatalogItemCellViewModel(CatalogItem model, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            _model = model;
        }

        public CatalogItem CatalogItem => _model;

        public string Id => _model.Id;

        public string Name => _model.Name;

        public string Description => _model.Description;

        public string Price
        {
            get { return _model.Price; }
        }

        //public string Price1
        //{
        //    get
        //    {
        //        long? unformattedAmount = _model.ItemData?.Variations?[0]?.ItemVariationData?.PriceMoney?.Amount;
        //        return unformattedAmount != null ? string.Format("{0:C}", unformattedAmount / 100) : null;
        //    }
        //}

        public string ImageUrl => _model.ImageUrl;
    }
}
