using System;
using System.Reactive.Linq;
using GameCtor.Repository;
using ReactiveUI;
using Splat;
using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public class CatalogItemCellViewModel : ReactiveObject, ICatalogItemCellViewModel
    {
        private readonly CatalogItem _model;

        private bool _visibleToUsers;

        public CatalogItemCellViewModel(CatalogItem model, IRepository<CatalogItem> itemRepo)
        {
            _model = model;
            _visibleToUsers = model.VisibleToUsers;

            this
                .WhenAnyValue(x => x.VisibleToUsers)
                .Skip(1)
                .Do(x => _model.VisibleToUsers = x)
                .SelectMany(
                    _ =>
                    {
                        return itemRepo.Upsert(_model);
                    })
                .Subscribe();
        }

        public CatalogItem Model => _model;

        public string Id => _model.Id;

        public string Name => _model.Name;

        public string Description => _model.Description;

        public string Price => _model.Price;

        public string ImageUrl
        {
            get => _model.ImageUrl ?? @"Images/Stamp.png";
            set => _model.ImageUrl = value;
        }

        public bool VisibleToUsers
        {
            get => _visibleToUsers;
            set => this.RaiseAndSetIfChanged(ref _visibleToUsers, value);
        }
    }
}
