using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Leds_Run.models;
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
            Leaderboard leaderboard = await RepoWorkout.GetLeaderboard();
            lblPlace1.Text = leaderboard.entries[0].Username;
            lblPlace2.Text = leaderboard.entries[1].Username;
            lblPlace3.Text = leaderboard.entries[2].Username;

            int i = 1;

            foreach (Leaderboard.Entry entry in leaderboard.entries)
            {
                Grid grid = new Grid();
                grid.HorizontalOptions = LayoutOptions.FillAndExpand;

                grid.ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{ } ,
                    new ColumnDefinition{ } ,
                    new ColumnDefinition{ } ,
                    new ColumnDefinition{ } ,
                    new ColumnDefinition{ } ,
                    new ColumnDefinition{ }
                };

                grid.Children.Add(new Label { Text = i.ToString(), HorizontalTextAlignment = TextAlignment.Center }, 0, 0);
                grid.Children.Add(new Label { Text = entry.Username, HorizontalTextAlignment = TextAlignment.Center }, 0, 1);
                grid.Children.Add(new Label { Text = entry.Time.TotalSeconds.ToString(), HorizontalTextAlignment = TextAlignment.End }, 0, 2);
                grid.Children.Add(new Label { Text = "s", HorizontalTextAlignment = TextAlignment.Start }, 0, 3);
                grid.Children.Add(new Label { Text = entry.Speed.ToString(), HorizontalTextAlignment = TextAlignment.End }, 0, 4);
                grid.Children.Add(new Label { Text = "km/h", HorizontalOptions = LayoutOptions.Start }, 0, 5);
                stackDetail.Children.Add(grid);
                i++;
            }

        }
    }
}