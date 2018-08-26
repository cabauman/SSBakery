//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Foundation;
//using UIKit;
//using SSBakeryAdmin.iOS.Controllers;
//using SSBakeryAdmin.ViewModels;
//using SSBakeryAdmin.iOS.Views;
//using System.Threading.Tasks;

//namespace SSBakeryAdmin.iOS
//{
//	public class MainTableViewController : BaseTableViewController
//	{
//		private SearchResultsTableController resultsTableController;
//        private UISearchController searchController;
//        private bool searchControllerWasActive;
//        private bool searchControllerSearchFieldWasFirstResponder;
//        private LoadingOverlay loadingOverlay;

//        public MainTableViewController()
//		{
//            Title = "Home";
//            ViewModel = new CustomerDirectoryViewModel();
//        }

//        public CustomerDirectoryViewModel ViewModel { get; set; }

//        public override void ViewDidLoad()
//		{
//			base.ViewDidLoad();

//            NavigationItem.SetRightBarButtonItem(
//                new UIBarButtonItem(UIBarButtonSystemItem.Refresh, (sender, args) =>
//                {
//                    RetrieveCustomers();
//                }),
//                true);

//            InitSearchController();

//            RetrieveCustomers();
//        }

//        public override void ViewWillAppear(bool animated)
//        {
//            base.ViewWillAppear(animated);
//            TableView.ReloadData();
//        }

//        [Export("searchBarSearchButtonClicked:")]
//		public virtual void SearchButtonClicked(UISearchBar searchBar)
//		{
//			searchBar.ResignFirstResponder();
//		}

//		public override nint RowsInSection(UITableView tableview, nint section)
//		{
//			return ViewModel.Customers == null ? 0 : ViewModel.Customers.Count;
//		}

//		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
//		{
//            UITableViewCell cell = tableView.DequeueReusableCell(CELL_ID);
            
//            if(cell == null)
//            {
//                cell = new UITableViewCell (UITableViewCellStyle.Subtitle, CELL_ID);
//                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
//            }
            
//            CustomerViewModel customer = ViewModel.Customers[indexPath.Row];
//			ConfigureCell(cell, customer);

//			return cell;
//		}

//		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
//		{
//            CustomerViewModel selectedCustomer = (tableView == TableView) ? ViewModel.Customers[indexPath.Row] : resultsTableController.FilteredCustomers[indexPath.Row];

//            var stampCardViewController = new StampCardViewController(StampCardViewController.GetLayout());
//            stampCardViewController.ViewModel = selectedCustomer.StampCard;
//            //var detailViewController = (DetailViewController)Storyboard.InstantiateViewController ("DetailViewController");

//            //PresentViewController(stampCardViewController, true, null);
//			NavigationController.PushViewController(stampCardViewController, true);
//			tableView.DeselectRow(indexPath, true);
//		}

//        public override UISwipeActionsConfiguration GetLeadingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath)
//        {
//            CustomerViewModel selectedCustomer = (tableView == TableView) ? ViewModel.Customers[indexPath.Row] : resultsTableController.FilteredCustomers[indexPath.Row];
//            var addRewardAction = ContextualAddRewardAction(selectedCustomer, tableView, indexPath);
//            var removeRewardAction = ContextualRemoveRewardAction(selectedCustomer, tableView, indexPath);
//            var leadingSwipe = UISwipeActionsConfiguration.FromActions(new UIContextualAction[] { addRewardAction, removeRewardAction });
//            leadingSwipe.PerformsFirstActionWithFullSwipe = false;

//            return leadingSwipe;
//        }

//        public override UISwipeActionsConfiguration GetTrailingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath)
//        {
//            CustomerViewModel selectedCustomer = (tableView == TableView) ? ViewModel.Customers[indexPath.Row] : resultsTableController.FilteredCustomers[indexPath.Row];
//            var useRewardAction = ContextualUseRewardAction(selectedCustomer, tableView, indexPath);
//            var leadingSwipe = UISwipeActionsConfiguration.FromActions(new UIContextualAction[] { useRewardAction });
//            leadingSwipe.PerformsFirstActionWithFullSwipe = false;

