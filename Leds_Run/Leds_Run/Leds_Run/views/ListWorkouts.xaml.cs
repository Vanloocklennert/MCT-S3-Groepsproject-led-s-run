﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListWorkouts : ContentPage
    {
        public ListWorkouts()
        {
            InitializeComponent();

            int i = 0;
        }
    }
}