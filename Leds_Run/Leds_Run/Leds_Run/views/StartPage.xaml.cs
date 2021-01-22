using System;
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
            ImgLeaderboard.ImageSource = ImageSource.FromResource("Leds_Run.Assets.LeaderbordIcon.png");
            Start.Source = ImageSource.FromResource("Leds_Run.Assets.StartButton.png");
            ShowWorkouts();
            //PckrWorkout.ItemsSource = ;
        }

        private async void ShowWorkouts()
        {
            defaultWorkouts = await RepoWorkout.GetDefaultWorkouts();

            foreach(Workout.Interval inter in defaultWorkouts)
            {
                WorkoutNames.Add(inter.Name);
            }
            PckrWorkout.ItemsSource = WorkoutNames;
        }
        private void Start_Clicked(object sender, EventArgs e)
        {
            //Kijken wat er geselecteerd is in PckrWorkout en meegeven naar de start workout pagina
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