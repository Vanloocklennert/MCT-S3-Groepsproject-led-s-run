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
        int time = 15;
        public StartupPage(Workout workout)
        {
            InitializeComponent();
            FillStartUp(workout);
            Timer(workout);
        }

        private async void FillStartUp(Workout workout)
        {
            //Frame Collor

            //labal color

            //Title workout
            Title = workout.Intervals[0].Name;
        }
        private async void Timer(Workout workout)
        {
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if(time < 0)
                {
                    Navigation.PushAsync(new StartWorkoutPage(workout));
                    return false;
                }
                lblTimer.Text =time.ToString();
                time -= 1;
                return true;
            });

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            time = 0;
        }
    }
}