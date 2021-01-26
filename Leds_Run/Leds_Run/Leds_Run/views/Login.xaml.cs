using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leds_Run.repositories;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Leds_Run.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            if (Password.Text != "" || Email.Text != "")
            {
                bool canLogin = await RepoWorkout.GetUserLogin(Email.Text, Password.Text);

                if (canLogin)
                {
                    Application.Current.Properties["user"] = RepoWorkout.Hash(Email.Text + Password.Text);
                    await Navigation.PopAsync();
                }
                else 
                {
                    // info label: fout wachtwoord / username
                }
            }
            else
            {
                // info label: vul naam en wachtwoord in
            }
        }

        private async void BtnRegister_Clicked(object sender, EventArgs e)
        {
            if (Password.Text != "" || Email.Text != "" || Username.Text != "")
            {
                bool succes = await RepoWorkout.CreateUser(Username.Text, Email.Text, Password.Text);

                if (succes)
                {
                    Application.Current.Properties["user"] = RepoWorkout.Hash(Email.Text + Password.Text);
                    await Navigation.PopAsync();
                }
                else
                {
                    // infolabel: foutmelding
                }
            }
            else
            {
                // info label: vul naam en wachtwoord en username in
            }
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            Password.IsPassword = true;
        }
    }
}