using System.Reactive;
using ReactiveUI;
using GameCtor.RxNavigation;

namespace SSBakeryAdmin.iOS.Modules.Customer
{
    public class CustomerViewModel : ICustomerViewModel, IPageViewModel
    {
        private SSBakery.Models.SSBakeryUser _customer;

        public CustomerViewModel(SSBakery.Models.SSBakeryUser customer)
        {
            _customer = customer;
        }

        public string Title => "Customer";

        public string Name => _customer.Name;

        public string PhoneNumber => _customer.PhoneNumber;

        public ReactiveCommand<Unit, Unit> UseReward { get; }
    }
}