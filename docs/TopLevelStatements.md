# Top level statements

Top level statements reduce the amount of boiler code necessary for certain projects such as console applications.
Although these were introduced with C# 9, they've been simplified in C# 10, the top level usings are no longer necessary, and are now used in the default Visual Studio project templates.

> Default console app startup file prior to C# 10

```C#
using System;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```

> Alternative console app startup file for C# 9

```C#
using System;

Console.WriteLine("Hello World!");

```
> Default console app startup file for C# 10

```C#
Console.WriteLine("Hello, World!");
```

A single line can now replace 12 lines of code :). It is almost as short as in Python:

```Python
print("Hello, World!")
```

## Minimal Web Api
Top level statements also allow for [minimal ASP.NET Web API](https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio) projects, 5 lines are enough (or 4 without https):

``` C#
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/hey", () => "Hello World!").WithName("Hello World");

app.Run();

```

You can play with [this simple hello world api project](https://github.com/buggy-line/dotnetsix/blob/main/SimpleApi/Program.cs) that also has swagger integration, or create a new one in under a minute.

Note that minimal web APIs have important limitations, please read the [documentation from Microsoft](https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio#differences-between-minimal-apis-and-apis-with-controllers).


## Limitations

As in C# 9, only one file with top level statements can be present in a project, otherwise the following error is thrown: 

> Error	CS8802	Only one compilation unit can have top-level statements.

If both top level statements and a `Main` method are added to a project, no error is thrown but a warning will be raised:

> warning CS7022: The entry point of the program is global code; ignoring 'Program.Main(string[])' entry point.


## OK, but where are my arguments?

You don't declare an `args` variable. For the single source file that contains your top-level statements, the compiler recognizes `args` to mean the command-line arguments. The type of `args` is a `string[]`, as in all C# programs.

Try executing the [ConsoleApp](https://github.com/buggy-line/dotnetsix/blob/main/ConsoleApp/Program.cs) in the current repository to understand how to use the new top level statements.



## What about namespaces?

You can read [here](GlobalAndImplicitUsings.md) on the use of implicit global usings/namespaces.



## Community Response

Although top level statements are easy to use for simple console apps, in more complex scenarios they can prove problematic, especially when coupled with global implicit usings. 

The framework maintainers requested feedback on the new project templates using top-level statements, and considering the (negative) feedback they received, improvements could be introduced in the future https://github.com/dotnet/docs/issues/27420. The discussion is quite lively as of Feb 2022.