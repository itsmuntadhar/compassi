using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Compassi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.MainView()
            {
                Visual = VisualMarker.Material,
                BackgroundColor = Color.FromHex("#121212")
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
