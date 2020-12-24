using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinForms.JavaScriptInterpreter
{
    public partial class MainPage : ContentPage
    {
        public MainViewModel MainViewModel => BindingContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        void Picker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            MainViewModel.ScriptContent = MainViewModel.SelectedScript.Content;
        }
    }
}
