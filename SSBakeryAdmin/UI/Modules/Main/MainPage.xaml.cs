using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using GameCtor.RxNavigation;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;

namespace SSBakeryAdmin.UI.Modules
{
    public partial class MainPage : ReactiveMasterDetailPage<MainViewModel>
    {
        public MainPage(IView detailView)
        {
            Detail = (Xamarin.Forms.Page)detailView;

            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.MenuItems, v => v.MyListView.ItemsSource)
                        .DisposeWith(disposables);
                    this
                        .Bind(ViewModel, vm => vm.Selected, v => v.MyListView.SelectedItem)
                        .DisposeWith(disposables);
                    this
                        .WhenAnyValue(x => x.ViewModel.Selected)
                        .Where(x => x != null)
                        .Subscribe(
                            _ =>
                            {
                                // Deselect the cell.
                                Device.BeginInvokeOnMainThread(() => MyListView.SelectedItem = null);
                                // Hide the master panel.
                                IsPresented = false;
                            })
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.SignOut, v => v.SignOutButton)
                        .DisposeWith(disposables);
                });
        }
    }
}
