using System;
using System.Collections.Generic;
using System.Windows.Input;
using Acr.UserDialogs;
using Jint;
using Jint.Native;
using PropertyChanged;
using Xamarin.Forms;
using XamarinForms.JavaScriptInterpreter.Helpers;

namespace XamarinForms.JavaScriptInterpreter
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel : IScriptContext
    {
        public MainViewModel()
        {
            RunScriptCommand = new Command(() =>
            {
                if (SelectedScript is null)
                {
                    return;
                }

                using (Profiler.Profile(SelectedScript.Name))
                {
                    var jint = new Engine();

                    jint.SetValue("context", this);

                    jint.Execute(ScriptContent);
                }
            });

            ScriptContent = SelectedScript.Content;
        }

        public string Title { get; set; } = "Embedded JavaScript Intepreter";

        public string Icon { get; set; } = "xds";

        public string ScriptContent { get; set; }

        public int SelectedScriptIndex { get; set; } = 0;

        public List<Script> Scripts { get; set; } = new List<Script>()
        {
            new Script()
            {
                Name = "SetAppTheme.js",
                Content = ResourcesHelper.ReadResourceContent("SetAppTheme.js"),
            },
            new Script()
            {
                Name = "SetAppIcon.js",
                Content = ResourcesHelper.ReadResourceContent("SetAppIcon.js"),
            },
            new Script()
            {
                Name = "LoremIpsum.js",
                Content = ResourcesHelper.ReadResourceContent("LoremIpsum.js"),
            },
        };

        [DependsOn(nameof(Scripts), nameof(SelectedScriptIndex))]
        public Script SelectedScript => Scripts != null ? Scripts[SelectedScriptIndex] : null;

        public ICommand RunScriptCommand { get; } 

        public void ActionSheet(JsValue title, JsValue options, JsValue callback)
        {
            var config = new ActionSheetConfig();
            config.Title = title.AsString();

            List<ActionSheetOption> choices = new List<ActionSheetOption>();

            foreach (var option in options.AsArray())
            {
                var choice = new ActionSheetOption(option.AsString(), () => callback.Invoke(option.AsString()));
                choices.Add(choice);
            }

            config.Options = choices;

            Acr.UserDialogs.UserDialogs.Instance.ActionSheet(config);
        }

        public void Alert(JsValue title, JsValue message)
        {
            var config = new AlertConfig();
            config.Title = title.AsString();
            config.Message = message.AsString();

            UserDialogs.Instance.Alert (config);
        }

        public void Confirm(JsValue title, JsValue message, JsValue callback)
        {
            var config = new ConfirmConfig();
            config.Title = title.AsString();
            config.Message = message.AsString();
            config.OnAction = (result) => callback.Invoke(result);

            Acr.UserDialogs.UserDialogs.Instance.Confirm(config);
        }

        public string GetAppTheme()
        {
            return App.Instance.themeName;
        }

        public void Prompt(JsValue title, JsValue message, JsValue placeholder, JsValue callback)
        {
            var config = new PromptConfig();
            config.Title = title.AsString();
            config.Message = message.AsString();
            config.Text = placeholder.AsString();
            config.OnAction = (result) =>
            {
                if (result.Ok)
                {
                    callback.Invoke(result.Text);
                }
            };

            UserDialogs.Instance.Prompt(config);
        }

        public void SetAppTheme(JsValue theme)
        {
            App.Instance.LoadTheme(theme.ToString());
        }

        public void SetAppIcon(JsValue icon)
        {
            Icon = icon.ToString();
        }

        public void Toast(JsValue message)
        {
            UserDialogs.Instance.Toast(message.AsString());
        }
    }
}
