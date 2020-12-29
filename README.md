# Executing JavaScript in Xamarin Using Jint
Adding JavaScript support to your Xamarin.Forms app.

## Introduction
Over the years I've been building mobile apps using Xamarin, I've notice tmany apps reach a point where common trend has emerged. Eventually, when business and their apps reach a certain size and success, there is often a requirement to perform user-defined calculations or the need to dynamically update the apps business logic without redeploying the app (code push).

Some of the uses for an executing JavaScript in your app could be:

 * Let customers use JavaScript to can customise parts of the apps behaviour via an in-app API.
 * Code push: You could move some business logic that changes frequently into JavaScript and update it without redeploying the app.
 * Implement an embedded calculation engine, similar to [SyncFusions Excel formulas calculation engine](https://www.syncfusion.com/xamarin-ui-controls/xamarin-calculation-engine), using JavaScript.

## Introducing Jint

This sample shows how to add JavaScript scripting into a Xamarin.Forms app [using Jint](https://github.com/sebastienros/jint).

Jint is a Javascript interpreter for .NET which provides full [ECMA 5.1 compliance](https://www.w3schools.com/js/js_es5.asp) and can run on any .NET platform, including Android and iOS.

Jint can operate within a sandbox, or with limited visibility into our apps environment or with full access to the .NET CLR!

We can use Jint to execute single expressions for a result:

```
var square = new Jint.Engine()
       .SetValue("x", 3) // define a new variable
       .Execute("x * x") // execute a statement
       .GetCompletionValue() // get the latest statement completion value
       .ToObject() // converts the value to .NET
       ;
```

Or we can execute custom scripts:

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

Jint is a fast, stable, secure and full-feature JavaScript runtime that can be embedded into our Xamarin apps. Using Jint we are no longer constrained to only execute code we have compiled into our app bundles.

Let's look at how we can

## Integrating Jint Into A Xamarin App

Jint can be added to any Xamarin app by installing the [Jint NuGet package](https://www.nuget.org/packages/Jint). This can be installed in only the assembly where Jint is used (as Jint supports netstandard), in our code Xamarin.Forms assembly or in 

### Executing JavaScript Using Jint



### C# Interoperability

 * Exposing an API to a script.
 * Executing C# functions.
 * Converting data from JavaScript to .NET.
 * Transferring program flow back to JavaScript.


## The Power Of Jint

To demonstrate the usefulness of integrating Jint into our app, I've built 3 samples that showcase various ways we can using
