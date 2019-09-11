using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Compassi.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public ICommand OpenWebsiteCommand { get; }

        public AboutViewModel(INavigation navigation) : base(navigation)
        {
            Title = "About";
            OpenWebsiteCommand = new Command<string>(OpenWebsite);
        }

        private async void OpenWebsite(string param)
        {
            if (param == "GD")
                await Xamarin.Essentials.Browser.OpenAsync("https://www.flaticon.com/authors/good-ware", Xamarin.Essentials.BrowserLaunchMode.External);
            else if (param == "FL")
                await Xamarin.Essentials.Browser.OpenAsync("https://www.flaticon.com/", Xamarin.Essentials.BrowserLaunchMode.External);
            else
                await Xamarin.Essentials.Browser.OpenAsync("https://muntadhar.net", Xamarin.Essentials.BrowserLaunchMode.External);
        }
    }
}
