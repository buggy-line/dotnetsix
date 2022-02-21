namespace ImmutableValueRecords;

internal readonly record struct User(string? Name, DateTime Birthdate)
{
    public List<string>? Stuff { get; init; } = new List<string>();

    public User() : this(string.Empty, default)
    {

    }

    public User(string? Name, DateTime Birthdate, List<string>? stuff) : this(Name, Birthdate)
    {
        Stuff = stuff;
    }

    //init parameters and readonly parameters from the primary constructor cannot be mutated.
    //public void Mutate()
    //{
    //    Name = "ChangedName";
    //    Stuff = new List<string>();
    //}
}


public class Tests
{
    [Fact]
    public void WhenTestingForEquality()
    {
        var user1 = new User("Daniel", new DateTime(1990, 3, 21));
        var user2 = new User("Daniel", new DateTime(1990, 3, 21));

        Assert.NotEqual(user1, user2);
        Assert.NotEqual(user1.GetHashCode(), user2.GetHashCode());
    }

    [Fact]
    public void WhenTestingOtherEqualsScenarios()
    {
        //when passing null lists
        var user1 = new User("Daniel", new DateTime(1990, 3, 21), null);
        var user2 = new User("Daniel", new DateTime(1990, 3, 21), null);
        Assert.Equal(user1, user2);
        Assert.Equal(user1.GetHashCode(), user2.GetHashCode());


        //when passing new lists
        user1 = new User("Daniel", new DateTime(1990, 3, 21), new List<string>());
        user2 = new User("Daniel", new DateTime(1990, 3, 21), new List<string>());
        Assert.NotEqual(user1, user2);
        Assert.NotEqual(user1.GetHashCode(), user2.GetHashCode());


        //when passing same list instance
        var stuff = new List<string>();
        user1 = new User("Daniel", new DateTime(1990, 3, 21), stuff);
        user2 = new User("Daniel", new DateTime(1990, 3, 21), stuff);
        Assert.Equal(user1, user2);
        Assert.Equal(user1.GetHashCode(), user2.GetHashCode());

        user1.Stuff?.Add("something");
        Assert.Equal(user1, user2);
        Assert.Equal(user1.GetHashCode(), user2.GetHashCode());


        //when cloning a record via 'with'
        user1 = new User("Daniel", new DateTime(1990, 3, 21), new List<string>());
        user2 = user1 with { };
        Assert.Equal(user1, user2);
        Assert.Equal(user1.GetHashCode(), user2.GetHashCode());

        user1.Stuff?.Add("something");
        Assert.Equal(user1, user2);
        Assert.Equal(user1.GetHashCode(), user2.GetHashCode());
    }

    [Fact]
    public void WhenTestingClonesForEquality()
    {
        var user1 = new User("Daniel", new DateTime(1990, 3, 21));
        var user2 = user1 with { };

        //Value based equality is used
        Assert.Equal(user1, user2);
        Assert.Equal(user1.GetHashCode(), user2.GetHashCode());
    }

    [Fact(Skip = "Readonly (immutable) record struct cannot be modified")]
    public void WhenModifyingAReadonlyRecord()
    {
        //var user1 = new User("Daniel", new DateTime(1990, 3, 21));
        //user1.Name = "Alex";

        //var user2 = new User("Alex", new DateTime(1990, 3, 21));

        //Assert.Equal("Alex", user1.Name);
        //Assert.Equal(user1, user2);
    }

    [Fact]
    public void WhenCloningARecord()
    {
        var user1 = new User("Daniel", new DateTime(1990, 3, 21));

        var user2 = user1 with { }; // shallow clone!!!

        Assert.Equal(user1.Name, user2.Name);
        Assert.Equal(user1.Birthdate, user2.Birthdate);

        Assert.Equal(user1, user2); // value based equality still works
    }

    [Fact]
    public void WhenCopyingARecord()
    {
        var user1 = new User("Daniel", new DateTime(1990, 3, 21));

        var user2 = user1 with { Name = "Alex" };

        Assert.Equal("Alex", user2.Name);
        Assert.Equal(user1.Birthdate, user2.Birthdate); // birthdate was copied from user1
        Assert.NotEqual(user1.Name, user2.Name); // only the name was changed

        Assert.NotEqual(user1, user2); // the records are no longer equal
    }

    [Fact]
    public void WhenAddingSimilarRecordsToHashSet()
    {
        var user1 = new User("Daniel", new DateTime(1990, 3, 21));
        var user2 = new User("Daniel", new DateTime(1990, 3, 21));

        var hashSet = new HashSet<User>();
        var user1Added = hashSet.Add(user1);
        var user2Added = hashSet.Add(user2);

        Assert.Contains(user1, hashSet);
        Assert.Contains(user2, hashSet);

        Assert.Equal(2, hashSet.Count);

        Assert.True(user1Added);
        Assert.True(user2Added);
    }

    [Fact]
    public void WhenAddingIdenticalRecordsToHashSet()
    {
        var stuff = new List<string>();
        var user1 = new User("Daniel", new DateTime(1990, 3, 21), stuff);
        var user2 = new User("Daniel", new DateTime(1990, 3, 21), stuff);
        //same would happen if stuff were null, or user2 = user1 with {} was used.

        var hashSet = new HashSet<User>();
        var user1Added = hashSet.Add(user1);
        var user2Added = hashSet.Add(user2);

        Assert.Contains(user1, hashSet);
        Assert.Contains(user2, hashSet);

        Assert.Single(hashSet);

        Assert.True(user1Added);
        Assert.False(user2Added);
    }
}
