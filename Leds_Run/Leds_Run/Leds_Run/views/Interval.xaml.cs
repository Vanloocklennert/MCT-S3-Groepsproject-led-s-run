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
    public partial class Interval : ContentPage
    {
        public Interval()
        {
            InitializeComponent();
        }

        private void pcType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EntryDurDis.IsEnabled = true;
            if (pcType.SelectedItem == "runspeedtime")
            {
                lblDurDis.Text = "meter";
            }
            else
            {
                lblDurDis.Text = "minuten";
            }
        }
    }
}