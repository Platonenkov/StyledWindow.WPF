using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using MathCore.Annotations;
using MathCore.WPF.Commands;

namespace StyledWindow.WPF.Commands
{
    public class SaveThemeCommand : Command
    {
        #region Overrides of Command

        public override async void Execute(object? parameter)
        {
            var theme_file = "ThemeSettings.json";
            if (parameter is string { Length: >0 } file_name)
                theme_file = file_name;

            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            try
            {
                var file = new FileInfo(theme_file);
                if (file.Exists && file.IsLocked())
                {
                    var time_out_count = 0;
                    while (file.IsLocked() && time_out_count < 100)
                    {
                        await Task.Delay(300).ConfigureAwait(false);
                        time_out_count++;
                    }
                }

                await using var json_file = File.Create(theme_file);
                await JsonSerializer.SerializeAsync(json_file, theme).ConfigureAwait(false);
            }
            catch (Exception)
            { }
        }

        #endregion
    }
    public static partial class ThemeEx
    {
        public static async Task SaveThemeAsync(string filePath)
        {
            var theme_file = "ThemeSettings.json";
            if (!filePath.IsNullOrWhiteSpace())
                theme_file = filePath;

            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            try
            {
                var file = new FileInfo(theme_file);
                if (file.Exists && file.IsLocked())
                {
                    var time_out_count = 0;
                    while (file.IsLocked() && time_out_count < 100)
                    {
                        await Task.Delay(300).ConfigureAwait(false);
                        time_out_count++;
                    }
                }

                await using var json_file = File.Create(theme_file);
                await JsonSerializer.SerializeAsync(json_file, theme).ConfigureAwait(false);
            }
            catch (Exception)
            { }

        }
    }

}