//            return leadingSwipe;
//        }

//        private UIContextualAction ContextualUseRewardAction(CustomerViewModel selectedCustomer, UITableView tableView, NSIndexPath indexPath)
//        {
//            bool hasReward = selectedCustomer.RewardData.UnclaimedRewardCount > 0;
//            string title = hasReward ? "Use Reward" : "No Rewards";

//            Action<UIAlertAction> alertActionHandler = null;
//            if(hasReward)
//            {
//                alertActionHandler = async _ => await RemoveUnclaimedReward(selectedCustomer, tableView, indexPath);
//            }

//            UIContextualActionHandler contextualActionHandler = (FlagAction, view, success) => { success(true); };
//            if(hasReward)
//            {
//                contextualActionHandler =
//                    (FlagAction, view, success) =>
//                    {
//                        var alertController = UIAlertController.Create("Use reward?", "", UIAlertControllerStyle.Alert);
//                        alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
//                        alertController.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Default, alertActionHandler));
//                        PresentViewController(alertController, true, null);

//                        success(true);
//                    };
//            }

//            var contextualAction = UIContextualAction.FromContextualActionStyle(
//                UIContextualActionStyle.Normal,
//                title,
//                contextualActionHandler);

//            //action.Image = UIImage.FromFile("feedback.png");
//            if(hasReward)
//            {
//                contextualAction.BackgroundColor = UIColor.Blue;
//            }
//            else
//            {
//                contextualAction.BackgroundColor = UIColor.Gray;
//            }

//            return contextualAction;
//        }

//        private UIContextualAction ContextualAddRewardAction(CustomerViewModel selectedCustomer, UITableView tableView, NSIndexPath indexPath)
//        {
//            Action<UIAlertAction> alertActionHandler =
//                async _ => await AddUnclaimedReward(selectedCustomer, tableView, indexPath);

//            UIContextualActionHandler contextualActionHandler =
//                (FlagAction, view, success) =>
//                {
//                    var alertController = UIAlertController.Create("Add reward?", "", UIAlertControllerStyle.Alert);
//                    alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
//                    alertController.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Default, alertActionHandler));
//                    PresentViewController(alertController, true, null);

//                    success(true);
//                };

//            var contextualAction = UIContextualAction.FromContextualActionStyle(
//                UIContextualActionStyle.Normal,
//                "Add Reward",
//                contextualActionHandler);

//            contextualAction.BackgroundColor = UIColor.Green;

//            return contextualAction;
//        }

//        public UIContextualAction ContextualRemoveRewardAction(CustomerViewModel selectedCustomer, UITableView tableView, NSIndexPath indexPath)
//        {
//            bool hasAtLeastOneReward = selectedCustomer.RewardData.UnclaimedRewardCount > 0;

//            Action<UIAlertAction> alertActionHandler = null;
//            if(hasAtLeastOneReward)
//            {
//                alertActionHandler = async _ => await RemoveUnclaimedReward(selectedCustomer, tableView, indexPath);
//            }

//            UIContextualActionHandler contextualActionHandler = (FlagAction, view, success) => { success(true); };
//            if(hasAtLeastOneReward)
//            {
//                contextualActionHandler =
//                    (FlagAction, view, success) =>
//                    {
//                        var alertController = UIAlertController.Create("Remove reward?", "", UIAlertControllerStyle.Alert);
//                        alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
//                        alertController.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Default, alertActionHandler));
//                        PresentViewController(alertController, true, null);

//                        success(true);
//                    };
//            }

//            var contextualAction = UIContextualAction.FromContextualActionStyle(
//                UIContextualActionStyle.Normal,
//                "Remove Reward",
//                contextualActionHandler);

//            if(hasAtLeastOneReward)
//            {
//                contextualAction.BackgroundColor = UIColor.Red;
//            }
//            else
//            {
//                contextualAction.BackgroundColor = UIColor.Gray;
//            }

//            return contextualAction;
//        }

//        private async Task AddUnclaimedReward(CustomerViewModel selectedCustomer, UITableView tableView, NSIndexPath indexPath)
//        {
//            selectedCustomer.RewardData.UnclaimedRewardCount += 1;

