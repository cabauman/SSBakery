using System;
using System.Reactive.Linq;
using GameCtor.Repository;
using ReactiveUI;
using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public class CatalogCategoryCellViewModel : ReactiveObject, ICatalogCategoryCellViewModel
    {
        private readonly CatalogCategory _model;

        private bool _visibleToUsers;

        public CatalogCategoryCellViewModel(CatalogCategory model, IRepository<CatalogCategory> categoryRepo)
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
                        return categoryRepo.Upsert(_model);
                    })
                .Subscribe();
        }

        public string Id => _model.Id;

        public string Name => _model.Name;

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
