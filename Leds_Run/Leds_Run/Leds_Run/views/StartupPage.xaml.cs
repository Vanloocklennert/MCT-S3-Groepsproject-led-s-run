using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Leds_Run.models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartupPage : ContentPage
    {
        public StartupPage()
        {
            InitializeComponent();
            FillStartUp();
            Timer();
        }

        private async void FillStartUp()
        {
            //Frame Collor

            //labal color

            //Title workout
            Title = "Name Workout";
        }
        private async void Timer()
        {
            int time = 15;
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if(time == 0)
                {
                    //Navigation.PushAsync(new StartWorkoutPage());
                    return false;
                }
                lblTimer.Text =time.ToString();
                time -= 1;
                return true;
            });

        }
    }
}