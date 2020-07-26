﻿using Autofac;
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
        }

        public static Dictionary<Type, Type> PageMap { get; } = GetAndRegisterViewsToViewModels();

        private readonly Lazy<INavigation> _navigation;

        public async Task<bool> PushAsync<TViewModel>(object parameter = null, bool animated = true) where TViewModel : BaseViewModel
        {
            var pageType = PageMap[typeof(TViewModel)];
            var resolvedPage = App.Container.Resolve(pageType);
            if (resolvedPage is Page page)
            {
                await _navigation.Value.PushAsync(page, animated);
                var baseView = page.BindingContext as BaseViewModel;
                await baseView.InitializeAsync(parameter);
                return true;
            }

            return false;
        }

        public async Task PopAsync()
        {
            await _navigation.Value.PopAsync();
        }

        /// <summary>
        /// Registers all view types to viewmodel types.
        /// </summary>
        /// <returns>Dictionary where the key is viewmodel and value is view.</returns>
        private static Dictionary<Type, Type> GetAndRegisterViewsToViewModels()
        {
            var assembly = Assembly.GetCallingAssembly();
            var types = assembly.GetTypes();

            var pageMap = new Dictionary<Type, Type>();
            foreach (var type in types)
            {
                if (type.BaseType == typeof(BaseViewModel))
                {
                    pageMap.Add(type, null);
                }
            }

            foreach (var type in types)
            {
                if (type.BaseType == typeof(ContentPage)
                    || type.BaseType == typeof(TabbedPage)
                    || type.BaseType == typeof(Page))
                {
                    var pageViewModel = Type.GetType($"{type.FullName}Model");
                    pageMap[pageViewModel] = type;
                }
            }

            return pageMap;
        }
    }
}