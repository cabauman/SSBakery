﻿using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface ICatalogCategoryListViewModel
    {
        ReactiveCommand<Unit, IReadOnlyList<ICatalogCategoryCellViewModel>> LoadCatalogCategories { get; }

        IReadOnlyList<ICatalogCategoryCellViewModel> CatalogCategories { get; }

        ICatalogCategoryCellViewModel SelectedItem { get; set; }
    }
}