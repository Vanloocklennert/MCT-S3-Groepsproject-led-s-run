﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Leds_Run.models;
using Leds_Run.repositories;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {

        List<Workout.Interval> defaultWorkouts;
        List<string> WorkoutNames = new List<string>();

        public StartPage()
        {
            InitializeComponent();
            ShowWorkouts();
        }

        private async void ShowWorkouts()
        {         
            defaultWorkouts = await RepoWorkout.GetDefaultWorkouts();
            defaultWorkouts.AddRange(await RepoWorkout.GetChallenges());

            foreach(Workout.Interval inter in defaultWorkouts)
            {
                WorkoutNames.Add(inter.Name);
            }
            PckrWorkout.ItemsSource = WorkoutNames;
            PckrWorkout.SelectedIndex = 0;
        }
        private void Start_Clicked(object sender, EventArgs e)
        {
            Workout workout = new Workout();
            workout.Intervals = new List<Workout.Interval>();
            workout.Intervals.Add(defaultWorkouts[PckrWorkout.SelectedIndex]);
            Navigation.PushAsync(new StartupPage(workout));
        }

        private void NewWorkout_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListWorkouts());
        }

        private void ImgLeaderboard_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NavigationPage(new LeaderboardPage()));
        }
    }
}