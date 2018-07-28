﻿using Square.Connect.Model;
using SSBakery.UI.Common;
using SSBakery.UI.Navigation.Interfaces;

namespace SSBakery.UI.Modules
{
    public class CatalogCategoryCellViewModel : ViewModelBase, ICatalogCategoryCellViewModel
    {
        private CatalogObject _model;

        public CatalogCategoryCellViewModel(CatalogObject model, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            _model = model;
        }

        public CatalogObject CatalogObject
        {
            get { return _model; }
        }

        public string Id
        {
            get { return _model.Id; }
        }

        public string Name
        {
            get { return _model.ItemData.Name; }
        }

        public string ImageUrl
        {
            get { return _model.ItemData.ImageUrl; }
        }
    }
}