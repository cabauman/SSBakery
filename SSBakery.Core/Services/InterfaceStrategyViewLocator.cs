using System;
using System.Reflection;
using ReactiveUI;
using Splat;

namespace SSBakery.Core.Services
{
    public class DefaultViewLocator2 : IViewLocator, IEnableLogger
    {
        public DefaultViewLocator2(Func<string, string> viewModelToViewFunc = null)
        {
            ViewModelToViewFunc = viewModelToViewFunc ?? (vm => vm.Replace("ViewModel", "View"));
        }

        /// <summary>
        /// Gets or sets a function that is used to convert a view model name to a proposed view name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If unset, the default behavior is to change "ViewModel" to "View". If a different convention is followed, assign an appropriate function to this
        /// property.
        /// </para>
        /// <para>
        /// Note that the name returned by the function is a starting point for view resolution. Variants on the name will be resolved according to the rules
        /// set out by the <see cref="ResolveView"/> method.
        /// </para>
        /// </remarks>
        public Func<string, string> ViewModelToViewFunc { get; set; }

        /// <summary>
        /// Returns the view associated with a view model, deriving the name of the type via <see cref="ViewModelToViewFunc"/>, then discovering it via the
        /// service locator.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Given view model type <c>T</c> with runtime type <c>RT</c>, this implementation will attempt to resolve the following views:
        /// <list type="number">
        /// <item>
        /// <description>
        /// Look for a service registered under the type whose name is given to us by passing <c>RT</c> to <see cref="ViewModelToViewFunc"/> (which defaults to changing "ViewModel" to "View").
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Look for a service registered under the type <c>IViewFor&lt;RT&gt;</c>.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Look for a service registered under the type whose name is given to us by passing <c>T</c> to <see cref="ViewModelToViewFunc"/> (which defaults to changing "ViewModel" to "View").
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Look for a service registered under the type <c>IViewFor&lt;T&gt;</c>.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If <c>T</c> is an interface, change its name to that of a class (i.e. drop the leading "I"). If it's a class, change to an interface (i.e. add a leading "I").
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Repeat steps 1-4 with the type resolved from the modified name.
        /// </description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <param name="viewModel">
        /// The view model whose associated view is to be resolved.
        /// </param>
        /// <param name="contract">
        /// Optional contract to be used when resolving from Splat
        /// </param>
        /// <returns>
        /// The view associated with the given view model.
        /// </returns>
        public IViewFor ResolveView<T>(T viewModel, string contract = null)
            where T : class
        {
            var view = this.AttemptViewResolutionFor(ToggleViewModelType(viewModel.GetType()), contract);

            if(view != null)
            {
                return view;
            }

            view = this.AttemptViewResolutionFor(viewModel.GetType(), contract);

            if(view != null)
            {
                return view;
            }

            view = this.AttemptViewResolutionFor(typeof(T), contract);

            if(view != null)
            {
                return view;
            }

            view = this.AttemptViewResolutionFor(ToggleViewModelType(typeof(T)), contract);

            if(view != null)
            {
                return view;
            }

            this.Log().Warn("Failed to resolve view for view model type '{0}'.", typeof(T).FullName);
            return null;
        }

        private IViewFor AttemptViewResolutionFor(Type viewModelType, string contract)
        {
            if(viewModelType == null) return null;

            var proposedViewTypeName = typeof(IViewFor<>).MakeGenericType(viewModelType).AssemblyQualifiedName;
            var view = this.AttemptViewResolution(proposedViewTypeName, contract);

            if(view != null)
            {
                return view;
            }

            var viewModelTypeName = viewModelType.AssemblyQualifiedName;
            proposedViewTypeName = this.ViewModelToViewFunc(viewModelTypeName);
            view = this.AttemptViewResolution(proposedViewTypeName, contract);

            return view;
        }

