# StyledWindow.WPF 
Библиотека с настроенным стилем окна для Windows WPF приложения.

`Install-Package StyledWindow.WPF -Version 6.0.0.1`

![Styled](https://github.com/Platonenkov/StyledWindow.WPF/blob/master/Resources/Header.png)
  <p >
<img src="https://github.com/Platonenkov/StyledWindow.WPF/blob/master/Resources/WIndowDark.png" alt="WIndowDark" width="400"/>
<img src="https://github.com/Platonenkov/StyledWindow.WPF/blob/master/Resources/WindowLight.png" alt="WindowLight" width="400"/>
</p>


### Что есть на титульном баре

* Стандартные кнопки: свернуть, на весь экран, закрыть
* Кнопка TopMost - сделать по верх всех окон
* Кнопка смены языка локализации приложения
* Кнопка настройки темы (светлая/тёмная темы и цвета шрифтов и панелей)
* Любой контент какой хотите отразить в Title
* Стандартное место под иконку

![Styled](https://github.com/Platonenkov/StyledWindow.WPF/blob/master/Resources/Styled.gif)
### Как настроить

#### 1. Настраиваем App.xaml

```xaml
...
             xmlns:materialDesign="http://materialdesigninxaml.net/winfx/xaml/themes"
             xmlns:themes="clr-namespace:StyledWindow.WPF.Themes;assembly=StyledWindow.WPF"
             xmlns:components="clr-namespace:StyledWindow.WPF.Components;assembly=StyledWindow.WPF"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <materialDesign:BundledTheme BaseTheme="Inherit" PrimaryColor="Amber" SecondaryColor="LightBlue"
                                             ColorAdjustment="{materialDesign:ColorAdjustment}" />
                <ResourceDictionary Source="pack://application:,,,/StyledWindow.WPF;component/Themes/Generic.xaml" />
                
                <!-- Other ...-->
                
            </ResourceDictionary.MergedDictionaries>
            
            <Style x:Key="LocalizedStyledWindow"  TargetType="{x:Type Window}" BasedOn="{StaticResource StyledWindow}">
                <Setter Property="themes:StyledWindow.LanguageContent">
                    <Setter.Value>
                        <themes:KnownLanguages>
                            <components:LanguageInfo CultureName="ru-RU" ShortName="Ru"/>
                            <components:LanguageInfo CultureName="en-US" ShortName="En"/>
                        </themes:KnownLanguages>
                    </Setter.Value>
                </Setter>
            </Style>
            
        </ResourceDictionary>

    </Application.Resources>
</Application>

```
#### 2. Настраиваем [App.xaml.cs](https://github.com/Platonenkov/StyledWindow.WPF/blob/master/TestApp.WPF/App.xaml.cs)

#### 3. Применяем стиль к окну

```xaml
        Style="{StaticResource LocalizedStyledWindow}"
```

### Настройка отображения кнопок
```xaml
<Window ...
        xmlns:themes="clr-namespace:StyledWindow.WPF.Themes;assembly=StyledWindow.WPF"
        Style="{StaticResource LocalizedStyledWindow}"
        
        themes:StyledWindow.LanguageButtonVisible="True"
        themes:StyledWindow.ThemeButtonVisible="True"
        themes:StyledWindow.TopmostButtonVisible="True">
...
```

### Список языков в кнопке смены языка
```xaml
<Window ...
        >

    <themes:StyledWindow.LanguageContent>
        <themes:KnownLanguages>
            <components:LanguageInfo CultureName="ru-RU" ShortName="Ru"/>
            <components:LanguageInfo CultureName="en-US" ShortName="En"/>
        </themes:KnownLanguages>
    </themes:StyledWindow.LanguageContent>
...
```

![Styled](https://github.com/Platonenkov/StyledWindow.WPF/blob/master/Resources/LanguageContent.png)

```C#

//ThemeEx.ChangeCulture += OnCultureChange Action<string>;
ThemeEx.ChangeCulture += LocalizationManager.ChangeCulture;
```
#### Подробнее про работу с локализацией и настройку [Localization.WPF](https://github.com/Platonenkov/Localization.WPF)

### Любой контент который нужно пометить в шапке
```xaml
<Window ...
        >

    <themes:StyledWindow.HeaderContent>
        <TextBlock Text="Any content instead of TextBox" VerticalAlignment="Center" />
    </themes:StyledWindow.HeaderContent>
...
```

### Для тестирования есть [тестовый проект](https://github.com/Platonenkov/StyledWindow.WPF)

### Подробнее про использование [темы MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)

![Styled](https://github.com/Platonenkov/StyledWindow.WPF/blob/master/Resources/ThemeContent.png)


### known issues:
`UseLayoutRounding="True"` removes the border at the window...
