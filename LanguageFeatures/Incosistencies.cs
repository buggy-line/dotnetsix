namespace Incosistencies;

internal record User(string? Name, DateTime Birthdate); // class based record (reference type)

public class Incosistencies
{
    [Fact]
    public void AnonymousTypeEqualityInconsistency()
    {
        var item1 = new { Name = "Simon", Birthdate = new DateTime(1994, 4, 22) };
        var item2 = new { Name = "Simon", Birthdate = new DateTime(1994, 4, 22) };

        Assert.Equal(item1, item2);         // ok
        Assert.True(item1.Equals(item2));   // ok, the Equals overload on anonymous types uses value semantics.

        Assert.False(item1 == item2);       // false, reference equality. The == operator dpes not use value semantics.
    }

    [Fact]
    public void TupleTypeEqualityConsistency()
    {
        (string Name, DateTime Birthdate) item1 = ("Simon", new DateTime(1994, 4, 22));
        (string Name, DateTime Birthdate) item2 = ("Simon", new DateTime(1994, 4, 22));

        Assert.Equal(item1, item2);         // ok
        Assert.True(item1.Equals(item2));   // ok 

        Assert.True(item1 == item2);        // ok, tuples are value types and implement the == operator since C# 7.3 using value semantics
    }

    [Fact]
    public void RecordTypeEqualityConsistency()
    {
        var item1 = new User("Simon", new DateTime(1994, 4, 22));
        var item2 = new User("Simon", new DateTime(1994, 4, 22));

        Assert.Equal(item1, item2);         // ok
        Assert.True(item1.Equals(item2));   // ok 

        Assert.True(item1 == item2);        // ok, records use value based equality when using == operator
    }
}
