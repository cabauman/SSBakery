using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Genesis.Ensure;
using Splat;
using SSBakery.UI.Navigation.Interfaces;

namespace SSBakery.UI.Navigation
{
    public sealed class ViewStackService : IViewStackService, IEnableLogger
    {
        private readonly BehaviorSubject<IImmutableList<IModalViewModel>> _modalStack;
        private readonly BehaviorSubject<IImmutableList<IPageViewModel>> _pageStack;
        private readonly IView _view;

        public ViewStackService(IView view)
        {
            Ensure.ArgumentNotNull(view, nameof(view));

            _modalStack = new BehaviorSubject<IImmutableList<IModalViewModel>>(ImmutableList<IModalViewModel>.Empty);
            _pageStack = new BehaviorSubject<IImmutableList<IPageViewModel>>(ImmutableList<IPageViewModel>.Empty);
            _view = view;

            this
                ._view
                .PagePopped
                .Do(
                    poppedPage =>
                    {
                        var currentPageStack = _pageStack.Value;

                        if(currentPageStack.Count > 0 && poppedPage == currentPageStack[currentPageStack.Count - 1])
                        {
                            var removedPage = PopStackAndTick(this._pageStack);
                            this.Log().Debug("Removed page '{0}' from stack.", removedPage.Id);
                        }
                    })
                .Subscribe();
        }

        public IView View => _view;

        public int PageCount => _pageStack.Value.Count;

        public IObservable<IImmutableList<IModalViewModel>> ModalStack => _modalStack;

        public IObservable<IImmutableList<IPageViewModel>> PageStack => _pageStack;

        public IObservable<Unit> PushPage(IPageViewModel page, string contract = null, bool resetStack = false, bool animate = true)
        {
            Ensure.ArgumentNotNull(page, nameof(page));

            return this
                ._view
                .PushPage(page, contract, resetStack, animate)
                .Do(
                    _ =>
                    {
                        AddToStackAndTick(this._pageStack, page, resetStack);
                        this.Log().Debug("Added page '{0}' (contract '{1}') to stack.", page.Id, contract);
                    });
        }

        public IObservable<Unit> PopPages(int count = 1, bool animateLastPage = true)
        {
            Ensure.ArgumentCondition(count > 0, "Number of pages should be greater than 0.", nameof(count));

            return Observable
                .Range(1, count)
                .SelectMany(x => _view.PopPage(x == count && animateLastPage))
                .Skip(count - 1);
        }

        public IObservable<Unit> PopToPage<TViewModel>(bool animateLastPage = true)
            where TViewModel : IPageViewModel
        {
            var stack = _pageStack.Value;
            int idxOfLastPage = stack.Count - 1;

            return stack
                .ToObservable()
                .Take(idxOfLastPage)
                .LastOrDefaultAsync(x => x is TViewModel)
                .SelectMany(x =>
                    {
                        if(x != null)
                        {
                            int numPagesToPop = idxOfLastPage - stack.LastIndexOf(x);
                            return PopPages2(numPagesToPop, animateLastPage);
                        }
                        else
                        {
                            return Observable.Return(Unit.Default);
                        }
                    });
        }

        public IObservable<Unit> PopToPageAndPush<TPageToPopTo>(IPageViewModel pageToPush, string contract = null, bool animateLastPage = true)
            where TPageToPopTo : IPageViewModel
        {
            var stack = _pageStack.Value;

            return stack
                .ToObservable()
                .Take(stack.Count - 1)
                .LastOrDefaultAsync(x => x is TPageToPopTo)
                .SelectMany(x =>
                {
                    if(x != null)
                    {
                        int idxOfPageToPopTo = stack.LastIndexOf(x);
                        stack = stack.Insert(idxOfPageToPopTo + 1, pageToPush);
                        _pageStack.OnNext(stack);
                        _view.InsertPage(idxOfPageToPopTo + 1, pageToPush);
                        int idxOfLastPage = stack.Count - 1;
                        int numPagesToPop = idxOfLastPage - idxOfPageToPopTo;
                        return PopPages2(numPagesToPop, animateLastPage);
                    }
                    else
                    {
                        return Observable.Return(Unit.Default);
                    }
                });
        }

        public IObservable<Unit> PopPages2(int count = 1, bool animateLastPage = true)
        {
            Ensure.ArgumentCondition(count > 0 && count < PageCount, "Page pop count should be less than the size of the stack.", nameof(count));

            var stack = _pageStack.Value;

            if(count > 1)
            {
                // Remove count - 1 pages (leaving only the top page).
                int idxOfSecondToLastPage = stack.Count - 2;
                for(int i = idxOfSecondToLastPage; i >= stack.Count - count; --i)
                {
                    _view.RemovePage(i);
                }

                stack = stack.RemoveRange(stack.Count - count, count - 1);
                _pageStack.OnNext(stack);
            }

            // Now remove the top page with optional animation.
            return this
                ._view
                .PopPage(animateLastPage);
        }

        public IObservable<Unit> PushModal(IModalViewModel modal, string contract = null)
        {
            Ensure.ArgumentNotNull(modal, nameof(modal));

            return this
                ._view
                .PushModal(modal, contract)
                .Do(
                    _ =>
                    {
                        AddToStackAndTick(this._modalStack, modal, false);
                        this.Log().Debug("Added modal '{0}' (contract '{1}') to stack.", modal.Id, contract);
                    });
        }

        public IObservable<Unit> PopModal() =>
            this
                ._view
                .PopModal()
                .Do(
                    _ =>
                    {
                        var removedModal = PopStackAndTick(this._modalStack);
                        this.Log().Debug("Removed modal '{0}' from stack.", removedModal.Id);
                    });

        private static void AddToStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject, T item, bool reset)
        {
            var stack = stackSubject.Value;

            if(reset)
            {
                stack = new[] { item }.ToImmutableList();
            }
            else
            {
                stack = stack.Add(item);
            }

            stackSubject.OnNext(stack);
        }

        private static T PopStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject)
        {
            var stack = stackSubject.Value;

            if(stack.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            var removedItem = stack[stack.Count - 1];
            stack = stack.RemoveAt(stack.Count - 1);
            stackSubject.OnNext(stack);
            return removedItem;
        }
    }
}
