using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.UI.Modules
{
    public interface ICatalogCategoryViewModel
    {
        ReactiveCommand<Unit, IReadOnlyList<ICatalogItemCellViewModel>> LoadItems { get; }

        ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        IReadOnlyList<ICatalogItemCellViewModel> Items { get; }
    }
}