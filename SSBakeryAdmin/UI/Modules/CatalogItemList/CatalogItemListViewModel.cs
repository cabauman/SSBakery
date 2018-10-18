using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using SSBakery.Repositories.Interfaces;

namespace SSBakeryAdmin.UI.Modules
{
    public class CatalogItemListViewModel : ReactiveObject, ICatalogItemListViewModel, IPageViewModel
    {
        private readonly ObservableAsPropertyHelper<IReadOnlyList<ICatalogItemCellViewModel>> _items;

        public CatalogItemListViewModel(
            string categoryId,
            ICatalogItemRepoFactory itemRepoFactory = null,
            IViewStackService viewStackService = null)
        {
            itemRepoFactory = itemRepoFactory ?? Locator.Current.GetService<ICatalogItemRepoFactory>();
            viewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();

            var itemRepo = itemRepoFactory.Create(categoryId);

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

            var firebaseStorageService = Locator.Current.GetService<GameCtor.FirebaseStorage.DotNet.IFirebaseStorageService>();
            DownloadImages = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return Items
                        .ToObservable()
                        .SelectMany(
                            itemCell =>
                            {
                                return firebaseStorageService
                                    .GetDownloadUrl("catalogPhotos/" + itemCell.Id + ".jpg")
                                    .Catch<string, Exception>(ex => Observable.Return<string>(null))
                                    .Where(x => x != null)
                                    .Do(imageUrl => itemCell.ImageUrl = imageUrl)
                                    .Select(_ => itemCell);
                            })
                        .SelectMany(
                            itemCell =>
                            {
                                return itemRepo
                                    .Upsert(itemCell.Model);
                            });
                });
        }

        public ReactiveCommand<Unit, IReadOnlyList<ICatalogItemCellViewModel>> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> DownloadImages { get; }

        public IReadOnlyList<ICatalogItemCellViewModel> Items => _items.Value;

        public string Title => "Catalog Items";
    }
}
