using MaterialDesignThemes.Wpf;
using MathCore.WPF.Commands;

namespace StyledWindow.WPF.Commands
{
    public class ChangeThemeCommand : Command
    {
        #region Overrides of Command

        public override void Execute(object? parameter)
        {
            if (parameter is not bool isDarkTheme) return;
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
        }

        #endregion
    }
}