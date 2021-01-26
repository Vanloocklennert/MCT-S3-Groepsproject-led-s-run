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
    public partial class NewWorkout : ContentPage
    {
        List<Workout.Interval> ListIntervals;
        public NewWorkout()
        {
            InitializeComponent();
            ListIntervals = new List<Workout.Interval>();
            NewWorkout_Subscribe();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //Add intervall
            Application.Current.MainPage = new NavigationPage(new NewWorkout());
            Application.Current.MainPage.Navigation.PushAsync(new Interval());
        }

        private void btnAddWorkout_Clicked(object sender, EventArgs e)
        {
            Workout workout = new Workout();
            workout.Intervals = new List<Workout.Interval>();
            workout.Intervals.AddRange(ListIntervals);

            //add new workout
            RepoWorkout.CreateWorkout(Application.Current.Properties["user"].ToString(), workout);
            //go back to start
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void FillNewWorkout(Workout.Interval intervals)
        {
            
            ListIntervals.Add(intervals);
            stackWorkout.Children.Clear();
            string TimeDist;
            string NumMinMeter;
            string MinMeter;
            foreach (Workout.Interval interval in ListIntervals)
            {
                if (interval.Type == "rundistancespeed")
                {
                    TimeDist = "DISTANCE";
                    NumMinMeter = interval.Distance.ToString();
                    MinMeter = "m";
                }
                else
                {
                    TimeDist = "TIME";
                    NumMinMeter = interval.Time.TotalMinutes.ToString();
                    MinMeter = "min";
                }
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
                grid.Children.Add(new Label { Text = TimeDist, HorizontalTextAlignment = TextAlignment.End }, 0, 0);
                grid.Children.Add(new Label { Text = NumMinMeter, HorizontalTextAlignment = TextAlignment.End }, 1, 0);
                grid.Children.Add(new Label { Text = MinMeter, HorizontalTextAlignment = TextAlignment.Start }, 2, 0);
                grid.Children.Add(new Label { Text = (interval.Speed*3.6).ToString(), HorizontalTextAlignment = TextAlignment.End }, 3, 0);
                grid.Children.Add(new Label { Text = "km/h", HorizontalOptions = LayoutOptions.Start }, 4, 0);
                stackWorkout.Children.Add(grid);
            }
        }

         private void NewWorkout_Subscribe()
        {
            MessagingCenter.Subscribe<Interval, Workout.Interval>(this, "interval", (s, a) =>
             {
                 FillNewWorkout(a);
             });
        }
    }
}