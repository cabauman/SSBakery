using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace SSBakeryAdmin.iOS.Modules.CustomerDirectory
{
    public class CustomerDirectorySearchResultsController : BaseCustomerDirectoryController<ICustomerDirectorySearchResultsViewModel>
    {
        public IList<ICustomerCellViewModel> FilteredCustomers { get; set; }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return FilteredCustomers.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CELL_ID);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CELL_ID);
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            }

            ICustomerCellViewModel customer = FilteredCustomers[indexPath.Row];
            ConfigureCell(cell, customer);

            return cell;
        }
    }
}