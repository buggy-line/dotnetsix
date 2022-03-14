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

## Differences between class records and struct records

### Semantics

A `record class` or `record` has reference semantics, while a `record struct` has value semantics. This matters most when you use mutable records.

Passing a record struct to a method creates a copy of that record. Changing a property on the copy **does not** impact the initial record.

> Value semantics
```C#
internal record struct User(string Name, DateTime Birthdate); //positional syntax creates a mutable `record struct`

[Fact]
public void WhenModifyingARecordOutsideCurrentScope()
{
    var user1 = new User("Daniel", new DateTime(1990, 3, 21));
    var user2 = ChangeName(user1, "Gandalf");

    Assert.Equal("Daniel", user1.Name); // user1.Name is still 'Daniel', passing a value type to the ChangeName method creates a new value type scoped to that method.
    Assert.NotEqual(user1, user2);
    Assert.NotEqual(user1.GetHashCode(), user2.GetHashCode());

    static User ChangeName(User user, string name)
    {
        user.Name = name;
        return user;
    }
}
```

Passing a record class to a method creates a copy of that record's reference. Changing a property on the reference **does** impact the initial record.
> Reference semantics
```C#
internal record class User // we need explicit syntax to create a mutable `record class`
{
    public User(string name, DateTime birthdate){ Name = name; Birthdate = birthdate;}
    public string? Name { get; set; } = default;
    public DateTime Birthdate { get; set; } = default;
}

public class Tests
{
    [Fact]
    public void WhenModifyingARecordOutsideCurrentScope()
    {
        var user1 = new User("Daniel", new DateTime(1990, 3, 21));
        var user2 = ChangeName(user1, "Gandalf");

        Assert.Equal("Gandalf", user1.Name); // user1.Name is now Gandalf.
        Assert.Equal(user1, user2);
        Assert.Equal(user1.GetHashCode(), user2.GetHashCode());

        static User ChangeName(User user, string name)
        {
            user.Name = name;
            return user;
        }
    }
}

```

If you want changes to a record struct's properties reflected at the call site, use `ref` or `out` keywords to pass by reference.
> Value semantics with pass by reference `ref`
```C#
internal record struct User(string Name, DateTime Birthdate); //positional syntax creates a mutable `record struct`

[Fact]
public void WhenModifyingARecordOutsideCurrentScopePassByRef()
{
    var user1 = new User("Daniel", new DateTime(1990, 3, 21));
    var user2 = ChangeName(ref user1, "Gandalf");

    Assert.Equal("Gandalf", user1.Name); // user1.Name is now Gandalf, the record struct was passed by reference
    Assert.Equal(user1, user2);
    Assert.Equal(user1.GetHashCode(), user2.GetHashCode());

    static User ChangeName(ref User user, string name)
    {
        user.Name = name;
        return user;
    }
}

```

## Combining value and reference semantics
Consider the following readonly record declaration:

``` C#
readonly record struct User(string? Name, DateTime Birthdate, List<string> Stuff)
```

At first sight it may look like an immutable record. The `User.Name` and `User.Birthdate` fields are generated by the compiler thanks to the positional parameters, and they are immutable due to the `readonly` keyword.

The `User.Stuff` is an optional init only field initialized by the explicit constructor and defaulted an empty list. While the `User.Stuff` field is readonly, the contents of the list are not.

To complicate the scenario, consider what happens when trying to add such a record to a HashSet or using it as a dictionary key.

``` C#
readonly record struct User(string? Name, DateTime Birthdate, List<string> Stuff);

var user1 = new User("Daniel", new DateTime(1990, 3, 21), new List<string> {"foo", "bar"});
var user2 = new User("Daniel", new DateTime(1990, 3, 21), new List<string> {"foo", "bar"});

var hashSet = new HashSet<User>();
var user1Added = hashSet.Add(user1); // added to hash set
var user2Added = hashSet.Add(user2); // added to hash set, the user1.Stuff and user2.Stuff are not the same object even if the contents are identical
```

With a User record missing `User.Stuff` field, adding the user2 to the hashSet behaves differently.

``` C#
record struct User(string? Name, DateTime Birthdate);

var user1 = new User("Daniel", new DateTime(1990, 3, 21), new List<string> {"foo", "bar"});
var user2 = new User("Daniel", new DateTime(1990, 3, 21), new List<string> {"foo", "bar"});

var hashSet = new HashSet<User>();
var user1Added = hashSet.Add(user1); // added to hash set
var user2Added = hashSet.Add(user2); // not added, user1 and user2 are identical
```

Identical to what happens when passing a null list:

``` C#
readonly record struct User(string? Name, DateTime Birthdate, List<string> Stuff);

var user1 = new User("Daniel", new DateTime(1990, 3, 21), null);
var user2 = new User("Daniel", new DateTime(1990, 3, 21), null);

var hashSet = new HashSet<User>();
var user1Added = hashSet.Add(user1); // added to hash set
var user2Added = hashSet.Add(user2); // not added, user1 and user2 are identical
```

Or the same list.

``` C#
readonly record struct User(string? Name, DateTime Birthdate, List<string> Stuff);

var stuff = new List<string> {"foo", "bar"};
var user1 = new User("Daniel", new DateTime(1990, 3, 21), stuff);
var user2 = new User("Daniel", new DateTime(1990, 3, 21), stuff);

var hashSet = new HashSet<User>();
var user1Added = hashSet.Add(user1); // added to hash set
var user2Added = hashSet.Add(user2); // not added, user1 and user2 are identical
```

Because of the scenarios listed above, one should be very careful when using `records` that mix fields with value and reference semantics. 