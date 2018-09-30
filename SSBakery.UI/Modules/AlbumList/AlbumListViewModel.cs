using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class AlbumListViewModel : PageViewModel, IAlbumListViewModel
    {
        private ObservableAsPropertyHelper<List<AlbumCellViewModel>> _albums;
        private AlbumCellViewModel _selectedItem;

        public AlbumListViewModel(IFacebookPhotoService photoService = null, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            photoService = photoService ?? Locator.Current.GetService<IFacebookPhotoService>();

            LoadAlbums = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    var albumData = await photoService.GetAlbumsAsync(Config.Constants.FACEBOOK_PAGE_ID, Config.Constants.FACEBOOK_PAGE_ACCESS_TOKEN, default(CancellationToken));

                    var viewModels = new List<AlbumCellViewModel>(albumData.Data.Count);
                    foreach(var model in albumData.Data)
                    {
                        var vm = new AlbumCellViewModel(model);
                        viewModels.Add(vm);
                    }

                    return viewModels;
                });

            _albums = LoadAlbums
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ToProperty(this, vm => vm.Albums, scheduler: RxApp.MainThreadScheduler);

            this.WhenActivated(
                disposables =>
                {
                    SelectedItem = null;

                    this
                        .WhenAnyValue(vm => vm.SelectedItem)
                        .Where(x => x != null)
                        .SelectMany(x => LoadSelectedPage(x))
                        .Subscribe()
                        .DisposeWith(disposables);

                    LoadAlbums
                        .ThrownExceptions
                        .Subscribe(ex => this.Log().WarnException("Failed to load albums", ex))
                        .DisposeWith(disposables);
                });
        }

        public ReactiveCommand<Unit, List<AlbumCellViewModel>> LoadAlbums { get; set; }

        public List<AlbumCellViewModel> Albums
        {
            get { return _albums.Value; }
        }

        public AlbumCellViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { this.RaiseAndSetIfChanged(ref _selectedItem, value); }
        }

        private IObservable<Unit> LoadSelectedPage(AlbumCellViewModel viewModel)
        {
            return ViewStackService.PushPage(new AlbumViewModel(viewModel.FacebookAlbum.Id));
        }
    }
}
