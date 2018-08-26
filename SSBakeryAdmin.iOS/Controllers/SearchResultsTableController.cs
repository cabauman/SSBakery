//using System;
//using System.Collections.Generic;
//using Foundation;
//using UIKit;
//using SSBakeryAdmin.ViewModels;

//namespace SSBakeryAdmin.iOS
//{
//	public class SearchResultsTableController : BaseTableViewController
//	{
//		public List<CustomerViewModel> FilteredCustomers { get; set; }

//		public override nint RowsInSection(UITableView tableview, nint section)
//		{
//			return FilteredCustomers.Count;
//		}

//		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
//		{
//            UITableViewCell cell = tableView.DequeueReusableCell(CELL_ID);

//            if(cell == null)
//            {
//                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CELL_ID);
//                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
//            }

//            CustomerViewModel customer = FilteredCustomers[indexPath.Row];
//			ConfigureCell(cell, customer);

//			return cell;
//		}
//	}
//}