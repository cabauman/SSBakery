using System;
using System.Collections.Generic;
using Foundation;
using ReactiveUI;
using SSBakeryAdmin.iOS.Views;
using UIKit;

namespace SSBakeryAdmin.iOS.Modules.CustomerDirectory
{
    public class CustomerDirectoryController : BaseCustomerDirectoryController<ICustomerDirectoryViewModel>
    {
        private CustomerDirectorySearchResultsController _searchResultsController;
        private UISearchController _searchController;
        private bool _searchControllerWasActive;
        private bool _searchControllerSearchFieldWasFirstResponder;
        private LoadingOverlay _loadingOverlay;

        ReactiveTableViewSource<ICustomerCellViewModel> source;

        public CustomerDirectoryController()
        {
            Title = "Home";

            //source.ElementSelected
            source = new ReactiveTableViewSource<ICustomerCellViewModel>(TableView);
            this.OneWayBind(ViewModel, vm => vm.Customers, v => v.source.Data);
            //TableView.RegisterClassForCellReuse(typeof(UITableViewCell), CELL_ID);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIBarButtonSystemItem.Refresh, (sender, args) =>
                {
                    //RetrieveCustomers();
                }),
                true);

            InitSearchController();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            TableView.ReloadData();
        }

        [Export("searchBarSearchButtonClicked:")]
        public virtual void SearchButtonClicked(UISearchBar searchBar)
        {
            searchBar.ResignFirstResponder();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ViewModel.Customers == null ? 0 : ViewModel.Customers.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CELL_ID);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CELL_ID);
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            }

            ICustomerCellViewModel customer = ViewModel.Customers[indexPath.Row];
            ConfigureCell(cell, customer);

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            ICustomerCellViewModel selectedCustomer = (tableView == TableView) ?
                ViewModel.Customers[indexPath.Row] :
                _searchResultsController.FilteredCustomers[indexPath.Row];

            tableView.DeselectRow(indexPath, true);
        }

        public override UISwipeActionsConfiguration GetLeadingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath)
        {
            ICustomerCellViewModel selectedCustomer = (tableView == TableView) ?
                ViewModel.Customers[indexPath.Row] :
                _searchResultsController.FilteredCustomers[indexPath.Row];
            var addRewardAction = ContextualAddRewardAction(selectedCustomer, tableView, indexPath);
            var removeRewardAction = ContextualRemoveRewardAction(selectedCustomer, tableView, indexPath);
            var leadingSwipe = UISwipeActionsConfiguration.FromActions(new UIContextualAction[] { addRewardAction, removeRewardAction });
            leadingSwipe.PerformsFirstActionWithFullSwipe = false;

            return leadingSwipe;
        }

        public override UISwipeActionsConfiguration GetTrailingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath)
        {
            ICustomerCellViewModel selectedCustomer = (tableView == TableView) ?
                ViewModel.Customers[indexPath.Row] :
                _searchResultsController.FilteredCustomers[indexPath.Row];
            var useRewardAction = ContextualUseRewardAction(selectedCustomer, tableView, indexPath);
            var leadingSwipe = UISwipeActionsConfiguration.FromActions(new UIContextualAction[] { useRewardAction });
            leadingSwipe.PerformsFirstActionWithFullSwipe = false;

            return leadingSwipe;
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

        [Export("updateSearchResultsForSearchController:")]
        public virtual void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            var tableController = (CustomerDirectorySearchResultsController)searchController.SearchResultsController;
            tableController.FilteredCustomers = PerformSearch(searchController.SearchBar.Text);
            tableController.TableView.ReloadData();
        }

        private UIContextualAction ContextualUseRewardAction(ICustomerCellViewModel selectedCustomer, UITableView tableView, NSIndexPath indexPath)
        {
            bool hasReward = selectedCustomer.RewardData.UnclaimedRewardCount > 0;
            string title = hasReward ? "Use Reward" : "No Rewards";

            Action<UIAlertAction> alertActionHandler = null;
            if (hasReward)
            {
                alertActionHandler = async _ => await RemoveUnclaimedReward(selectedCustomer, tableView, indexPath);
            }

            UIContextualActionHandler contextualActionHandler = (flagAction, view, success) => { success(true); };
            if (hasReward)
            {
                contextualActionHandler =
                    (flagAction, view, success) =>
                    {
                        var alertController = UIAlertController.Create("Use reward?", string.Empty, UIAlertControllerStyle.Alert);
                        alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
                        alertController.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Default, alertActionHandler));
                        PresentViewController(alertController, true, null);

                        success(true);
                    };
            }

            var contextualAction = UIContextualAction.FromContextualActionStyle(
                UIContextualActionStyle.Normal,
                title,
                contextualActionHandler);

            //action.Image = UIImage.FromFile("feedback.png");
            if (hasReward)
            {
                contextualAction.BackgroundColor = UIColor.Blue;
            }
            else
            {
                contextualAction.BackgroundColor = UIColor.Gray;
            }

            return contextualAction;
        }

        private UIContextualAction ContextualAddRewardAction(ICustomerCellViewModel selectedCustomer, UITableView tableView, NSIndexPath indexPath)
        {
            Action<UIAlertAction> alertActionHandler =
                _ => ViewModel.AddUnclaimedReward(indexPath.Row);

            UIContextualActionHandler contextualActionHandler =
                (flagAction, view, success) =>
                {
                    var alertController = UIAlertController.Create("Add reward?", string.Empty, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
                    alertController.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Default, alertActionHandler));
                    PresentViewController(alertController, true, null);

                    success(true);
                };

            var contextualAction = UIContextualAction.FromContextualActionStyle(
                UIContextualActionStyle.Normal,
                "Add Reward",
                contextualActionHandler);

            contextualAction.BackgroundColor = UIColor.Green;

            return contextualAction;
        }

        private UIContextualAction ContextualRemoveRewardAction(ICustomerCellViewModel selectedCustomer, UITableView tableView, NSIndexPath indexPath)
        {
            bool hasAtLeastOneReward = selectedCustomer.RewardData.UnclaimedRewardCount > 0;

            Action<UIAlertAction> alertActionHandler = null;
            if (hasAtLeastOneReward)
            {
                alertActionHandler = _ => ViewModel.RemoveUnclaimedReward(indexPath.Row);
            }

            UIContextualActionHandler contextualActionHandler = (flagAction, view, success) => { success(true); };
            if (hasAtLeastOneReward)
            {
                contextualActionHandler =
                    (flagAction, view, success) =>
                    {
                        var alertController = UIAlertController.Create("Remove reward?", "", UIAlertControllerStyle.Alert);
                        alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
                        alertController.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Default, alertActionHandler));
                        PresentViewController(alertController, true, null);

                        success(true);
                    };
            }

            var contextualAction = UIContextualAction.FromContextualActionStyle(
                UIContextualActionStyle.Normal,
                "Remove Reward",
                contextualActionHandler);

            if (hasAtLeastOneReward)
            {
                contextualAction.BackgroundColor = UIColor.Red;
            }
            else
            {
                contextualAction.BackgroundColor = UIColor.Gray;
            }

            return contextualAction;
        }

        private void InitSearchController()
        {
            _searchResultsController = new CustomerDirectorySearchResultsController
            {
                FilteredCustomers = new List<ICustomerCellViewModel>()
            };

            _searchController = new UISearchController(_searchResultsController)
            {
                WeakDelegate = this,
                DimsBackgroundDuringPresentation = false,
                WeakSearchResultsUpdater = this
            };

            _searchController.SearchBar.KeyboardType = UIKeyboardType.PhonePad;
            _searchController.SearchBar.SizeToFit();
            TableView.TableHeaderView = _searchController.SearchBar;

            _searchResultsController.TableView.WeakDelegate = this;
            _searchController.SearchBar.WeakDelegate = this;

            DefinesPresentationContext = true;

            if (_searchControllerWasActive)
            {
                _searchController.Active = _searchControllerWasActive;
                _searchControllerWasActive = false;

                if (_searchControllerSearchFieldWasFirstResponder)
                {
                    _searchController.SearchBar.BecomeFirstResponder();
                    _searchControllerSearchFieldWasFirstResponder = false;
                }
            }
        }

        private void ShowLoadingOverlay()
        {
            var bounds = UIScreen.MainScreen.Bounds;
            _loadingOverlay = new LoadingOverlay(bounds, "Loading customers...");
            View.Add(_loadingOverlay);
        }

        private void HideLoadingOverlay()
        {
            _loadingOverlay.Hide();
        }
    }
}