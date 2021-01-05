# Embedding a JavaScript Interpreter in Xamarin With Jint
Adding JavaScript support to your Xamarin.Forms app.

## Introduction

Over the years I've built Xamarin apps, I've seen multiple reach a point where they need to update business logic without republishing. This concept is loosely known as "code push", the ability to change an apps behaviour without the hassle of republishing it through the stores.

Some of the use cases for code push could be:

 * Update business logic that changes frequently without redeploying the app.
 * Let customers script and customise parts of the apps behaviour via an in-app API.
 * Implement a calculation engine, such as an Excel formulas engine, to dynamically calculate .

While each of these problems could be solved with some clever software engineering or commercial libraries (and I've seen it done many times!), it would be ideal if we could write this dynamic logic as code that our app could execute.

Unfortunately, as Xamarin/MAUI compiles into IL or machine code (platform dependant) and bundled into a package, we cannot dynamically update the C# after publishing. Fortunately, Xamarin/MAUI is part of the .NET ecosystem and has access to a massive library of libraries.

## Jint - A JavaScript Interpreter For .NET

Jint is an open source Javascript interpreter for .NET that provides full [ECMA 5.1 compliance](https://www.w3schools.com/js/js_es5.asp). It = can run on any .NET platform, including Android and iOS and is available as a NuGet package. Adding Jint means that we now have a JavaScript interpreter within our app!

Jint can operate within a sandbox, or with limited visibility into our apps environment or with full access to the .NET CLR.

We can use Jint to execute single expressions for a result:

**C#**
```
var square = new Jint.Engine()
       .SetValue("x", 3) // define a new variable
       .Execute("x * x") // execute a statement
       .GetCompletionValue() // get the latest statement completion value
       .ToObject() // converts the value to .NET
       ;
```

Or we can execute custom scripts:

**C#**
```
var engine = new Jint.Engine()
    .SetValue("log", new Action<object>(Console.WriteLine))
    ;

engine.Execute(@"
  function hello() {
    log('Hello World');
  };

  hello();
");
```

Jint is a fast, stable, secure and full-feature JavaScript runtime that can be embedded into our Xamarin apps. Using Jint, we are no longer constrained to only execute code compiled into our published app.

## Integrating Jint Into A Xamarin App

Jint can be added to any Xamarin app by installing the [Jint NuGet package](https://www.nuget.org/packages/Jint) into your desired project. As Jint supports `netstandard`, this could be your core assembly or any of your platform specific projects.

To execute a JavaScript code block, we create an instance of the `Engine` object and use `Execute()` to run a given script:

**C#**
```
var engine = new Engine();
engine.Execute(script);
```

To expose an object to our JavaScript during execution, we use the `Engine.SetValue` method. We could expose the current main page of the app instance to the script with the following code:

**C#**
```
engine.SetValue("mainPage", App.Current.MainPage);
```

Scripts can then use all methods and properties on the object:

**JavaScript**
```
mainPage.Title = "Hello From JavaScript!"; // Changes the pages title.
mainPage.DisplayAlert("Hello From JavaScript", "This is an alert called from JavaScript", "Cancel"); // Shows an alert.
```

Using the `SetValue` method, we can also expose global functions for scripts to use:

**C#**
```
engine.SetValue("log", new Action<object>(Console.WriteLine);
engine.SetValue("parseColor", new Func<JsValue, Xamarin.Forms.Color>( (hexValue) => Xamarin.Forms.Color.FromHex(hexValue.AsString()));
```

**JavaScript**
```
log("Hello from JavaScript"); // Calls Console.WriteLine to print "Hello from JavaScript" in the console output.
mainPage.BackgroundColor = parseColor("#228811"); // Converts the given string into a Xamarin.Forms color for assignment to the BackgroundColor.
```

When Jint passes data back to C#, it can automatically convert JavaScript object back to their .NET counter parts. Alternatively, we can manually receive the `JsValue`, inspect it and then handle the conversion ourselves.

The below example uses the `Is` and `As` methods to inspect the type of the JavaScript value and convert it to it's .NET equivalent:

**C#**
```
engine.SetValue("savePreference", new Action<JsValue, JsValue>((JsValue key, JsValue value) =>
{
    if (value.IsString())
    {
        Preferences.Set(key.AsString(), value.AsString());
    }
    else if (value.IsNumber())
    {
        Preferences.Set(key.AsString(), value.AsNumber());
    }
    else if(value.IsBoolean())
    {
        Preferences.Set(key.AsString(), value.AsBoolean());
    }
}));
```

We may want to stop scripts that exceed a certain condition. For example, a script may take too long to execute, use too much memory or end up in a recursion loop. We can do this using **constraints**:

**C#**
```
var jint = new Engine((options) => {
    options.TimeoutInterval(TimeSpan.FromSeconds(2)); // Stop the script if it runs longer than 2 seconds.
    options.LimitMemory(2_000_00); // Limit total memory consumption to 2 MB
    options.LimitRecursion(10); // Limit the amount of times a function can recurse to 10 deep.
    options.CancellationToken(token); // Ends the scripts execution if the token is set to cancelled.
    options.MaxStatements(10000); // Limit the total statements a script may execute to 10,000.
});
```

Constraints will through an exception when

Lastly, we can create our own custom constraints by inheriting from the `IConstraint` interface and registering them with the options:

**C#**
```
public class AppIsInitialisedConstraint : IConstraint
{
    public void Check()
    {
        if (App.Current.MainPage == null)
        {
            throw new InvalidOperationException("No main page is present");
        }
    }

    public void Reset()
    {
        // TODO:
    }
}

options.Constraint(new AppIsInitialisedConstraint());
```

## The Power Of Jint

To show what we can do by adding JavaScript support to an app, I've created some samples that allow JavaScript to interact with an app:

 * The [`SetAppTheme.js`](https://github.com/matthewrdev/xamarin.forms-jint/blob/main/src/XamarinForms.JavaScriptInterpreter/XamarinForms.JavaScriptInterpreter/Resources/SetAppTheme.js) and [`SetAppIcon.js`](https://github.com/matthewrdev/xamarin.forms-jint/blob/main/src/XamarinForms.JavaScriptInterpreter/XamarinForms.JavaScriptInterpreter/Resources/SetAppIcon.js) scripts change the visual state of the app. It demonstrates custom logic, interacting with a controlled API surface, showing dialogs via JavaScript to C# interop and also handing program flow back to JavaScript by executing a script-provided lambda.
 * The [`LoremIpsum.js`](https://github.com/matthewrdev/xamarin.forms-jint/blob/main/src/XamarinForms.JavaScriptInterpreter/XamarinForms.JavaScriptInterpreter/Resources/LoremIpsum.js) script lets a user choose how many sentences to generate, generates it in JavaScript and then shows the result via an alert dialog. It demonstrates a wide range of JavaScript language features, implementing and executing complex logic as well as JavaScript to C# interop.

In these examples, I've exposed a global `context` variable to Jint; this context defines the API surface that a script uses to interact with the app. While possible to expose the whole app domain to Jint, I prefer using a custom API for interopping from JavaScript to C#. In my experience I've found this makes debugging scripts much easier (EG: set and use breakpoints within the API methods) and makes tracking down runtime errors easier also (captured stack traces show the custom API code).

## Summary

By integrating Jint, we can add JavaScript support into our Xamarin.Forms apps. With an embedded JavaScript interpreter, we can write parts of our apps logic in JavaScript and through data and implement a "lite" form of code push.

To see a full working example of Jint in a Xamarin.Forms app, please find the [full source code here](https://github.com/matthewrdev/xamarin.forms-jint).

🤙
Matthew Robbins
