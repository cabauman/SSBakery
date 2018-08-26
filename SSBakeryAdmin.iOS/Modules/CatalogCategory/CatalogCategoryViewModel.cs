using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Xml;
using ReactiveUI;
using RxNavigation;
using Splat;
using SSBakery.Repositories.Interfaces;

namespace SSBakeryAdmin.iOS.Modules.CatalogCategory
{
    public class CatalogCategoryViewModel : ReactiveObject, ICatalogCategoryViewModel, IPageViewModel
    {
        private readonly ObservableAsPropertyHelper<IReadOnlyList<ICatalogItemCellViewModel>> _items;

        private string _timestampOfLatestSync;

        public CatalogCategoryViewModel(string categoryId, ICatalogItemRepo itemRepo = null)
        {
            itemRepo = itemRepo ?? Locator.Current.GetService<ICatalogItemRepo>();

            SyncWithPosSystem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return itemRepo
                        .PullFromPosSystemAndStoreInFirebase(categoryId, _timestampOfLatestSync)
                        .Do(_ => SaveTimestampOfLatestSync());
                });

            LoadItems = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return itemRepo
                        .GetItems()
                        .SelectMany(x => x)
                        .Select(x => new CatalogItemCellViewModel(x))
                        .ToList()
                        .Select(x => x as IReadOnlyList<ICatalogItemCellViewModel>);
                });

            _items = LoadItems.ToProperty(this, x => x.Items);
        }

        public ReactiveCommand<Unit, IReadOnlyList<ICatalogItemCellViewModel>> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        public IReadOnlyList<ICatalogItemCellViewModel> Items => _items.Value;

        public string Title => "Catalog Items";

        private void SaveTimestampOfLatestSync()
        {
            _timestampOfLatestSync = XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc);
        }
    }
}