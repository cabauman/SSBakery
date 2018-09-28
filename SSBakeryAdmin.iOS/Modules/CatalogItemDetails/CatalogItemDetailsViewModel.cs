using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using GameCtor.FirebaseStorage.DotNet;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ReactiveUI;
using GameCtor.RxNavigation;
using Splat;

namespace SSBakeryAdmin.iOS.Modules.CatalogItemDetails
{
    public class CatalogItemDetailsViewModel : ReactiveObject, ICatalogItemDetailsViewModel, IPageViewModel
    {
        private SSBakery.Models.CatalogItem _catalogItem;
        private ObservableAsPropertyHelper<MediaFile> _mediaFile;
        private ObservableAsPropertyHelper<IBitmap> _photo;
        private ObservableAsPropertyHelper<Stream> _photoStream;

        public CatalogItemDetailsViewModel(SSBakery.Models.CatalogItem catalogItem, IFirebaseStorageService storageService)
        {
            _catalogItem = catalogItem;
            storageService = storageService ?? Locator.Current.GetService<IFirebaseStorageService>();

            TakePhoto = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        //Directory = "Sample",
                        Name = $"{_catalogItem.Id}.jpg",
                        AllowCropping = true,
                        CompressionQuality = 92,
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                        //PhotoSize = PhotoSize.Custom,
                        //CustomPhotoSize = 90 //Resize to 90% of original
                    };

                    return CrossMedia.Current
                        .TakePhotoAsync(mediaOptions)
                        .ToObservable();
                });

            PickPhoto = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    var mediaOptions = new PickMediaOptions
                    {
                        CompressionQuality = 92,
                        PhotoSize = PhotoSize.Small,
                        SaveMetaData = true,
                    };

                    return CrossMedia.Current
                        .PickPhotoAsync(mediaOptions)
                        .ToObservable();
                });

            _mediaFile = Observable
                .Merge(TakePhoto, PickPhoto)
                .Where(file => file != null)
                .ToProperty(this, x => x.MediaFile);

            _photoStream = this
                .WhenAnyValue(x => x.MediaFile)
                .Select(file => file.GetStream())
                .SelectMany(
                    stream =>
                    {
                        return storageService
                            .Upload($"CatalogPhotos/{_catalogItem.Id}.jpg", stream)
                            .ToObservable()
                            .Select(_ => stream);
                    })
                .ToProperty(this, x => x.PhotoStream);

            this
                .WhenAnyValue(x => x.PhotoStream)
                .Do(_ => MediaFile.Dispose());
        }

        public ReactiveCommand<Unit, MediaFile> TakePhoto { get; }

        public ReactiveCommand<Unit, MediaFile> PickPhoto { get; }

        public string Title => "ItemDetails";

        public MediaFile MediaFile => _mediaFile.Value;

        public IBitmap Photo => _photo.Value;

        public Stream PhotoStream => _photoStream.Value;
    }
}