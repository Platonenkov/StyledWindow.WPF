using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using MathCore.Annotations;
using MathCore.WPF.Commands;

namespace StyledWindow.WPF.Commands
{
    public class LoadThemeCommand : Command
    {
        #region Overrides of Command

        public override async void Execute(object? parameter)
        {
            var theme_file = "ThemeSettings.json";
            if (parameter is string { Length: >0 } file_name)
                theme_file = file_name;

            var paletteHelper = new PaletteHelper();
            if (File.Exists(theme_file))
            {
                var file = new FileInfo(theme_file);
                if (!file.Exists) return;
                try
                {
                    var time_out_count = 0;
                    while (file.IsLocked() && time_out_count < 100)
                    {
                        await Task.Delay(300).ConfigureAwait(false);
                        time_out_count++;
                    }

                    await using var json_file = File.OpenRead(theme_file);
                    var theme = await JsonSerializer.DeserializeAsync<Theme>(json_file);
                    if (theme is not null)
                        paletteHelper.SetTheme(theme);
                }
                catch (Exception)
                { }
            }
            else
            {
                var com = new SaveThemeCommand();
                com.Execute(theme_file);
            }
        }

        #endregion
    }
}