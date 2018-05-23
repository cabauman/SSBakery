using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using Square.Connect.Model;
using SSBakery;
using SSBakery.Repositories.Interfaces;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class CatalogViewModel : ViewModelBase
    {
        private CatalogItemCellViewModel _selectedItem;
        private ObservableAsPropertyHelper<bool> _isRefreshing;

        public CatalogViewModel(IDataStore dataStore = null, IScreen hostScreen = null)
            : base(hostScreen)
        {
            UrlPathSegment = "Catalog Items";

            DataStore = dataStore ?? Locator.Current.GetService<IDataStore>();

            LoadCatalogObjects = ReactiveCommand.CreateFromObservable<Unit, IEnumerable<CatalogObject>>(
                _ =>
                {
                    return DataStore.CatalogObjectRepo.GetAll();
                });

            this.WhenActivated(
                disposables =>
                {
                    SelectedItem = null;

                    LoadCatalogObjects
                        .Where(items => items != null)
                        .SelectMany(x => x)
                        .Where(x => x.Type == CatalogObject.TypeEnum.ITEM)
                        .Select(item => new CatalogItemCellViewModel(item))
                        .Log(this, "Adding catalog item view model")
                        .Subscribe(
                            itemViewModel => CatalogItems.Add(itemViewModel),
                            ex =>
                            {
                                this.Log().Debug(ex.Message);
                            })
                        .DisposeWith(disposables);

                    LoadCatalogObjects
                        .ThrownExceptions
                        .Subscribe(
                            ex =>
                            {
                                this.Log().Debug(ex.Message);
                            })
                        .DisposeWith(disposables);

                    this
                        .WhenAnyValue(x => x.SelectedItem)
                        .Where(x => x != null)
                        .Subscribe(
                            x => LoadSelectedPage(x),
                            ex =>
                            {
                                this.Log().Debug(ex.Message);
                            })
                        .DisposeWith(disposables);
                });
        }

        public ReactiveList<CatalogItemCellViewModel> CatalogItems { get; } = new ReactiveList<CatalogItemCellViewModel>();

        public ReactiveCommand<Unit, IEnumerable<CatalogObject>> LoadCatalogObjects { get; }

        public IDataStore DataStore { get; }

        public CatalogItemCellViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { this.RaiseAndSetIfChanged(ref _selectedItem, value); }
        }

        public bool IsRefreshing => _isRefreshing.Value;

        private void LoadSelectedPage(CatalogItemCellViewModel viewModel)
        {
            HostScreen
                .Router
                .Navigate
                .Execute(new CatalogItemDetailsViewModel(viewModel.CatalogObject))
                .Subscribe();
        }
    }
}
