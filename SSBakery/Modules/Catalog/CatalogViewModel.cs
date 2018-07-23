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
using SSBakery.UI.Navigation.Interfaces;

namespace SSBakery.UI.Modules
{
    public class CatalogViewModel : PageViewModel, ICatalogViewModel
    {
        private ICatalogItemCellViewModel _selectedItem;
        private ObservableAsPropertyHelper<IEnumerable<ICatalogItemCellViewModel>> _catalogItems;
        private ObservableAsPropertyHelper<bool> _isRefreshing;

        public CatalogViewModel(IRepoContainer dataStore = null, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            RepoContainer = dataStore ?? Locator.Current.GetService<IRepoContainer>();

            LoadCatalogItems = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return RepoContainer.CatalogObjectRepo.GetAll()
                        .Where(items => items != null)
                        .SelectMany(x => x)
                        .Where(x => x.Type == CatalogObject.TypeEnum.ITEM)
                        .Select(item => new CatalogItemCellViewModel(item) as ICatalogItemCellViewModel)
                        .ToList()
                        .Select(x => x.AsEnumerable());
                });

            LoadCatalogItems
                .ToProperty(this, x => x.CatalogItems, out _catalogItems);

            this.WhenActivated(
                disposables =>
                {
                    SelectedItem = null;

                    LoadCatalogItems
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
                        .SelectMany(x => LoadSelectedPage(x))
                        .Subscribe()
                        .DisposeWith(disposables);
                });
        }

        public IEnumerable<ICatalogItemCellViewModel> CatalogItems => _catalogItems?.Value;

        public ReactiveCommand<Unit, IEnumerable<ICatalogItemCellViewModel>> LoadCatalogItems { get; }

        public IRepoContainer RepoContainer { get; }

        public ICatalogItemCellViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { this.RaiseAndSetIfChanged(ref _selectedItem, value); }
        }

        public bool IsRefreshing => _isRefreshing.Value;

        private IObservable<Unit> LoadSelectedPage(ICatalogItemCellViewModel viewModel)
        {
            return ViewStackService.PushPage(new CatalogItemDetailsViewModel(viewModel.CatalogObject));
        }
    }
}
