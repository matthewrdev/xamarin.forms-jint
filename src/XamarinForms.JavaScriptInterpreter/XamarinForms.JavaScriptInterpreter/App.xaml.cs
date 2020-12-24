using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinForms.JavaScriptInterpreter.Themes;

namespace XamarinForms.JavaScriptInterpreter
{
    public partial class App : Application
    {
        public static App Instance => App.Current as App;

        public string themeName => (string)Resources["themeName"];
        public App()
        {
            InitializeComponent();
            LoadTheme("Light");

            MainPage = new MainPage();
        }

        public void LoadTheme(string themeName)
        {
            switch (themeName.ToLower())
            {
                case "light":
                    SetTheme<LightTheme>("Light");
                    break;
                case "dark":
                    SetTheme<DarkTheme>("Dark");
                    break;
            }
        }

        private void SetTheme<TTheme>(string themeName) where TTheme : ResourceDictionary, new()
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                mergedDictionaries.Add(new TTheme());
                Application.Current.Resources["themeName"] = themeName;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
