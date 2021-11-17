using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using MathCore.WPF.ViewModels;

namespace StyledWindow.WPF.Components.Themes
{
    /// <summary>
    /// Логика взаимодействия для ThemeSettings.xaml
    /// </summary>
    public partial class ThemeSettings : UserControl
    {
        public ThemeSettingsViewModel ThemeModel { get; private set; } = new ThemeSettingsViewModel();
        public ThemeSettings()
        {
            InitializeComponent();
        }
        public class ThemeSettingsViewModel : ViewModel
        {

            public ThemeSettingsViewModel()
            {
                var paletteHelper = new PaletteHelper();
                var theme = paletteHelper.GetTheme();
                var base_theme = theme.GetBaseTheme();
                _isDarkTheme = base_theme is BaseTheme.Dark;
            }

            private bool _isDarkTheme;
            public bool IsDarkTheme
            {
                get => _isDarkTheme;
                set
                {
                    if (Set(ref _isDarkTheme, value))
                    {
                        ModifyTheme(theme => theme.SetBaseTheme(value ? Theme.Dark : Theme.Light));
                    }
                }
            }

            private bool _isColorAdjusted;
            public bool IsColorAdjusted
            {
                get => _isColorAdjusted;
                set
                {
                    if (Set(ref _isColorAdjusted, value))
                    {
                        ModifyTheme(theme =>
                        {
                            if (theme is Theme internalTheme)
                            {
                                internalTheme.ColorAdjustment = value
                                    ? new ColorAdjustment
                                    {
                                        DesiredContrastRatio = DesiredContrastRatio,
                                        Contrast = ContrastValue,
                                        Colors = ColorSelectionValue
                                    }
                                    : null;
                            }
                        });
                    }
                }
            }

            private float _desiredContrastRatio = 4.5f;
            public float DesiredContrastRatio
            {
                get => _desiredContrastRatio;
                set
                {
                    if (Set(ref _desiredContrastRatio, value))
                    {
                        ModifyTheme(theme =>
                        {
                            if (theme is Theme { ColorAdjustment: { } } internalTheme)
                                internalTheme.ColorAdjustment.DesiredContrastRatio = value;
                        });
                    }
                }
            }

            public IEnumerable<Contrast> ContrastValues => Enum.GetValues(typeof(Contrast)).Cast<Contrast>();

            private Contrast _contrastValue;
            public Contrast ContrastValue
            {
                get => _contrastValue;
                set
                {
                    if (Set(ref _contrastValue, value))
                    {
                        ModifyTheme(theme =>
                        {
                            if (theme is Theme { ColorAdjustment: { } } internalTheme)
                                internalTheme.ColorAdjustment.Contrast = value;
                        });
                    }
                }
            }

            public IEnumerable<ColorSelection> ColorSelectionValues => Enum.GetValues(typeof(ColorSelection)).Cast<ColorSelection>();

            private ColorSelection _colorSelectionValue;
            public ColorSelection ColorSelectionValue
            {
                get => _colorSelectionValue;
                set
                {
                    if (Set(ref _colorSelectionValue, value))
                    {
                        ModifyTheme(theme =>
                        {
                            if (theme is Theme { ColorAdjustment: { } } internalTheme)
                                internalTheme.ColorAdjustment.Colors = value;
                        });
                    }
                }
            }

            private static void ModifyTheme(Action<ITheme> modificationAction)
            {
                var paletteHelper = new PaletteHelper();
                var theme = paletteHelper.GetTheme();

                modificationAction?.Invoke(theme);

                paletteHelper.SetTheme(theme);
            }
        }

    }
}
