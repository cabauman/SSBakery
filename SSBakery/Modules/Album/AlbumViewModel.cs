using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using Splat;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;
using SSBakery.UI.Navigation.Interfaces;

namespace SSBakery.UI.Modules
{
    public class AlbumViewModel : PageViewModel, IAlbumViewModel
    {
        private ObservableAsPropertyHelper<IList<PhotoCellViewModel>> _photos;
        private PhotoCellViewModel _selectedItem;

        public AlbumViewModel(string albumId, IFacebookPhotoService photoService = null, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            photoService = photoService ?? Locator.Current.GetService<IFacebookPhotoService>();

            LoadPhotos = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    var photos = await photoService.GetAlbumPhotos(albumId, Config.Constants.FACEBOOK_PAGE_ACCESS_TOKEN);

                    var viewModels = new List<PhotoCellViewModel>(photos.Count);
                    foreach(var model in photos)
                    {
                        var vm = new PhotoCellViewModel(model);
                        viewModels.Add(vm);
                    }

                    return viewModels;
                });

            _photos = LoadPhotos.ToProperty(this, x => x.Photos);
        }

        public ReactiveCommand<Unit, List<PhotoCellViewModel>> LoadPhotos { get; set; }

        public IList<PhotoCellViewModel> Photos
        {
            get { return _photos.Value; }
        }

        public PhotoCellViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { this.RaiseAndSetIfChanged(ref _selectedItem, value); }
        }
    }
}
