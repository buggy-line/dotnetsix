# New LINQ Features

Although not an language feature, LINQ is part of the core .,NET libraries, and in .NET 6 it has some interesting new additions.

## Index `^` and Range `..` operators
Although introduced in C# 8, the  [Index `^` and Range `..` operators](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/ranges-indexes) can now be used with LINQ as well via methods like `Take`.

``` C#
var items = new List<string>
{
    "zero",
    "one",
    "two",
    "three",
    "four"
};

var last = items.ElementAt(^1);
var secondToLast = items.ElementAt(^2);

Assert.Equal(items.Last(), last); 
Assert.Equal(items[items.Count - 2], secondToLast);

var lastTwo = items.Take(^2..);
Assert.Equal(new[] {"three", "four"}, lastTwo);

var take3AndSkip2 = items.Take(^3..^2); // take last3 and then skip the last two
Assert.Equal(new[] { "two" }, take3AndSkip2);
```


## Chunk
`Chunk` allows one to split an enumerable into batches or chunks of a fixed length. The last chunk will contain the remaining elements and may be of a smaller size.

``` C#
var items = new List<int> { 0, 1, 2, 3, 4 }; // 5 items
var chunks = items.Chunk(3);

// {
//     new [] { 0, 1, 2 },
//     new [] { 3, 4}
// }

```

## MaxBy and MinBy

``` C#
internal readonly record struct Population(int Year, int Count);

internal List<Population> EstimatedPopulation = new List<Population>
{
    new Population(2022, 19_031_335),
    new Population(2021, 19_127_774),
    //...
    new Population(1951, 16_512_664),
    new Population(1950, 16_236_296)
};
```
Before .NET 6, if you wanted to find the maximum or minimum item in a collection, you'd write something like below:

``` C#
var minPopulation = EstimatedPopulation.OrderBy(x => x.Count).First();

var maxPopulation = EstimatedPopulation.OrderBy(x => x.Count).Last(); 
```

.NET 6 simplifies this a bit

``` C#
var minPopulation = EstimatedPopulation.MinBy(x => x.Count);

var maxPopulation = EstimatedPopulation.MaxBy(x => x.Count);
```

Note that `MinBy` is not the same as the `Min` overload that takes a selector function:


``` C#
//return type is Population
Population minPopulation = EstimatedPopulation.MinBy(x => x.Count);

// return type is int, the type of the Population.Count field.
int minCount = EstimatedPopulation.Min(x => x.Count); 
int alsoMinCount = EstimatedPopulation.Select(x => x.Count).Min(); // longer version

```

## DistinctBy
`DistinctBy` is  similar to MaxBy/MinBy, it allows one to pass in a predicate used to find distinct items. In past framework versions one had to use the `GroupBy` statement to achive similar functionality - unless the objects in the collection implemented `IEquatable` or an `IEqualityComparer`


``` C#
var shapes = new List<(string Name, stringColor)>
{
    new ("Square", "Red"),
    new ("Circle", "Red"),
    new ("Triangle", "Green"),
    new ("Square", "Blue"),
    new ("Circle", "Yellow"),
};
```

Distinct by Shape.Name:
``` C#
var distictShapesByName = shapes.DistinctB(s => s.Name);

// {
//     new ("Square", "Red"),
//     new ("Circle", "Red"),
//     new ("Triangle", "Green"),
// }
```

Distinct by Shape.Color:
``` C#
var distinctShapesByColors = shapesDistinctBy(s => s.Color);

// {
//     new ("Square", "Red"),
//     new ("Triangle", "Green"),
//     new ("Square", "Blue"),
//     new ("Circle", "Yellow")
// }
```
## IntersectBy
`IntersectBy` takes two sets and finds the common values between them and ignores the rest:

``` C#

distinctShapesByName = new List<(string Name, string Color)>
{
    new ("Square", "Red"),
    new ("Circle", "Red"),
    new ("Triangle", "Green"),
};

distinctShapesByColors = new List<(string Name, string Color)>
 {
     new ("Square", "Red"),
     new ("Triangle", "Green"),
     new ("Square", "Blue"),
     new ("Circle", "Yellow"),
 };

var intersectionByColor = distinctShapesByName.IntersectBy(distinctShapesByColors.Select(s => s.Color), s => s.Color);

// {
//     new ("Square", "Red"),
//     new ("Triangle", "Green"),
// }

```
## ExceptBy
`ExceptBy` is the opposite of `IntersectBy`, it finds items that dont appear in both sources.

``` C#
distinctShapesByName = new List<(string Name, string Color)>
{
    new ("Square", "Red"),
    new ("Circle", "Red"),
    new ("Triangle", "Green"),
};

distinctShapesByColors = new List<(string Name, string Color)>
{
    new ("Square", "Red"),
    new ("Triangle", "Green"),
    new ("Square", "Blue"),
    new ("Circle", "Yellow"),
};

var exceptByColor = distinctShapesByColors.ExceptBy(distinctShapesByName.Select(s => s.Color), s => s.Color);

// {
//     new ("Square", "Blue"),
//     new ("Circle", "Yellow")
// }

```
## UnionBy
`UnionBy` iterarates over all the elements in the first and second collection, yielding only once each unique element - according to the predicate.

``` c#
var squares = new List<(string Name, stringColor)>
{
    new ("Square", "Red"),
    new ("Square", "Red"),
    new ("Square", "Green"),
    new ("Sqhare", "Blue"),
};

var circles = new List<(string Name, stringColor)>
{
    new ("Circle", "Red"),
    new ("Circle", "Green"),
    new ("Circle", "Yellow"),
};

var shapeUnion = squares.UnionBy(circles, s =>s.Color);

// {
//     new ("Square", "Red"),
//     new ("Square", "Green"),
//     new ("Sqhare", "Blue"),
//     new ("Circle", "Yellow"),
// }
```