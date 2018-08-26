using System.Collections.Generic;
using System.IO;
using System.Reactive;
using Plugin.Media.Abstractions;
using ReactiveUI;

namespace SSBakeryAdmin.iOS.Modules.CatalogItemDetails
{
    public interface ICatalogItemDetailsViewModel
    {
        ReactiveCommand<Unit, MediaFile> TakePhoto { get; }

        ReactiveCommand<Unit, MediaFile> PickPhoto { get; }

        MediaFile MediaFile { get; }

        Stream PhotoStream { get; }
    }
}