//            await SaveCustomerAndRefreshRow(selectedCustomer, tableView, indexPath);
//        }

//        private async Task RemoveUnclaimedReward(CustomerViewModel selectedCustomer, UITableView tableView, NSIndexPath indexPath)
//        {
//            selectedCustomer.RewardData.UnclaimedRewardCount -= 1;

//            await SaveCustomerAndRefreshRow(selectedCustomer, tableView, indexPath);
//        }

//        private async Task SaveCustomerAndRefreshRow(CustomerViewModel selectedCustomer, UITableView tableView, NSIndexPath indexPath)
//        {
//            ShowLoadingOverlay();

//            try
//            {
//                await selectedCustomer.Save();
//                tableView.BeginUpdates();
//                tableView.ReloadRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
//                tableView.EndUpdates();
//            }
//            catch(Exception ex)
//            {
//                var okAlertController = UIAlertController.Create("Error", "Oops, there was a problem saving the used reward.", UIAlertControllerStyle.Alert);
//                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
//                PresentViewController(okAlertController, true, null);
//            }

//            HideLoadingOverlay();
//        }

//        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
//        {
//            return true;
//        }

//		[Export("updateSearchResultsForSearchController:")]
//		public virtual void UpdateSearchResultsForSearchController (UISearchController searchController)
//		{
//			var tableController = (SearchResultsTableController)searchController.SearchResultsController;
//			tableController.FilteredCustomers = PerformSearch (searchController.SearchBar.Text);
//			tableController.TableView.ReloadData();
//		}

//        private void InitSearchController()
//        {
//            resultsTableController = new SearchResultsTableController
//            {
//                FilteredCustomers = new List<CustomerViewModel>()
//            };

//            searchController = new UISearchController(resultsTableController)
//            {
//                WeakDelegate = this,
//                DimsBackgroundDuringPresentation = false,
//                WeakSearchResultsUpdater = this
//            };

//            searchController.SearchBar.KeyboardType = UIKeyboardType.PhonePad;
//            searchController.SearchBar.SizeToFit();
//            TableView.TableHeaderView = searchController.SearchBar;

//            resultsTableController.TableView.WeakDelegate = this;
//            searchController.SearchBar.WeakDelegate = this;

//            DefinesPresentationContext = true;

//            if(searchControllerWasActive)
//            {
//                searchController.Active = searchControllerWasActive;
//                searchControllerWasActive = false;

//                if(searchControllerSearchFieldWasFirstResponder)
//                {
//                    searchController.SearchBar.BecomeFirstResponder();
//                    searchControllerSearchFieldWasFirstResponder = false;
//                }
//            }
//        }

//        private async Task RetrieveCustomers()
//        {
//            ShowLoadingOverlay();

//            try
//            {
//                await ViewModel.RetrieveCustomers();
//            }
//            catch(Exception ex)
//            {
//                var okAlertController = UIAlertController.Create("Error", "Oops, there was a problem loading the customers.", UIAlertControllerStyle.Alert);
//                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
//                PresentViewController(okAlertController, true, null);
//            }

//            Title = "Customers: " + ViewModel.Customers.Count;
//            HideLoadingOverlay();
//            TableView.ReloadData();
//        }

//		private List<CustomerViewModel> PerformSearch(string searchString)
//		{
//            searchString = string.Concat(searchString.Where(char.IsDigit));
            
//			IEnumerable<CustomerViewModel> query =
//                from c in ViewModel.Customers
//                where c.PhoneNumber != null &&
//                      string.Concat(c.PhoneNumber.Where(char.IsDigit))?.IndexOf(searchString) >= 0
//				select c;

//            var filteredProducts = new List<CustomerViewModel>(query);

//            return filteredProducts.Distinct().ToList();
//		}

//        private void ShowLoadingOverlay()
//        {
//            var bounds = UIScreen.MainScreen.Bounds;
//            loadingOverlay = new LoadingOverlay(bounds, "Loading customers...");
//            View.Add(loadingOverlay);
//        }

//        private void HideLoadingOverlay()
//        {
//            loadingOverlay.Hide();
//        }
//    }
//}