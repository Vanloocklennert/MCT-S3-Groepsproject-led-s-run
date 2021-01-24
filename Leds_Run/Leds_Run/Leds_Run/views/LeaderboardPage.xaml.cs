using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Leds_Run.repositories;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaderboardPage : ContentPage
    {
        public LeaderboardPage()
        {
            InitializeComponent();
            ShowLeaderboard();
        }

        private async void ShowLeaderboard()
        {
            var test = await RepoWorkout.GetLeaderboard();
        }
    }
}