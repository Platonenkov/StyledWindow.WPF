using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;
using MathCore.WPF.Commands;

namespace StyledWindow.WPF.Commands
{
    /// <summary> Команда загрузки темы </summary>
    public class LoadThemeCommand : Command
    {
        #region Overrides of Command

        public override async void Execute(object parameter)
        {
            await ThemeEx.LoadThemeAsync(parameter is string { } file_path ? file_path : null);
        }

        #endregion
    }

    public static partial class ThemeEx
    {
        /// <summary> загрузка темы </summary>
        public static async Task LoadThemeAsync(string filePath)
        {
            var theme_file = "ThemeSettings.json";
            if (!filePath.IsNullOrWhiteSpace())
                theme_file = filePath;

            var paletteHelper = new PaletteHelper();
            if (File.Exists(theme_file))
            {
                var file = new FileInfo(theme_file!);
                if (!file.Exists) return;
                try
                {
                    var time_out_count = 0;
                    while (file.IsLocked() && time_out_count < 100)
                    {
                        await Task.Delay(300).ConfigureAwait(false);
                        time_out_count++;
                    }

                    using var json_file = File.OpenRead(theme_file!);
                    var theme = await JsonSerializer.DeserializeAsync<Theme>(json_file);
                    if (theme is not null)
                        paletteHelper.SetTheme(theme);
                }
                catch (Exception)
                { }
            }
            else
            {
                await ThemeEx.SaveThemeAsync(theme_file).ConfigureAwait(false);
            }

        }
    }
}