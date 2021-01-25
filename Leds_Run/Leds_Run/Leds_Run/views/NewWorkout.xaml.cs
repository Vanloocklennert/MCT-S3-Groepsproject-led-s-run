using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewWorkout : ContentPage
    {
        public NewWorkout()
        {
            InitializeComponent();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            lblRepet.IsEnabled = !lblRepet.IsEnabled; 
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //Add intervall

        }
    }
}