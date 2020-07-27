using Autofac;
using MoviesBrowser.Common.Database;
using MoviesBrowser.Common.Navigation;
using MoviesBrowser.Modules.RootTabbedPage;
using System;
using Xamarin.Forms;

namespace MoviesBrowser
{
    public partial class App : Application
    {
        public static IContainer Container { get; set; }
        private readonly NavigationPage _navigationPage;

        public App()
        {
            InitializeComponent();

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                   .AsImplementedInterfaces()
                   .AsSelf();

            _navigationPage = new NavigationPage();
            var lazyNavigation = new Lazy<INavigation>(() => _navigationPage.Navigation);
            builder.RegisterType<NavigationService>()
                   .As<INavigationService>()
                   .WithParameter("navigation", lazyNavigation);

            builder.RegisterGeneric(typeof(Repository<>))
                   .As(typeof(IRepository<>))
                   .WithParameter("path", DatabaseConstants.Path)
                   .WithParameter("flags", DatabaseConstants.Flags);

            Container = builder.Build();
        }

        protected override async void OnStart()
        {
            MainPage = _navigationPage;
            await _navigationPage.PushAsync(Container.Resolve<RootTabbedPageView>());
        }

        protected override void OnSleep()
        {
            // Not implemented on purpose.
        }

        protected override void OnResume()
        {
            // Not implemented on purpose.
        }
    }
}