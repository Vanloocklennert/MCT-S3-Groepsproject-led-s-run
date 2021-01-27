using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Leds_Run.models;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutDetail : ContentPage
    {
        public WorkoutDetail(Workout workout)
        {
            InitializeComponent();
            FullDetails(workout);
            
        }

        public async void FullDetails(Workout workout)
        {
            foreach (Workout.Interval interval in workout.Intervals)
            {
                Grid grid = new Grid();
                grid.HorizontalOptions = LayoutOptions.FillAndExpand;

                grid.ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{ } ,
                    new ColumnDefinition{ } ,
                    new ColumnDefinition{ } ,
                    new ColumnDefinition{ } ,
                    new ColumnDefinition{ }
                };
                string type;
                string typenum;
                if (interval.Type == "runspeedtime")
                {
                    type = "min";
                    typenum = interval.Time.TotalMinutes.ToString();
                }
                else
                {
                    type = "m";
                    typenum = interval.Distance.ToString();
                }

                grid.Children.Add(new Label { Text = interval.Type }, 0, 0);
                grid.Children.Add(new Label { Text = typenum, HorizontalOptions = LayoutOptions.End }, 1, 0);
                grid.Children.Add(new Label { Text = type, HorizontalOptions = LayoutOptions.Start }, 2, 0);
                grid.Children.Add(new Label { Text = (3.6*interval.Speed).ToString(), HorizontalOptions = LayoutOptions.End }, 3, 0);
                grid.Children.Add(new Label { Text = "km/h", HorizontalOptions = LayoutOptions.Start }, 4, 0);
                stackInterval.Children.Add(grid);
            }
        }
    }
}