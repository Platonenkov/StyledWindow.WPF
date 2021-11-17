using MathCore.WPF.Commands;

namespace StyledWindow.WPF.Commands
{
    public delegate void ChangeCultureHandler(string culture);
    /// <summary> Команда смены темы светлая-тёмная </summary>
    public class ChangeCultureCommand : Command
    {
        #region Overrides of Command

        /// <inheritdoc />
        public override void Execute(object parameter)
        {
            if (parameter is string { Length: > 0 } lang)
                ThemeEx.ChangeCultureAction(lang);
        }

        #endregion
    }

    public static partial class ThemeEx
    {
        public static event ChangeCultureHandler ChangeCulture;

        public static void ChangeCultureAction(string lang)
        {
            ChangeCulture?.Invoke(lang);
        }

    }
}
