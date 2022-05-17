using System;
using System.Linq;
using System.Windows;
using Localization.WPF;
using StyledWindow.WPF.Commands;
using TestApp.WPF.Properties;

namespace TestApp.WPF
{
    public partial class App
    {
        public App()
        {
            //ThemeEx.ChangeCulture += Action<string>;
            SetCulture();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await ThemeEx.LoadThemeAsync(null);
            //var load_com = new LoadThemeCommand();
            //load_com.Execute(null);
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await ThemeEx.SaveThemeAsync(null).ConfigureAwait(true);

            //var save_com = new SaveThemeCommand();
            //save_com.Execute(null);

            base.OnExit(e);
        }

        /// <summary>Set current culture settings</summary>
        private static void SetCulture()
        {
            ThemeEx.ChangeCulture += LocalizationManager.ChangeCulture;

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
