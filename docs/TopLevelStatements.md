# Top level statements

Top level statements reduce the amount of boiler code necessary for certain projects such as connsole applications.

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

> Default console app startup file for C# 10

```C#
Console.WriteLine("Hello, World!");
```

