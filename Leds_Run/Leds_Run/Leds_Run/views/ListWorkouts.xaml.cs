using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListWorkouts : ContentPage
    {
        public ListWorkouts()
        {
            InitializeComponent();

            string user = LoggedInUser();

            if (user != null)
            {
                ShowCustomWorkouts(user);
            }

            ShowChallenges();
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

            // CHALLENGES STAAN NU STATIC IN XAML
        }

        private async void ShowCustomWorkouts(string user)
        {
            // haal custom workouts op en toon ze
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            if (LoggedInUser() != null)
            {
                //Navigation.PushAsync(new NavigationPage(new NewWorkout()));
            }
            else
            {
                Navigation.PushAsync(new NavigationPage(new Login()));
            }
        }
    }
}