using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF.Jint.MarkupExtensions
{
    [ContentProperty(nameof(Path))]
    public class JsBindExtension : IMarkupExtension
    {
        public string Path { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            // TODO: Get JsBindingContext, lookup

            return null;
        }
    }
}
