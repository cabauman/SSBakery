using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.iOS.Modules.StampCard
{
    public class StampCardViewModel : IStampCardViewModel
    {
        public ReactiveCommand<Unit, Unit> AddStamp { get; }

        public ReactiveCommand<Unit, Unit> RemoveStamp { get; }

        public ReactiveCommand<Unit, Unit> StartNewStampCard { get; }
    }
}