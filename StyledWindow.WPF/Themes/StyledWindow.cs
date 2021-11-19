using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using MaterialDesignThemes.Wpf;
using StyledWindow.WPF.Components;

namespace StyledWindow.WPF.Themes
{
    public partial class StyledWindow
    {
        #region Attached property TopmostButtonVisible : bool - Видимость кнопки "Поверх всех окон"

        /// <summary>Видимость кнопки "Поверх всех окон"</summary>
        public static readonly DependencyProperty TopmostButtonVisibleProperty =
            DependencyProperty.RegisterAttached(
                "TopmostButtonVisible",
                typeof(bool),
                typeof(StyledWindow),
                new PropertyMetadata(true));

        /// <summary>Видимость кнопки "Поверх всех окон"</summary>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetTopmostButtonVisible(DependencyObject d, bool value) => d.SetValue(TopmostButtonVisibleProperty, value);

        /// <summary>Видимость кнопки "Поверх всех окон"</summary>
        public static bool GetTopmostButtonVisible(DependencyObject d) => (bool)d.GetValue(TopmostButtonVisibleProperty);

        #endregion

        #region Attached property HeaderContent : object - Содержимое заголовка окна

        /// <summary>Содержимое заголовка окна</summary>
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.RegisterAttached(
                "HeaderContent",
                typeof(object),
                typeof(StyledWindow),
                new PropertyMetadata(default(object)));

        /// <summary>Содержимое заголовка окна</summary>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHeaderContent(DependencyObject d, object value) => d.SetValue(HeaderContentProperty, value);

        /// <summary>Содержимое заголовка окна</summary>
        public static object GetHeaderContent(DependencyObject d) => (object)d.GetValue(HeaderContentProperty);

        #endregion

        #region Attached property LanguageContent : IEnumerable<LanguageInfo> - Список языков

        /// <summary>Список языков</summary>
        public static readonly DependencyProperty LanguageContentProperty =
            DependencyProperty.RegisterAttached(
                "LanguageContent",
                typeof(IEnumerable<LanguageInfo>),
                typeof(StyledWindow),
                new PropertyMetadata(Array.Empty<LanguageInfo>()));

        /// <summary>Список языков</summary>
        public static void SetLanguageContent(DependencyObject element, IEnumerable<LanguageInfo> value) => element.SetValue(LanguageContentProperty, value);

        /// <summary>Список языков</summary>
        public static IEnumerable<LanguageInfo> GetLanguageContent(DependencyObject element) => (IEnumerable<LanguageInfo>)element.GetValue(LanguageContentProperty);

        #endregion

        #region Attached property LanguageButtonVisible : bool - Видимость кнопки "Language"

        /// <summary>Видимость кнопки "Language"</summary>
        public static readonly DependencyProperty LanguageButtonVisibleProperty =
            DependencyProperty.RegisterAttached(
                "LanguageButtonVisible",
                typeof(bool),
                typeof(StyledWindow),
                new PropertyMetadata(true));

        /// <summary>Видимость кнопки "Language"</summary>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetLanguageButtonVisible(DependencyObject d, bool value) => d.SetValue(LanguageButtonVisibleProperty, value);

        /// <summary>Видимость кнопки "Language"</summary>
        public static bool GetLanguageButtonVisible(DependencyObject d) => (bool)d.GetValue(LanguageButtonVisibleProperty);

        #endregion
        #region Attached property ThemeButtonVisible : bool - Видимость кнопки "Theme"

        /// <summary>Видимость кнопки "Theme"</summary>
        public static readonly DependencyProperty ThemeButtonVisibleProperty =
            DependencyProperty.RegisterAttached(
                "ThemeButtonVisible",
                typeof(bool),
                typeof(StyledWindow),
                new PropertyMetadata(true));

        /// <summary>Видимость кнопки "Theme"</summary>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetThemeButtonVisible(DependencyObject d, bool value) => d.SetValue(ThemeButtonVisibleProperty, value);

        /// <summary>Видимость кнопки "Theme"</summary>
        public static bool GetThemeButtonVisible(DependencyObject d) => (bool)d.GetValue(ThemeButtonVisibleProperty);

        #endregion
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

        public StyledWindow()
        {
            //var Position = Taskbar.Position;
            //var AlwaysOnTop = Taskbar.AlwaysOnTop ;
            //var AutoHide = Taskbar.AutoHide;
            //var CurrentBounds = Taskbar.CurrentBounds;
            //var DisplayBounds = Taskbar.DisplayBounds;
            //var Handle = Taskbar.Handle;
        }



    }
    public class KnownLanguages
        : List<LanguageInfo>
    {
    }

}
