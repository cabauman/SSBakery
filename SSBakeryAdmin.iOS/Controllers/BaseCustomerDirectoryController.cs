using ReactiveUI;
using SSBakeryAdmin.iOS.Modules.CustomerDirectory;
using UIKit;

namespace SSBakeryAdmin.iOS
{
    public class BaseCustomerDirectoryController<TViewModel> : ReactiveTableViewController<TViewModel>
        where TViewModel : class
    {
        protected const string SUBTITLE_FORMAT = "Stamps: {0} {1}";
        protected const string CELL_ID = "CELL_ID";

        private TViewModel _viewModel;

        public BaseCustomerDirectoryController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //TableView.RegisterNibForCellReuse(UINib.FromName("TableCell", null), CELL_ID);
        }

        protected void ConfigureCell(UITableViewCell cell, ICustomerCellViewModel customer)
        {
            cell.TextLabel.Text = customer.PhoneNumber;
            string customerName = customer.Name + ", " ?? string.Empty;
            string rewardText = customer.RewardData.UnclaimedRewardCount > 0 ?
                string.Format("***{0} Reward***", customer.RewardData.UnclaimedRewardCount) :
                string.Empty;
            cell.DetailTextLabel.Text = customerName + string.Format(SUBTITLE_FORMAT, customer.Stamps, rewardText);
        }
    }
}