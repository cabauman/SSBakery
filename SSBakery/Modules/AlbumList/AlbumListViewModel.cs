using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using SSBakery;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class AlbumListViewModel : ViewModelBase, IAlbumListViewModel
    {
        private ObservableAsPropertyHelper<List<AlbumCellViewModel>> _albums;
        private AlbumCellViewModel _selectedItem;

        public AlbumListViewModel(IFacebookPhotoService photoService = null, IScreen hostScreen = null)
            : base(hostScreen)
        {
            photoService = photoService ?? Locator.Current.GetService<IFacebookPhotoService>();

            LoadAlbums = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    var albumData = await photoService.GetAlbumsAsync(Config.Constants.FACEBOOK_PAGE_ID, Config.Constants.FACEBOOK_PAGE_ACCESS_TOKEN);

                    var viewModels = new List<AlbumCellViewModel>(albumData.Data.Count);
                    foreach(var model in albumData.Data)
                    {
                        var vm = new AlbumCellViewModel(model);
                        viewModels.Add(vm);
                    }

                    return viewModels;
                });

            _albums = LoadAlbums.ToProperty(this, vm => vm.Albums);

            this.WhenActivated(
                disposables =>
                {
                    SelectedItem = null;

                    this
                        .WhenAnyValue(vm => vm.SelectedItem)
                        .Where(x => x != null)
                        .Subscribe(
                            x => LoadSelectedPage(x),
                            ex =>
                            {
                                this.Log().Debug(ex.Message);
                            })
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

        private void LoadSelectedPage(AlbumCellViewModel viewModel)
        {
            HostScreen
                .Router
                .Navigate
                .Execute(new AlbumViewModel(viewModel.FacebookAlbum.Id))
                .Subscribe();
        }
    }
}
