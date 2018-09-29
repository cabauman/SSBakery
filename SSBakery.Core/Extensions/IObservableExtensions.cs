using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace SSBakery
{
    public static class IObservableExtensions
    {
        public static IObservable<T> WhereNotNull<T>(this IObservable<T> @this)
        {
            return @this.Where(x => x != null);
        }

        public static IObservable<Unit> ToSignal<T>(this IObservable<T> @this)
        {
            return @this.Select(_ => Unit.Default);
        }

        public static IObservable<T> Spy<T>(this IObservable<T> source, string opName = null)
        {
            opName = opName ?? "IObservable";
            Console.WriteLine("{0}: Observable obtained on Thread: {1}",
                              opName,
                              Thread.CurrentThread.ManagedThreadId);

            return Observable.Create<T>(obs =>
            {
                Console.WriteLine("{0}: Subscribed to on Thread: {1}",
                                  opName,
                                  Thread.CurrentThread.ManagedThreadId);

                try
                {
                    var subscription = source
                        .Do(x => Console.WriteLine("{0}: OnNext({1}) on Thread: {2}",
                                                    opName,
                                                    x,
                                                    Thread.CurrentThread.ManagedThreadId),
                            ex => Console.WriteLine("{0}: OnError({1}) on Thread: {2}",
                                                     opName,
                                                     ex,
                                                     Thread.CurrentThread.ManagedThreadId),
                            () => Console.WriteLine("{0}: OnCompleted() on Thread: {1}",
                                                     opName,
                                                     Thread.CurrentThread.ManagedThreadId)
                        )
                        .Subscribe(obs);
                    return new CompositeDisposable(
                        subscription,
                        Disposable.Create(() => Console.WriteLine(
                              "{0}: Cleaned up on Thread: {1}",
                              opName,
                              Thread.CurrentThread.ManagedThreadId)));
                }
                finally
                {
                    Console.WriteLine("{0}: Subscription completed.", opName);
                }
            });
        }
    }
}
