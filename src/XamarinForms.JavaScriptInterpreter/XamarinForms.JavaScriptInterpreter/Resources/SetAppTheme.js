var currentTheme = context.GetAppTheme();

var newTheme = "Dark";

if (currentTheme == 'Dark') {
    newTheme = "Light";
}

var setThemeFunc = function (result) {
    if (result === true) {
        context.SetAppTheme(newTheme);
    }
}

context.Confirm('Would you like to change the app theme to ' + newTheme + '?', 'Change Theme?', setThemeFunc);