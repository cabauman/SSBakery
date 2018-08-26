//using UIKit;

//namespace SSBakeryAdmin.iOS
//{
//	public class BaseTableViewController : UITableViewController
//	{
//        private const string SUBTITLE_FORMAT = "Stamps: {0} {1}";

//		protected const string CELL_ID = "CELL_ID";

//		public BaseTableViewController()
//		{
//        }

//		protected void ConfigureCell(UITableViewCell cell, CustomerViewModel customer)
//		{
//			cell.TextLabel.Text = customer.PhoneNumber;
//            string customerName = customer.Name + ", " ?? string.Empty;
//            string rewardText = customer.RewardData.UnclaimedRewardCount > 0 ?
//                string.Format("***{0} Reward***", customer.RewardData.UnclaimedRewardCount) :
//                "";
//			cell.DetailTextLabel.Text = customerName + string.Format(SUBTITLE_FORMAT, customer.Stamps, rewardText);
//		}

//		public override void ViewDidLoad()
//		{
//			base.ViewDidLoad();
//			//TableView.RegisterNibForCellReuse(UINib.FromName("TableCell", null), CELL_ID);
//		}
//	}
//}