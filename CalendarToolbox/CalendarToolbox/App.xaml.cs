using System.Threading.Tasks;
using CalendarToolbox.Models.Consts;
using CalendarToolbox.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CalendarToolbox
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<DeviceCalendarsDataStore>();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            await InitAppCenterLogging();
        }

        private static async Task InitAppCenterLogging()
        {
            bool showCrashLog = true;
            string key = Keys.MicrosoftAppCenterDebugKey;
#if RELEASE
            showCrashLog = false;
            if (DeviceInfo.DeviceType != DeviceType.Virtual)
            {
                key = Keys.MicrosoftAppCenterKey;
            }
#endif
            AppCenter.Start(key, typeof(Analytics), typeof(Crashes));

            // Display crash information
            if (showCrashLog && await Crashes.HasCrashedInLastSessionAsync())
            {
                var report = await Crashes.GetLastSessionCrashReportAsync();
                await Shell.Current.DisplayAlert("Error details", report.StackTrace, "Ok");
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
