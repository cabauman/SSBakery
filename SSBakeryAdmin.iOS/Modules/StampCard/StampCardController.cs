using System;
using CoreGraphics;
using Foundation;
using ReactiveUI;
using SSBakeryAdmin.iOS.Helpers;
using SSBakeryAdmin.iOS.Modules.StampCard;
using SSBakeryAdmin.iOS.Views;
using UIKit;

namespace SSBakeryAdmin.iOS.Controllers
{
    public partial class StampCardController : ReactiveCollectionViewController<IStampCardViewModel>
    {
        private const string CELL_ID = "CELL_ID";

        private LoadingOverlay _loadingOverlay;

        public StampCardController(UICollectionViewLayout layout)
            : base(layout)
        {
            Title = "Stamp Card";
        }

        public static UICollectionViewLayout GetLayout()
        {
            var headerHeight = 0f;
            var insets = new UIEdgeInsets(2, 0, 0, 0);
            var interitemSpacing = 2f;
            var lineSpacing = 2f;
            var numRows = 5;
            var numCols = 2;
            var itemWidth = ViewHelper.GetItemWidthViaScreenWidth(numCols, interitemSpacing, insets);
            var availHeight = ViewHelper.ScreenHeightMinusStatusAndNavBar - headerHeight;
            var itemHeight = ViewHelper.GetItemHeightViaAvailableScreenHeight(
                availHeight,
                numRows,
                lineSpacing,
                insets);

            var layout = new UICollectionViewFlowLayout()
            {
                HeaderReferenceSize = new CGSize(0, headerHeight),
                ItemSize = new CGSize(itemWidth, itemHeight),
                SectionInset = insets,
                MinimumInteritemSpacing = interitemSpacing,
                MinimumLineSpacing = lineSpacing
            };

            return layout;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIBarButtonSystemItem.Save, async (sender, args) =>
                {
                    ShowLoadingOverlay();

                    try
                    {
                        await ViewModel.Save();
                    }
                    catch (Exception ex)
                    {
                        var okAlertController = UIAlertController.Create("Error", "Oops, there was a problem saving the stamps.", UIAlertControllerStyle.Alert);
                        okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                        PresentViewController(okAlertController, true, null);
                    }

                    HideLoadingOverlay();
                }),
                true);

            CollectionView.BackgroundColor = UIColor.White;
            CollectionView.RegisterClassForCell(typeof(StampSlotCell), CELL_ID);
            //CollectionView.RegisterClassForSupplementaryView(typeof(Header), UICollectionElementKindSection.Header, headerId);
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return StampCardViewModel.NUM_SLOTS;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (StampSlotCell)collectionView.DequeueReusableCell(CELL_ID, indexPath);

            cell.Stamped = ViewModel.Stamps > indexPath.Item;

            return cell;
        }

        public override async void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (StampSlotCell)collectionView.CellForItem(indexPath);

            cell.Stamped = !cell.Stamped;

            if (cell.Stamped)
            {
                ViewModel.AddStamp();
                if (ViewModel.Stamps == 0)
                {
                    UIAlertController alert = UIAlertController.Create("Complete!", "A new stamp card will be made.", UIAlertControllerStyle.Alert);

                    var yesAction = UIAlertAction.Create("Confirm", UIAlertActionStyle.Default, action => StartNewStampCard());
                    alert.AddAction(yesAction);

                    var cancelAction = UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, action => RemoveStamp(cell));
                    alert.AddAction(cancelAction);

                    await PresentViewControllerAsync(alert, true);
                }
            }
            else
            {
                ViewModel.RemoveStamp();
            }
        }

        //public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        //{
        //    return base.GetViewForSupplementaryElement(collectionView, elementKind, indexPath);
        //}

        private void ShowLoadingOverlay()
        {
            var bounds = UIScreen.MainScreen.Bounds;
            _loadingOverlay = new LoadingOverlay(bounds, "Saving changes...");
            View.Add(_loadingOverlay);
        }

        private void HideLoadingOverlay()
        {
            _loadingOverlay.Hide();
        }
    }
}