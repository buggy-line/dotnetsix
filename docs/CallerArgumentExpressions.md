# Caller argument expressions
The `CallerArgumentExpressionAttribute` shares information about the context of a method call. Like the other [CompilerServices attributes](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices?view=net-6.0), this attribute is applied to an optional parameter.

```C#
internal static class CallerArgument
{
    public static string CheckGeneratedCallerArgument(decimal value, 
        [CallerArgumentExpression("value")] string? message = null)
    {
        Console.WriteLine($"Value: {message}");
        return message;
    }
}
```

The `CheckGeneratedCallerArgument` has an optional parameter decorated with an attribute: 
``` C#
[CallerArgumentExpression("value")] string? message = null

```
The `CallerArgumentExpression` attribute receives a string parameter that points the name of the first parameter passed into the method: `decimal value`. 
At compile time, the compiler will replace the optional argument with a `string` representation of the expression passed in to the `value` parameter. Take a look at the below examples:


```C#
var integerValue = CallerArgument.CheckGeneratedCallerArgument(42);
Assert.Equal("42", integerValue); 

var mathExpression = CallerArgument.CheckGeneratedCallerArgument(21 + 21);
Assert.Equal("21 + 21", mathExpression); 

int? x = null;
var nullCoalescing = CallerArgument.CheckGeneratedCallerArgument(x ?? 42);
Assert.Equal("x ?? 42", nullCoalescing);

var sum = (decimal a, decimal b) => a + b;
var lambdaInvocation = CallerArgument.CheckGeneratedCallerArgument(su(21, 21)); 
Assert.Equal("sum(21, 21)", lambdaInvocation);
```

The `ArgumentNullException.ThrowIfNull` method is one of the new additions to the standard library that makes use of the new attribute.
```C#
public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
```