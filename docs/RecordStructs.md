# Record structs
## Recap from C# 9

Records were introduced in C# 9. You could define reference based records (like a class), and they were immutable by default when using positional parameters.

> Positional `record` syntax.
```C#
//immutable
public record Person(string FirstName, string LastName);
```

> Explicit immutable `record`.
```C#
//immutable, notice the init only setters
public record Person
{
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
};
```
> Mutable `record`.
```C#
public record Person
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
};
```

<br/>

## Records in C# 10
You can now define a value type record with the following syntax.

> Value type positional record syntax, notice the extra `struct` keyword. Mutable by default.
```C#
//mutable by default !!!
public record struct Person(string FirstName, string LastName);
```

> Unlike a default immutable `record class`, you need to add `readonly` to make a `record struct` immutable.
```C#
//immutable thanks to readonly
public readonly record struct Person(string FirstName, string LastName);
```

> Equivalent to this immutable `record`
> 
```C#
//immutable, notice the init only setters 
public record struct Person
{
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
};
```

> For consistency, the optional **class** keyword can be used to declare reference based records.
```C#
public record class Person(string FirstName, string LastName);
```

A positional `record` or `class record` and a positional `readonly record struct` declare init-only properties. A positional record struct declares read-write properties. You can override either of those defaults, as shown in the previous above.

I'm not sure why `records` are immutable by default while `readonly` has to be added to a `record struct` to make it immutable. I assume the language designers preferred [`tuples`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples) and `record structs` to be kept consistent instead.

## More to follow
I'll expand this section in the near future, but for now you can read the official docs and play with some of the test scenarios available here https://github.com/buggy-line/dotnetsix/tree/main/LanguageFeatures/Records/ValueRecords. 