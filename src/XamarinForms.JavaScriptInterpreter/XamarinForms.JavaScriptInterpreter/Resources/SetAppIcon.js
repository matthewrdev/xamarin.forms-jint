var icons = ["FifteenPercentDrop", "monkeyfest", "xds", "Custom"];

var setAppIconCallback = function (choice) {
    if (choice == "Custom") {
        context.Prompt("Enter An Icon URL", "Choose Icon", "https://raw.githubusercontent.com/mfractor/mfractor.branding/master/PNG/icon.png", setAppIconCallback);
    }
    else {
        context.SetAppIcon(choice);
        context.Toast("App icon changed to " + choice);
    }

}

context.ActionSheet("Choose An Icon", icons, setAppIconCallback);