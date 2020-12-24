using System;

namespace XamarinForms.JavaScriptInterpreter
{
    public class Script
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
