using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using ReactiveUI;
using Splat;
using Square.Connect.Model;
using SSBakery;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class CatalogItemDetailsViewModel : ViewModelBase
    {
        private readonly CatalogObject _model;

        public CatalogItemDetailsViewModel(CatalogObject model, IScreen hostScreen = null)
            : base(hostScreen)
        {
            _model = model;
        }

        public CatalogObject CatalogObject
        {
            get { return _model; }
        }
    }
}
