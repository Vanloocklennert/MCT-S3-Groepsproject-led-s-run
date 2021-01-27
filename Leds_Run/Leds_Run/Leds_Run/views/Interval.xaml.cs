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
    public partial class Interval : ContentPage
    {
        public Interval()
        {
            InitializeComponent();
        }

        private void pcType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EntryDurDis.IsEnabled = true;
            if (pcType.SelectedItem == "Distance")
            {
                lblDurDis.Text = "meter";
            }
            else
            {
                lblDurDis.Text = "minuten";
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            double speed = Convert.ToDouble(EntryPace.Text);
            string type;
            double Distance, Speed;
            TimeSpan time;
            if (EntryDurDis.IsEnabled)
            {
                if(speed != 0)
                {
                    if (pcType.SelectedItem == "Distance")
                    {
                        type = "rundistancespeed";
                        Speed = Convert.ToDouble(EntryPace.Text)/3.6;
                        Distance = Convert.ToDouble(EntryDurDis.Text);
                        time = time.Add(TimeSpan.FromSeconds(Distance / Speed));
                    }
                    else
                    {
                        type = "runspeedtime";
                        Speed = Convert.ToDouble(EntryPace.Text)/3.6;
                        time = time.Add(TimeSpan.FromMinutes(Convert.ToDouble(EntryDurDis.Text)));
                        Distance = Speed*time.TotalSeconds;

                    }
                    Workout.Interval interval = new Workout.Interval();
                    interval.Type = type;
                    interval.Speed = Speed;
                    interval.Distance = Distance;
                    interval.Time = time;
                    interval.Name = "";

                    MessagingCenter.Send(this, "interval", interval);
                    Application.Current.MainPage.Navigation.PopAsync();
                }
                
            }
        }
    }
}