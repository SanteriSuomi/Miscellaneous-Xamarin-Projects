﻿using MoviesBrowser.Modules;
using System.Threading.Tasks;

namespace MoviesBrowser.Common.Navigation
{
    public interface INavigationService
    {
        Task PushAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel;
        Task PopAsync();
    }
}