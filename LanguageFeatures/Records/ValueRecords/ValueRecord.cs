namespace ValueRecords;

internal record struct User (string Name, DateTime Birthdate);

public class Tests
{
    [Fact]
    public void WhenTestingForEquality()
    {
        var user1 = new User("Daniel", new DateTime(1990, 3, 21));
        var user2 = new User("Daniel", new DateTime(1990, 3, 21));

        //Value based equality is used
        Assert.Equal(user1, user2);
        Assert.Equal(user1.GetHashCode(), user2.GetHashCode());
    }

    [Fact]
    public void WhenModifyingARecord()
    {
        var user1 = new User("Daniel", new DateTime(1990, 3, 21));

        var user2 = new User("Alex", new DateTime(1990, 3, 21));
        user1.Name = "Alex";

        Assert.Equal("Alex", user1.Name);
        Assert.Equal(user1, user2);
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

        var user2 = user1 with {Name = "Alex" };

        Assert.Equal("Alex", user2.Name);
        Assert.Equal(user1.Birthdate, user2.Birthdate); // birthdate was copied from user1
        Assert.NotEqual(user1.Name, user2.Name); // only the name was changed

        Assert.NotEqual(user1, user2); // the records are no longer equal
    }

    [Fact]
    public void WhenAddingRecordsToHashSet()
    {
        var user1 = new User("Daniel", new DateTime(1990, 3, 21));
        var user2 = new User("Daniel", new DateTime(1990, 3, 21));

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
