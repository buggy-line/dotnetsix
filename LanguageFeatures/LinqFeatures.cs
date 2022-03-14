namespace LinqFeatures;

public class LinqFeatures
{
    internal readonly record struct Population(int Year, int Count);

    internal List<Population> EstimatedPopulation = new List<Population>
    {
        new Population(2022, 19_031_335),
        new Population(2021, 19_127_774),
        new Population(2020, 19_237_691),
        new Population(2019, 19_364_557),
        new Population(2018, 19_506_114),
        new Population(2017, 19_653_969),
        new Population(2016, 19_796_285),
        new Population(2015, 19_925_175),
        new Population(2014, 20_035_930),
        new Population(2013, 20_132_776),
        new Population(2012, 20_227_469),
        new Population(2011, 20_336_718),
        new Population(2010, 20_471_864),
        new Population(2009, 20_637_991),
        new Population(2008, 20_829_517),
        new Population(2007, 21_034_189),
        new Population(2006, 21_234_305),
        new Population(2005, 21_417_291),
        new Population(2004, 21_577_885),
        new Population(2003, 21_720_407),
        new Population(2002, 21_853_273),
        new Population(2001, 21_989_350),
        new Population(2000, 22_137_419),
        new Population(1999, 22_298_125),
        new Population(1998, 22_466_286),
        new Population(1997, 22_637_604),
        new Population(1996, 22_805_703),
        new Population(1995, 22_964_754),
        new Population(1994, 23_115_811),
        new Population(1993, 23_256_956),
        new Population(1992, 23_375_822),
        new Population(1991, 23_456_644),
        new Population(1990, 23_489_160),
        new Population(1989, 23_466_407),
        new Population(1988, 23_393_730),
        new Population(1987, 23_288_400),
        new Population(1986, 23_175_058),
        new Population(1985, 23_071_274),
        new Population(1984, 22_983_968),
        new Population(1983, 22_907_302),
        new Population(1982, 22_830_554),
        new Population(1981, 22_737_209),
        new Population(1980, 22_615_639),
        new Population(1979, 22_463_308),
        new Population(1978, 22_285_507),
        new Population(1977, 22_087_896),
        new Population(1976, 21_879_086),
        new Population(1975, 21_665_643),
        new Population(1974, 21_450_410),
        new Population(1973, 21_232_621),
        new Population(1972, 21_011_137),
        new Population(1971, 20_783_546),
        new Population(1970, 20_548_911),
        new Population(1969, 20_305_354),
        new Population(1968, 20_055_971),
        new Population(1967, 19_810_615),
        new Population(1966, 19_582_335),
        new Population(1965, 19_379_568),
        new Population(1964, 19_207_135),
        new Population(1963, 19_059_938),
        new Population(1962, 18_924_157),
        new Population(1961, 18_780_202),
        new Population(1960, 18_613_939),
        new Population(1959, 18_420_454),
        new Population(1958, 18_202_712),
        new Population(1957, 17_968_466),
        new Population(1956, 17_726_631),
        new Population(1955, 17_483_935),
        new Population(1954, 17_243_818),
        new Population(1953, 17_005_717),
        new Population(1952, 16_764_904),
        new Population(1951, 16_512_664),
        new Population(1950, 16_236_296)
    };

    [Fact]
    public void WhenSearchingMinimum()
    {
        var minCount = EstimatedPopulation.Min(x => x.Count);

        var minPopulation = EstimatedPopulation.MinBy(x => x.Count);
        var alsoMinPopulation = EstimatedPopulation.OrderBy(x => x.Count).First(); // prior to .NET 6

        Assert.Equal(minCount, minPopulation.Count);
        Assert.Equal(minCount, alsoMinPopulation.Count);

        Assert.IsType<Population>(minPopulation);
        Assert.IsType<int>(minCount);
    }

    [Fact]
    public void WhenSearchingMaximum()
    {
        var maxCount = EstimatedPopulation.Max(x => x.Count);

        var maxPopulation = EstimatedPopulation.MaxBy(x => x.Count);
        var alsoMaxPopulation = EstimatedPopulation.OrderBy(x => x.Count).Last(); // prior to .NET 6

        Assert.Equal(maxCount, maxPopulation.Count);
        Assert.Equal(maxCount, alsoMaxPopulation.Count);

        Assert.IsType<Population>(maxPopulation);
        Assert.IsType<int>(maxCount);
    }

    [Fact]
    public void WhenUsingNewSetOperations()
    {
        var shapes = new List<(string Name, string Color)>
        {
            new ("Square", "Red"),
            new ("Circle", "Red"),
            new ("Triangle", "Green"),
            new ("Square", "Blue"),
            new ("Circle", "Yellow")
        };

        var distinctShapesByName = shapes.DistinctBy(s => s.Name);
        var distinctShapesByColors = shapes.DistinctBy(s => s.Color);

        Assert.Equal(distinctShapesByName,
            new List<(string Name, string Color)>
            {
                new ("Square", "Red"),
                new ("Circle", "Red"),
                new ("Triangle", "Green"),
            });

        Assert.Equal(distinctShapesByColors,
            new List<(string Name, string Color)>
            {
                new ("Square", "Red"),
                new ("Triangle", "Green"),
                new ("Square", "Blue"),
                new ("Circle", "Yellow")
            });


        var intersectionByColor = distinctShapesByName.IntersectBy(distinctShapesByColors.Select(s => s.Color), s => s.Color);
        Assert.Equal(intersectionByColor,
            new List<(string Name, string Color)>
            {
                new ("Square", "Red"),
                new ("Triangle", "Green"),
            });


        var exceptByColor = distinctShapesByColors.ExceptBy(distinctShapesByName.Select(s => s.Color), s => s.Color);
        Assert.Equal(exceptByColor,
            new List<(string Name, string Color)>
            {
                new ("Square", "Blue"),
                new ("Circle", "Yellow")
            });

        var squares = new List<(string Name, string Color)>
        {
            new ("Square", "Red"),
            new ("Square", "Red"),
            new ("Square", "Green"),
            new ("Sqhare", "Blue"),

        };

        var circles = new List<(string Name, string Color)>
        {
            new ("Circle", "Red"),
            new ("Circle", "Green"),
            new ("Circle", "Yellow"),
        };

        var shapeUnion = squares.UnionBy(circles, s => s.Color);

        Assert.Equal(shapeUnion,
            new List<(string Name, string Color)>
            {
                new ("Square", "Red"),
                new ("Square", "Green"),
                new ("Sqhare", "Blue"),
                new ("Circle", "Yellow"),
            });
    }

    [Fact]
    public void WhenUsingChunk()
    {
        var items = new List<int> { 0, 1, 2, 3, 4 }; // 5 items
        var chunks = items.Chunk(3);

        Assert.Equal(chunks, new List<int[]>
        {
            new [] { 0, 1, 2 },
            new [] { 3, 4}
        });
    }

    [Fact]
    public void WhenUsingRangeAndIndex()
    {
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

        var take3AndSkip2 = items.Take(^3..^2); // take last 3 and then skip the last two
        Assert.Equal(new[] { "two" }, take3AndSkip2);
    }
}


