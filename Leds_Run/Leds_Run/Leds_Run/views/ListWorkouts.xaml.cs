using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Leds_Run.repositories;
using Leds_Run.models;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListWorkouts : ContentPage
    {
        public ListWorkouts()
        {
            InitializeComponent();

            ShowChallenges();
            ShowDefaultWorkouts();
        }

        private string LoggedInUser()
        {
            if (Application.Current.Properties.ContainsKey("user"))
            {
                return Application.Current.Properties["user"] as string;              
            }
            else
            {
                return null;
            }  
        }

        private async void ShowChallenges()
        {
            // haal challenges op en toon ze
            List<Workout.Interval> challenges = await RepoWorkout.GetChallenges();

            // frontend binding magic:

            foreach (Workout.Interval interval in challenges)
            {
                Frame frame = new Frame();
                Label label = new Label();
                label.TextColor = Color.FromHex("#212121");
                label.BackgroundColor = Color.FromHex("#FFFFFF");
                label.FontSize = 14;
                label.Text = interval.Name;
                frame.Content = label;
                stckLayoutChallenges.Children.Add(frame);
            }
        }

        private async void ShowDefaultWorkouts()
        {
            List<Workout.Interval> defaultWorkouts = await RepoWorkout.GetDefaultWorkouts();

            // frontend binding magic:

            foreach (Workout.Interval interval in defaultWorkouts)
            {
                Frame frame = new Frame();
                Label label = new Label();
                label.TextColor = Color.FromHex("#212121");
                label.BackgroundColor = Color.FromHex("#FFFFFF");
                label.FontSize = 14;
                label.Text = interval.Name;
                frame.Content = label;
                stckLayoutDefaultWorkouts.Children.Add(frame);
            }
        }

        private async void ShowCustomWorkouts(string user)
        {
            // haal custom workouts op en toon ze
            List<Workout> workouts = await RepoWorkout.GetCostumWorkouts(user);

            // frontend binding magic:

            foreach(Workout workout in workouts)
            {
                Frame frame = new Frame();
                Label label = new Label();
                label.Text = workout.Intervals[0].Name;
                label.TextColor = Color.FromHex("#212121");
                label.BackgroundColor = Color.FromHex("#FFFFFF");
                label.FontSize = 14;
                frame.Content = label;
                stckLayoutCustomWorkouts.Children.Add(frame);
            }

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            if (LoggedInUser() != null)
            {
                Application.Current.MainPage.Navigation.PushAsync(new NewWorkout());
            }
            else
            {
                Navigation.PushAsync(new NavigationPage(new Login()));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string user = LoggedInUser();

            if (user != null)
            {
                ShowCustomWorkouts(user);
            }
        }
    }
}