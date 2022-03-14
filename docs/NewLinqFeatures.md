# New LINQ Features

Although not an language feature, LINQ is part of the core .,NET libraries, and in .NET 6 it has some interesting new additions.

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

Note than `MinBy` is not the same as the `Min` overload that takes a selector function:


``` C#
//return type is Population
Population minPopulation = EstimatedPopulation.MinBy(x => x.Count);

// return type is int, the type of the Population.Count field.
int minCount = EstimatedPopulation.Min(x => x.Count); 
int alsoMinCount = EstimatedPopulation.Select(x => x.Count).Min(); // longer version

```


## DistinctBy, IntersectBy, ExceptBy and UnionBy  

### `DistinctBy` is  similar to MaxBy/MinBy, it allows one to pass in a predicate used to find distinct items. In past framework versions one had to use the `GroupBy` statement to achive similar functionality - unless the objects in the collection implemented `IEquatable` or an `IEqualityComparer`


``` C#
var shapes = new List<(string Name, stringColor)>
{
    new ("Square", "Red"),
    new ("Circle", "Red"),
    new ("Triangle", "Green"),
    new ("Square", "Blue"),
    new ("Circle", "Yellow")
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

### `IntersectBy` takes two sets and finds the common values between them and ignores the rest:

``` C#
var intersectionByColor = distinctShapesByName.IntersectBy(distinctShapesByColors.Select(s => s.Color), s => s.Color);

// {
//     new ("Square", "Red"),
//     new ("Triangle", "Green"),
// }

```

### `ExceptBy` is the opposite of `IntersectBy`, it finds items that dont appear in both sources.

``` C#
var exceptByColor = distinctShapesByColors.ExceptBy(distinctShapesByName.Select(s => s.Color), s => s.Color);

// {
//     new ("Square", "Blue"),
//     new ("Circle", "Yellow")
// }

```

`UnionBy` is the last one in the series, it iterarates over all the elements in the first and second collection, yielding only once each unique element - according to the predicate.

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