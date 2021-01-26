using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Leds_Run.models;
using Leds_Run.repositories;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartupPage : ContentPage
    {
        int time = 15;
        Color color;
        public StartupPage(Workout workout)
        {
            InitializeComponent();
            FillStartUp(workout);
            Timer(workout);
        }

        private async void FillStartUp(Workout workout)
        {

            List<Color> colorList = new List<Color>
            {
                Color.Red,
                Color.Blue,
                Color.Green,
                Color.Cyan,
                Color.DarkOrange,
                Color.DarkSeaGreen,
                Color.Indigo
            };

            Random random = new Random(); 
            color = colorList[random.Next(0, 6)];

            //Frame Collor
            frameColor.BackgroundColor = color;
            //Title workout
            Title = workout.Intervals[0].Name;
        }
        private async void Timer(Workout workout)
        {
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if(time < 0)
                {
                    RepoMqtt mqtt = new RepoMqtt();

                    List<object> objectList = new List<object> 
                    {  
                        color.ToHex(),
                        workout
                    };

                    string payload = JsonConvert.SerializeObject(objectList);

                    mqtt.PublishMessage(payload, Guid.NewGuid().ToString());
                    
                    Navigation.PushAsync(new StartWorkoutPage(workout, color));
                    return false;
                }
                lblTimer.Text =time.ToString();
                time -= 1;
                return true;
            });

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            time = 0;
        }
    }
}