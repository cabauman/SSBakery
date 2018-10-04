using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface IAlbumCellViewModel
    {
        string Name { get; }

        int Count { get; }
    }
}