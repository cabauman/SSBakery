using System;
using System.Reactive.Disposables;
using ReactiveUI;
using Splat;
using Square.Connect.Model;
using SSBakery;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class CatalogItemCellViewModel : ViewModelBase
    {
        private CatalogObject _model;

        public CatalogItemCellViewModel(CatalogObject model, IScreen hostScreen = null)
            : base(hostScreen)
        {
            _model = model;
        }

        public CatalogObject CatalogObject
        {
            get { return _model; }
        }

        public string Name
        {
            get { return _model.ItemData.Name; }
        }

        public string Description
        {
            get { return _model.ItemData.Description; }
        }

        public string Price
        {
            get
            {
                long? unformattedAmount = _model.ItemData?.Variations?[0]?.ItemVariationData?.PriceMoney?.Amount;
                return unformattedAmount != null ? string.Format("{0:C}", unformattedAmount) : null;
            }
        }

        public string ImageUrl
        {
            get { return _model.ItemData.ImageUrl; }
        }
    }
}