        private IViewFor AttemptViewResolution(string viewTypeName, string contract)
        {
            try
            {
                var viewType = Reflection.ReallyFindType(viewTypeName, throwOnFailure: false);

                if(viewType == null)
                {
                    this.Log().Debug("Failed to find type named '{0}'.", viewTypeName);
                    return null;
                }

                var service = Locator.Current.GetService(viewType, contract);

                if(service == null)
                {
                    this.Log().Debug("Failed to resolve service for type '{0}'.", viewType.FullName);
                    return null;
                }

                var view = service as IViewFor;

                if(view == null)
                {
                    this.Log().Debug("Resolve service type '{0}' does not implement '{1}'.", viewType.FullName, typeof(IViewFor).FullName);
                    return null;
                }

                return view;
            }
            catch(Exception ex)
            {
                this.Log().ErrorException("Exception occurred whilst attempting to resolve type '" + viewTypeName + "' into a view.", ex);
                throw;
            }
        }

        private static Type ToggleViewModelType(Type viewModelType)
        {
            var viewModelTypeName = viewModelType.AssemblyQualifiedName;

            if(viewModelType.GetTypeInfo().IsInterface)
            {
                if(viewModelType.Name.StartsWith("I"))
                {
                    var toggledTypeName = DeinterfaceifyTypeName(viewModelTypeName);
                    var toggledType = Reflection.ReallyFindType(toggledTypeName, throwOnFailure: false);
                    return toggledType;
                }
            }
            else
            {
                var toggledTypeName = InterfaceifyTypeName(viewModelTypeName);
                var toggledType = Reflection.ReallyFindType(toggledTypeName, throwOnFailure: false);
                return toggledType;
            }

            return null;
        }

        private static string DeinterfaceifyTypeName(string typeName)
        {
            var idxComma = typeName.IndexOf(',');
            var idxPeriod = typeName.LastIndexOf('.', idxComma - 1);
            return typeName.Substring(0, idxPeriod + 1) + typeName.Substring(idxPeriod + 2);
        }

        private static string InterfaceifyTypeName(string typeName)
        {
            var idxComma = typeName.IndexOf(',');
            var idxPeriod = typeName.LastIndexOf('.', idxComma - 1);
            return typeName.Insert(idxPeriod + 1, "I");
        }
    }







    public class InterfaceStrategyViewLocator : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string contract = null)
            where T : class
        {
            var viewModelFullName = viewModel.GetType().AssemblyQualifiedName;
            var idxComma = viewModelFullName.IndexOf(',');
            var idxPeriod = viewModelFullName.LastIndexOf('.', idxComma - 1);
            var viewModelName = viewModelFullName.Substring(idxPeriod + 1, idxComma - (idxPeriod + 1));
            var viewTypeName = viewModelName.Replace("ViewModel", string.Empty) + viewModelFullName.Substring(idxComma);

            // Simple version
            viewTypeName = viewModelFullName.Replace("ViewModel", string.Empty);

            try
            {
                var viewType = Type.GetType(viewTypeName);
                if(viewType == null)
                {
                    this.Log().Error($"Could not find the view {viewTypeName} for view model {viewModelFullName}.");
                    return null;
                }

                return Activator.CreateInstance(viewType) as IViewFor;

                var service = Locator.Current.GetService(viewType, contract);

                if(service == null)
                {
                    this.Log().Debug("Failed to resolve service for type '{0}'.", viewType.FullName);
                    return null;
                }

                var view = service as IViewFor;

                if(view == null)
                {
                    this.Log().Debug("Resolve service type '{0}' does not implement '{1}'.", viewType.FullName, typeof(IViewFor).FullName);
                    return null;
                }

                return view;
            }
            catch(Exception)
            {
                this.Log().Error($"Could not instantiate view {viewTypeName}.");
                throw;
            }
        }

        private static string DeinterfaceifyTypeName(string typeName)
        {
            var idxComma = typeName.IndexOf(',');
            var idxPeriod = typeName.LastIndexOf('.', idxComma - 1);
            return typeName.Substring(0, idxPeriod + 1) + typeName.Substring(idxPeriod + 2);
        }

        private static string InterfaceifyTypeName(string typeName)
        {
            var idxPeriod = typeName.LastIndexOf('.');
            return typeName.Insert(idxPeriod + 1, "I");
        }
    }
}
