using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MaterialDesignThemes.Wpf;

namespace StyledWindow.WPF.Components
{
    /// <summary>
    /// Логика взаимодействия для ThemeButton.xaml
    /// </summary>
    public partial class ThemeButton : UserControl
    {
        public ThemeButton()
        {
            InitializeComponent();
        }
        #region Theme

        private void _themeBtn_OnLoaded(object Sender, RoutedEventArgs E)
        {
            if (Sender is not ToggleButton btn) return;
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            var base_theme = theme.GetBaseTheme();
            btn.IsChecked = base_theme is BaseTheme.Dark;
        }

        #endregion

    }
}
