using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartWorkoutPage : ContentPage
    {
        bool pause = false;
        public StartWorkoutPage()
        {
            InitializeComponent();
            FillStartWorkout();
            Timer();
        }

        private async void FillStartWorkout()
        {
            //Frame Collor

            //Title workout
            Title = "Name Workout";
        }

        private async void Timer()
        {
            DateTime Start = DateTime.Now;
            TimeSpan time;

            Device.StartTimer(new TimeSpan(0,0,0,0,5), () =>
            {
                if (pause){
                    Start = DateTime.Now - time;
                }
                else
                {
                    time = DateTime.Now - Start;

                    lblTimer.Text = time.Minutes + ":" + time.Seconds + ":" + time.Milliseconds;
                }
                return true;
            });

        }

        private void btnPlay_Clicked(object sender, EventArgs e)
        {
            pause = false;
            btnPause.IsEnabled = true;
            btnPause.IsVisible = true;
            btnPlay.IsEnabled = false;
            btnPlay.IsVisible = false;
            btnStop.IsEnabled = false;
            btnStop.IsVisible = false;
        }

        private void btnPause_Clicked(object sender, EventArgs e)
        {
            pause = true;
            btnPause.IsEnabled = false;
            btnPause.IsVisible = false;
            btnPlay.IsEnabled = true;
            btnPlay.IsVisible = true;
            btnStop.IsEnabled = true;
            btnStop.IsVisible = true;
        }

        private void btnStop_Clicked(object sender, EventArgs e)
        {

        }
    }
}