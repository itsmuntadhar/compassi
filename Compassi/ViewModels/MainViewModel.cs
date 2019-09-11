using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Compassi.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private double angle;
        /// <summary>
        /// Angle to rotate the "needle"
        /// </summary>
        public double Angle
        {
            get => angle;
            set => SetProperty(ref angle, value);
        }

        private double requestLocationBearing;
        public double RequestLocationBearing
        {
            get => requestLocationBearing;
            set => SetProperty(ref requestLocationBearing, value);
        }

        private double north;
        public double North
        {
            get => north;
            set => SetProperty(ref north, value);
        }

        private string toggleButtonText = "Start";
        public string ToggleButtonText
        {
            get => toggleButtonText;
            set => SetProperty(ref toggleButtonText, value);
        }

        private Location location;
        /// <summary>
        /// current user's location
        /// </summary>
        public Location Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }

        private double targetLat;
        public double TargetLat
        {
            get => targetLat;
            set => SetProperty(ref targetLat, value);
        }

        private double targetLon;
        public double TargetLon
        {
            get => targetLon;
            set => SetProperty(ref targetLon, value);
        }

        public ICommand ToggleCompassCommand { get; }
        public ICommand GetCurrentLocationCommand { get; }
        public ICommand AboutCommand { get; }

        public ImageSource NeedleSource { get; }

        public MainViewModel(INavigation navigation) : base(navigation)
        {
            Title = "Compassi";

            ToggleCompassCommand = new Command(ToggleCompass);
            GetCurrentLocationCommand = new Command(GetCurrentLocation);
            AboutCommand = new Command(About);
            NeedleSource = ImageSource.FromResource("Compassi.Images.needle.png");

            GetCurrentLocation();
            ToggleCompass();
        }

        private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            if (Location == null)
                Angle = 0;
            else
            {
                North = e.Reading.HeadingMagneticNorth;
                RequestLocationBearing = DegreeBearing(Location.Latitude, Location.Longitude, TargetLat, TargetLon);
                Angle = RequestLocationBearing - North;
            }
        }

        private void ToggleCompass()
        {
            if (Compass.IsMonitoring)
            {
                Compass.Stop();
                Compass.ReadingChanged -= Compass_ReadingChanged;
            }
            else
            {
                try
                {
                    Compass.Start(SensorSpeed.UI, true);
                    Compass.ReadingChanged += Compass_ReadingChanged;
                }
                catch (FeatureNotSupportedException fex)
                {

                }
                catch (FeatureNotEnabledException nex)
                {

                }
                catch (Exception ex)
                {

                }
            }
            ToggleButtonText = Compass.IsMonitoring ? "Stop" : "Start";

        }

        private async void GetCurrentLocation()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            Location = await Geolocation.GetLocationAsync();
            IsBusy = false;
        }

        private async void About()
        {
            await Navigation.PushAsync(new Views.AboutView());
        }

        double DegreeBearing(double lat1, double lon1, double lat2, double lon2)
        {
            var dLon = ToRad(lon2 - lon1);
            var dPhi = Math.Log(
                Math.Tan(ToRad(lat2) / 2 + Math.PI / 4) / Math.Tan(ToRad(lat1) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return ToBearing(Math.Atan2(dLon, dPhi));
        }

        public static double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }

        public static double ToBearing(double radians)
        {
            // convert radians to degrees (as bearing: 0...360)
            return (ToDegrees(radians) + 360) % 360;
        }
    }
}
