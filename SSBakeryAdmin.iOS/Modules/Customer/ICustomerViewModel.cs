using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.iOS.Modules.Customer
{
    public interface ICustomerViewModel
    {
        string Name { get; }

        string PhoneNumber { get; }

        ReactiveCommand<Unit, Unit> UseReward { get; }
    }
}