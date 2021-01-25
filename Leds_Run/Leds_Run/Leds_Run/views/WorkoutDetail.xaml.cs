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
            foreach(Workout.Interval interval in workout.Intervals)
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
                grid.Children.Add(new Label { Text = typenum, VerticalOptions = LayoutOptions.End }, 0, 1);
                grid.Children.Add(new Label { Text = type, VerticalOptions = LayoutOptions.Start }, 0, 2);
                grid.Children.Add(new Label { Text =  interval.Speed.ToString(), VerticalOptions = LayoutOptions.End }, 0, 3);
                grid.Children.Add(new Label { Text = "km/h", VerticalOptions = LayoutOptions.Start }, 1, 3);
                stackInterval.Children.Add(grid);
            }
        }
    }
}