using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface IAlbumViewModel
    {
        ReactiveCommand<Unit, List<PhotoCellViewModel>> LoadPhotos { get; }

        PhotoCellViewModel SelectedItem { get; set; }

        IList<PhotoCellViewModel> Photos { get; }
    }
}
