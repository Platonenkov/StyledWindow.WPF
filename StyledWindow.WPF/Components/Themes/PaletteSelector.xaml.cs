using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

using MaterialDesignColors;

using MaterialDesignThemes.Wpf;

using MathCore.WPF.Commands;
using MathCore.WPF.ViewModels;

namespace StyledWindow.WPF.Components.Themes
{
    /// <summary>
    /// Логика взаимодействия для PaletteSelector.xaml
    /// </summary>
    public partial class PaletteSelector : UserControl
    {
        public PaletteSelectorViewModel PaletteModel { get; private set; } = new PaletteSelectorViewModel();
        public PaletteSelector()
        {
            InitializeComponent();
        }
        public class PaletteSelectorViewModel : ViewModel
        {
            public PaletteSelectorViewModel()
            {
                Swatches = new SwatchesProvider().Swatches;
            }

            public IEnumerable<Swatch> Swatches { get; }

            public ICommand ApplyPrimaryCommand { get; } = Command.New(o => ApplyPrimary((Swatch)o));

            private static void ApplyPrimary(Swatch swatch)
                => ModifyTheme(theme => theme.SetPrimaryColor(swatch.ExemplarHue.Color));

            public ICommand ApplyAccentCommand { get; } = Command.New(o => ApplyAccent((Swatch)o));

            private static void ApplyAccent(Swatch swatch)
            {
                if (swatch is { AccentExemplarHue: not null })
                {
                    ModifyTheme(theme => theme.SetSecondaryColor(swatch.AccentExemplarHue.Color));
                }
            }

            private static void ModifyTheme(Action<ITheme> modificationAction)
            {
                var paletteHelper = new PaletteHelper();
                ITheme theme = paletteHelper.GetTheme();

                modificationAction?.Invoke(theme);

                paletteHelper.SetTheme(theme);
            }
        }

    }
}
