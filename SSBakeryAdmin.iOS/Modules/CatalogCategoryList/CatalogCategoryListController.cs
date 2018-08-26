using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive;
using System.Linq;

namespace SSBakeryAdmin.iOS.Modules.CatalogCategoryList
{
    [Register("CatalogCategoryListController")]
    public class CatalogCategoryListController : ReactiveTableViewController<ICatalogCategoryListViewModel>
    {
        private static readonly string HeaderId = "HeaderId";
        private static readonly string CellId = "CellId";

        private UIButton _syncCatalogButton;

        public CatalogCategoryListController()
        {
            TableView.RegisterClassForCellReuse(typeof(UITableViewCell), CellId);

            this
                .WhenAnyValue(x => ViewModel)
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel, x => x.LoadCategories);
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view

            _syncCatalogButton = new UIButton(UIButtonType.RoundedRect);
            _syncCatalogButton.SetTitle("Refresh Catalog", UIControlState.Normal);
            _syncCatalogButton.Font = _syncCatalogButton.Font.WithSize(28);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return ViewModel != null ? ViewModel.Categories.Count : 0;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellId, indexPath);
            var category = ViewModel.Categories[indexPath.Row];

            cell.TextLabel.Text = category.Name;

            return cell;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            UITableViewHeaderFooterView header = tableView.DequeueReusableHeaderFooterView(HeaderId);

            _syncCatalogButton = new UIButton(UIButtonType.RoundedRect);
            _syncCatalogButton.SetTitle("Refresh Catalog", UIControlState.Normal);
            _syncCatalogButton.Font = _syncCatalogButton.Font.WithSize(28);
            return _syncCatalogButton;


            // TODO: Add custom header with _syncCatalogButton

            //return header;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            ViewModel.NavigateToCategory.Execute(ViewModel.Categories[indexPath.Row]).Subscribe();
        }
    }
}