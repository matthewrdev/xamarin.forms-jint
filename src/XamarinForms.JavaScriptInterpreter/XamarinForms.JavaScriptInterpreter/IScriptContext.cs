using System;
using Jint.Native;

namespace XamarinForms.JavaScriptInterpreter
{
    public interface IScriptContext
    {
        void SetAppIcon(JsValue icon);

        void SetAppTheme(JsValue theme);

        string GetAppTheme();

        void Confirm(JsValue title, JsValue message, JsValue callback);

        void Prompt(JsValue title, JsValue message, JsValue placeholder, JsValue callback);

        void ActionSheet(JsValue title, JsValue options, JsValue callback);

        void Alert(JsValue title, JsValue message);

        void Toast(JsValue message);
    }
}
