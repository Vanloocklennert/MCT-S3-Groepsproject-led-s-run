﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leds_Run.models;
using Leds_Run.repositories;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartWorkoutPage : ContentPage
    {
        bool pause = false;
        string Username;
        TimeSpan totalTime = TimeSpan.FromSeconds(0);
        TimeSpan time;
        Workout workout;
        Workout.Interval intervals = new Workout.Interval();
        public StartWorkoutPage(Workout workouts, Color color, string username)
        {
            Username = username;
            workout = workouts;
            InitializeComponent();
            Timer(workouts);
            lblColor.BackgroundColor = color;
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
                        loop += 1;
                        if (loops <= loop)
                        {
                            Workouts = false;
                            Console.WriteLine("end");
                        }
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

        private async void btnStop_Clicked(object sender, EventArgs e)
        {
            if(workout.Intervals[0].Name == "Tiger" || workout.Intervals[0].Name == "Usain Bolt")
            {
                string speed = ((double)workout.Intervals[0].Distance / time.Seconds).ToString();
                await RepoWorkout.CreateLeaderBoardEntry(Username, time.ToString("c"), workout.Intervals[0].Distance.ToString(), speed);
            }

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WorkoutDetail(workout));
        }
    }
}