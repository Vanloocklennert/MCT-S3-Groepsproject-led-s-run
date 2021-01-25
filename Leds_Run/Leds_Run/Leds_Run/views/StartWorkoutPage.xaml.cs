using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leds_Run.models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartWorkoutPage : ContentPage
    {
        bool pause = false;
        TimeSpan totalTime = TimeSpan.FromSeconds(0);
        Workout workout;
        Workout.Interval intervals = new Workout.Interval();
        public StartWorkoutPage(Workout workouts)
        {
            workout = workouts;
            InitializeComponent();
            Timer(workouts);
        }

        private async void FillStartWorkout(Workout.Interval interval)
        {
            //Frame Color
            lblPace.Text = interval.Speed + " km/h";
            lblDistance.Text = interval.Distance.ToString();

            //Title workout
            Title = interval.Name;
        }

        private async void Timer(Workout workout)
        {
            DateTime Start = DateTime.Now;
            TimeSpan time;
            int loops = workout.Intervals.Count();
            int loop = 0;
            bool Workouts = true;

            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 5), () =>
            {
                if (totalTime < time)
                {
                    if(Workouts)
                    {
                        totalTime += workout.Intervals[loop].Time;
                        Console.WriteLine(workout.Intervals);
                        FillStartWorkout(workout.Intervals[loop]);
                        if (loops <= loop)
                        {
                            Workouts = false;
                            Console.WriteLine("end");
                        }
                        loop=+ 1;
                    }
                }

                if (pause)
                {
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WorkoutDetail(workout));
        }
    }
}