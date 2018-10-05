using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;

namespace SSBakeryAdmin.UI.Modules
{
    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        private MasterCellViewModel _selected;

        public MainViewModel(IViewStackService viewStackService = null)
        {
            viewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();

            MenuItems = GetMenuItems();

            NavigateToMenuItem = ReactiveCommand.CreateFromObservable<IPageViewModel, Unit>(
                pageVm => viewStackService.PushPage(pageVm, resetStack: true));

            this.WhenAnyValue(x => x.Selected)
                .Where(x => x != null)
                .StartWith(MenuItems.First())
                .Select(x => Locator.Current.GetService<IPageViewModel>(x.TargetType.FullName))
                .InvokeCommand(NavigateToMenuItem);
        }

        public string Title => "Home";

        public MasterCellViewModel Selected
        {
            get => _selected;
            set => this.RaiseAndSetIfChanged(ref _selected, value);
        }

        public ReactiveCommand<IPageViewModel, Unit> NavigateToMenuItem { get; }

        public IEnumerable<MasterCellViewModel> MenuItems { get; }

        private IEnumerable<MasterCellViewModel> GetMenuItems()
        {
            throw new NotImplementedException();
            //return new[]
            //{
            //    new MasterCellViewModel { Title = "Customer Directory", IconSource = "contacts.png", TargetType = typeof(CustomerDirectoryPage) },
            //    new MasterCellViewModel { Title = "Catalog", IconSource = "reminders.png", TargetType = typeof(CatalogCategoryListPage) },
            //};
        }
    }
}
