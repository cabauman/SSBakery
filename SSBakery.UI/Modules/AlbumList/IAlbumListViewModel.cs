using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface IAlbumListViewModel
    {
        ReactiveCommand<Unit, List<AlbumCellViewModel>> LoadAlbums { get; }

        List<AlbumCellViewModel> Albums { get; }

        AlbumCellViewModel SelectedItem { get; set; }
    }
}