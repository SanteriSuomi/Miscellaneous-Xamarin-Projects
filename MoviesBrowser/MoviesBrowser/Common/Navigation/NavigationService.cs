using Autofac;
using MoviesBrowser.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoviesBrowser.Common.Navigation
{
    public class NavigationService : INavigationService
    {
        public NavigationService(Lazy<INavigation> navigation)
        {
            _navigation = navigation;
            PageMap = GetAndRegisterViewAndViewModelTypes();
        }

        public Dictionary<Type, Type> PageMap { get; }

        private readonly Lazy<INavigation> _navigation;

        public async Task PushAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel
        {
            var pageType = PageMap[typeof(TViewModel)];
            var page = App.Container.Resolve(pageType) as Page;
            await _navigation.Value.PushAsync(page);

            var baseView = page.BindingContext as BaseViewModel;
            await baseView.InitializeAsync(parameter);
        }

        public async Task PopAsync()
        {
            await _navigation.Value.PopAsync();
        }

        private Dictionary<Type, Type> GetAndRegisterViewAndViewModelTypes()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            var pageMap = new Dictionary<Type, Type>();
            foreach (var type in types)
            {
                var typeName = type.FullName.ToLowerInvariant();
                if (typeName.Contains("viewmodel"))
                {
                    pageMap.Add(type, null);
                }
            }

            foreach (var type in types)
            {
                var typeName = type.FullName.ToLowerInvariant();
                if (!typeName.Contains("viewmodel")
                    && typeName.Contains("view"))
                {
                    var fullType = Type.GetType($"{type.FullName}Model");
                    if (fullType != null)
                    {
                        pageMap[fullType] = type;
                    }
                }
            }

            return pageMap;
        }
    }
}