﻿using System;
using System.Collections.Generic;
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
            ICatalogItemRepo itemRepo = null,
            IViewStackService viewStackService = null)
        {
            itemRepo = itemRepo ?? Locator.Current.GetService<ICatalogItemRepo>();
            viewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();

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

        public IReadOnlyList<ICatalogItemCellViewModel> Items => _items.Value;

        public string Title => "Catalog Items";
    }
}
