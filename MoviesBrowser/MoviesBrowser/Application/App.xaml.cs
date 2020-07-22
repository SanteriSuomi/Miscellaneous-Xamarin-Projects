using Autofac;
using MoviesBrowser.Common.Navigation;
using MoviesBrowser.Modules.MainPage;
using System;
using Xamarin.Forms;

namespace MoviesBrowser
{
    public partial class App : Application
    {
        public static IContainer Container { get; set; }

        public App()
        {
            InitializeComponent();

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                            .AsImplementedInterfaces()
                            .AsSelf();

            NavigationPage navigationPage = null;
            var lazyNavigation = new Lazy<INavigation>(() => navigationPage.Navigation);
            builder.RegisterType<NavigationService>()
                            .As<INavigationService>()
                            .WithParameter("navigation", lazyNavigation);

            Container = builder.Build();

            MainPage = new NavigationPage(Container.Resolve<MainPageView>());
        }

        protected override void OnStart()
        {
            // Not implemented on purpose.
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