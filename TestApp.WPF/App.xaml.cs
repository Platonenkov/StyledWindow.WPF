using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Localization.WPF;
using TestApp.WPF.Properties;

namespace TestApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SetCulture();
        }

        /// <summary>Set current culture settings</summary>
        private static void SetCulture()
        {
            LocalizationManager.CultureChanging += (s, e) => //Running when application culture is changed
            {
                var culture = e.NewCulture;
                TestApp.WPF.Properties.Resources.Culture = culture;
                //Insert other projects or libraries here
            };
            LocalizationManager.CultureChanged += (s, e) =>
            {
                //Here we save the settings that will be applied when the application starts
                Settings.Default.Culture = e.NewCulture.Name;
                Settings.Default.Save();
            };

            //Here we load the settings that was saved at last time
            var settings = Settings.Default;
            var last_culture = settings.Culture;
            var new_culture = last_culture == string.Empty ? "ru-RU" : last_culture;

            //or was sent in command line arguments
            var args = Environment.GetCommandLineArgs();
            if (args.Contains("-en")) new_culture = "en-US";
            else if (args.Contains("-rus")) new_culture = "ru-RU";

            LocalizationManager.ChangeCulture(new_culture);
        }

    }
}
