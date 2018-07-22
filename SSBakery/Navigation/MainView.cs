using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Genesis.Ensure;
using ReactiveUI;
using SSBakery.UI.Navigation.Interfaces;
using Xamarin.Forms;

namespace SSBakery.UI.Navigation
{
    public sealed class MainView : NavigationPage, IView
    {
        private readonly IScheduler _backgroundScheduler;
        private readonly IScheduler _mainScheduler;
        private readonly IViewLocator _viewLocator;

        public MainView(
            IScheduler backgroundScheduler,
            IScheduler mainScheduler,
            IViewLocator viewLocator)
        {
            Ensure.ArgumentNotNull(backgroundScheduler, nameof(backgroundScheduler));
            Ensure.ArgumentNotNull(mainScheduler, nameof(mainScheduler));
            Ensure.ArgumentNotNull(viewLocator, nameof(viewLocator));

            _backgroundScheduler = backgroundScheduler;
            _mainScheduler = mainScheduler;
            _viewLocator = viewLocator;

            PagePopped = Observable
                .FromEventPattern<NavigationEventArgs>(x => this.Popped += x, x => this.Popped -= x)
                .Select(ep => ep.EventArgs.Page.BindingContext as IPageViewModel)
                .WhereNotNull();
        }

        public IObservable<IPageViewModel> PagePopped { get; }

        public IObservable<Unit> PushModal(IModalViewModel modalViewModel, string contract)
        {
            Ensure.ArgumentNotNull(modalViewModel, nameof(modalViewModel));

            return Observable
                .Start(
                    () =>
                    {
                        var page = this.LocatePageFor(modalViewModel, contract);
                        this.SetPageTitle(page, modalViewModel.Id);
                        return page;
                    },
                    this._backgroundScheduler)
                .ObserveOn(this._mainScheduler)
                .SelectMany(
                    page =>
                        this
                            .Navigation
                            .PushModalAsync(page)
                            .ToObservable());
        }

        public IObservable<Unit> PopModal() =>
            this
                .Navigation
                .PopModalAsync()
                .ToObservable()
                .ToSignal()
                // XF completes the pop operation on a background thread :/
                .ObserveOn(this._mainScheduler);

        public IObservable<Unit> PushPage(IPageViewModel pageViewModel, string contract, bool resetStack, bool animate)
        {
            Ensure.ArgumentNotNull(pageViewModel, nameof(pageViewModel));

            // If we don't have a root page yet, be sure we create one and assign one immediately because otherwise we'll get an exception.
            // Otherwise, create it off the main thread to improve responsiveness and perceived performance.
            var hasRoot = Navigation.NavigationStack.Count > 0;
            var mainScheduler = hasRoot ? _mainScheduler : CurrentThreadScheduler.Instance;
            var backgroundScheduler = hasRoot ? _backgroundScheduler : CurrentThreadScheduler.Instance;

            return Observable
                .Start(
                    () =>
                    {
                        var page = this.LocatePageFor(pageViewModel, contract);
                        this.SetPageTitle(page, pageViewModel.Id);
                        return page;
                    },
                    backgroundScheduler)
                .ObserveOn(mainScheduler)
                .SelectMany(
                    page =>
                    {
                        if(resetStack)
                        {
                            if(this.Navigation.NavigationStack.Count == 0)
                            {
                                return this
                                    .Navigation
                                    .PushAsync(page, animated: false)
                                    .ToObservable();
                            }
                            else
                            {
                            // XF does not allow us to pop to a new root page. Instead, we need to inject the new root page and then pop to it.
                                this
                                    .Navigation
                                    .InsertPageBefore(page, this.Navigation.NavigationStack[0]);

                                return this
                                    .Navigation
                                    .PopToRootAsync(animated: false)
                                    .ToObservable();
                            }
                        }
                        else
                        {
                            return this
                                .Navigation
                                .PushAsync(page, animate)
                                .ToObservable();
                        }
                    });
        }

        public IObservable<Unit> PopPage(bool animate) =>
            this
                .Navigation
                .PopAsync(animate)
                .ToObservable()
                .ToSignal()
                // XF completes the pop operation on a background thread :/
                .ObserveOn(this._mainScheduler);

        public void RemovePage(int idx)
        {
            var page = Navigation.NavigationStack[idx];
            Navigation.RemovePage(page);
        }

        public void InsertPage(int idx, IPageViewModel pageViewModel, string contract = null)
        {
            var page = this.LocatePageFor(pageViewModel, contract);
            this.SetPageTitle(page, pageViewModel.Id);
            this.Navigation.InsertPageBefore(page, Navigation.NavigationStack[idx]);
        }

        private Page LocatePageFor(object viewModel, string contract)
        {
            Ensure.ArgumentNotNull(viewModel, nameof(viewModel));

            var viewFor = _viewLocator.ResolveView(viewModel, contract);
            var page = viewFor as Page;

            if(viewFor == null)
            {
                throw new InvalidOperationException($"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
            }

            if(page == null)
            {
                throw new InvalidOperationException($"Resolved view '{viewFor.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' is not a Page.");
            }

            viewFor.ViewModel = viewModel;

            return page;
        }

        private void SetPageTitle(Page page, string resourceKey)
        {
            // var title = Localize.GetString(resourceKey);
            page.Title = resourceKey; // title;
        }
    }
}
