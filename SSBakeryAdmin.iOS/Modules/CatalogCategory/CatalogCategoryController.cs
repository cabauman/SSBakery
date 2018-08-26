using System;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using CoreFoundation;
using Foundation;
using ReactiveUI;
using SSBakeryAdmin.iOS.Modules.CatalogCategory;
using UIKit;

namespace SSBakeryAdmin.iOS.Modules.CatalogCategoryList
{
    [Register("CatalogCategoryController")]
    public class CatalogCategoryController : ReactiveCollectionViewController<ICatalogCategoryViewModel>
    {
        private static readonly string CellReuseId = "CellReuseId";

        public CatalogCategoryController()
        {
            CollectionView.RegisterClassForCell(typeof(CatalogItemCell), CellReuseId);

            this
                .WhenAnyValue(x => ViewModel)
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel, x => x.LoadItems);
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
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (CatalogItemCell)collectionView.DequeueReusableCell(CellReuseId, indexPath);
            var item = ViewModel.Items[indexPath.Row];

            cell.TitleLabel.Text = item.Name;

            return cell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            base.ItemSelected(collectionView, indexPath);
        }
    }
}