# Executing JavaScript in Xamarin.Forms Using Jint
Adding JavaScript support to your Xamarin.Forms app.

## Introduction
This sample shows how to add JavaScript scripting into a Xamarin.Forms app using Jint.

Jint is a Javascript interpreter for .NET which provides full ECMA 5.1 compliance and can run on any .NET platform. We can integrate Jint into any Xamarin app to enable

Jint can operate within a sandbox, or with limited visibility into our apps environment or we can expose everything to it.

Some of the uses for an embedded JavaScript runtime can be:

 * Let customers use JavaScript to can customise a sub-set of the apps behaviour. We could expose an API This is useful if you're providing a white-label solution.
 * Code push: You could move some business logic that changes frequently into JavaScript and update it without redeploying the app.
 * Replace an embedded calculation engine, like SyncFusions calc engine or a custom built one, with JavaScript.
