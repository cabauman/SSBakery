using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using SSBakery.Repositories.Interfaces;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class CatalogCategoryViewModel : PageViewModel, ICatalogCategoryViewModel
    {
        private ObservableAsPropertyHelper<bool> _isRefreshing;
        private ICatalogItemCellViewModel _selectedItem;
        private ICatalogItemCellViewModel _itemAppearing;
        private int _cursor = 0;

        public CatalogCategoryViewModel(
            string categoryId,
            ICatalogItemRepoFactory catalogItemRepoFactory = null,
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
            catalogItemRepoFactory = catalogItemRepoFactory ?? Locator.Current.GetService<ICatalogItemRepoFactory>();
            ICatalogItemRepo catalogItemRepo = catalogItemRepoFactory.Create(categoryId);
            CatalogItems = new List<ICatalogItemCellViewModel>();

            LoadCatalogItems = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return catalogItemRepo.GetItems()
                        .SelectMany(x => x)
                        .Select(x => new CatalogItemCellViewModel(x) as ICatalogItemCellViewModel)
                        .Do(x => CatalogItems.Add(x));
                });

            LoadCatalogItems
                .ThrownExceptions
                .Subscribe(
                    ex =>
                    {
                        this.Log().Debug(ex.Message);
                    });

            this
                .WhenAnyValue(vm => vm.SelectedItem)
                .Where(x => x != null)
                .SelectMany(x => LoadSelectedPage(x))
                .Subscribe();

            //this
            //    .WhenAnyValue(vm => vm.ItemAppearing)
            //    .Where(item => item != null && item.Id == CatalogItems[CatalogItems.Count - 1].Id)
            //    .Select(_ => Unit.Default)
            //    .InvokeCommand(LoadCatalogItems)
            //    .DisposeWith(disposables);

            //_isRefreshing = LoadCatalogItems
            //    .IsExecuting
            //    .ToProperty(this, vm => vm.IsRefreshing, true);
        }

        public IList<ICatalogItemCellViewModel> CatalogItems { get; }

        //public bool IsRefreshing => _isRefreshing.Value;

        public ReactiveCommand<Unit, ICatalogItemCellViewModel> LoadCatalogItems { get; }

        public ICatalogItemCellViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { this.RaiseAndSetIfChanged(ref _selectedItem, value); }
        }

        //public ICatalogItemCellViewModel ItemAppearing
        //{
        //    get { return _itemAppearing; }
        //    set { this.RaiseAndSetIfChanged(ref _itemAppearing, value); }
        //}

        private IObservable<Unit> LoadSelectedPage(ICatalogItemCellViewModel viewModel)
        {
            return ViewStackService.PushPage(new CatalogItemDetailsViewModel(viewModel.CatalogItem));
        }
    }
}
