using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.Modules.StampCard
{
    public interface IStampCardViewModel
    {
        ReactiveCommand<Unit, Unit> AddStamp { get; }

        ReactiveCommand<Unit, Unit> RemoveStamp { get; }

        ReactiveCommand<Unit, Unit> StartNewStampCard { get; }
    }
}