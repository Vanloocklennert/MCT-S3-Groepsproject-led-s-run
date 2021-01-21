using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {


        public StartPage()
        {
            InitializeComponent();
            ImgLeaderboard.ImageSource = ImageSource.FromResource("Leds_Run.Assets.LeaderbordIcon.png");
            Start.Source = ImageSource.FromResource("Leds_Run.Assets.StartButton.png");
            //Itemsource vand PckrWorkout invullen
            //PckrWorkout.ItemsSource = ;
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
            //naar de leaderboard gaan
        }
    }
}