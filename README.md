# StyledWindow.WPF

Библиотека с настроенным стилем окна для Windows WPF приложения.

### Что есть на титульном баре

* Стандартные кнопки: свернуть, на весь экран, закрыть
* Кнопка TopMost - сделать по верх всех окон
* Кнопка смены языка локализации приложения
* Кнопка настройки темы (светлая/тёмная темы и цвета шрифтов и панелей)
* Любой контент какой хотите отразить в Title
* Стантартное место под иконку

### Как настроить

#### 1. Настраиваем App.xaml

```xaml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
            
                <materialDesign:BundledTheme BaseTheme="Inherit" PrimaryColor="Amber" SecondaryColor="LightBlue"
                                             ColorAdjustment="{materialDesign:ColorAdjustment}" />
                                             
                <ResourceDictionary Source="pack://application:,,,/StyledWindow.WPF;component/Themes/StyledWindow.xaml" />

            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>

    </Application.Resources>

```
#### 2. Настраиваем [App.xaml.cs](https://github.com/Platonenkov/StyledWindow.WPF/blob/master/TestApp.WPF/App.xaml.cs)

#### 3. Применяем стиль к окну

```xaml
        Style="{StaticResource StyledWindow}"
```

### Настройка отображения кнопок
```xaml
<Window ...
        xmlns:themes="clr-namespace:StyledWindow.WPF.Themes;assembly=StyledWindow.WPF"
        Style="{StaticResource StyledWindow}"
        
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