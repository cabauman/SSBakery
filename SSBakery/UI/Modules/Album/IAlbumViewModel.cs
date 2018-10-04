using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface IAlbumViewModel
    {
        ReactiveCommand<Unit, List<PhotoCellViewModel>> LoadPhotos { get; }

        IList<PhotoCellViewModel> Photos { get; }
    }
}
