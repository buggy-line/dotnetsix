namespace StructVsRecordStruct;

internal record struct User(string Name, DateTime Birthdate);

//the above record declaration will generate code similar to the below classical struct declaration

internal struct HandCraftedUserRecord
{
    public string Name { get; set; }

    public DateTime Birthdate { get; set; }

    public HandCraftedUserRecord(string name, DateTime birthdate)
    {
        Name =name;
        Birthdate = birthdate;
    }

    public override bool Equals(object? obj) => obj is HandCraftedUserRecord temp && Equals(temp);

    public bool Equals(HandCraftedUserRecord other)
    {
        return
            EqualityComparer<string>.Default.Equals(Name, other.Name) &&
            EqualityComparer<DateTime>.Default.Equals(Birthdate, other.Birthdate);
    }

    public static bool operator ==(HandCraftedUserRecord user1, HandCraftedUserRecord user2)
        => user1.Equals(user2);

    public static bool operator !=(HandCraftedUserRecord user1, HandCraftedUserRecord user2)
        => !(user1 == user2);

    public override int GetHashCode()
    {
        return HashCode.Combine(
            EqualityComparer<string>.Default.GetHashCode(Name),
            EqualityComparer<DateTime>.Default.GetHashCode(Birthdate));
    }
}

public class Tests
{
    [Fact]
    public void WhenTestingForEquality()
    {
        var user1 = new HandCraftedUserRecord("Daniel", new DateTime(1990, 3, 21));
        var user2 = new HandCraftedUserRecord("Daniel", new DateTime(1990, 3, 21));

        //Value based equality is used
        Assert.Equal(user1, user2);
        Assert.Equal(user1.GetHashCode(), user2.GetHashCode());
    }

    [Fact]
    public void WhenModifyingARecordLikeStruct()
    {
        var user1 = new HandCraftedUserRecord("Daniel", new DateTime(1990, 3, 21));

        var user2 = new HandCraftedUserRecord("Alex", new DateTime(1990, 3, 21));
        user1.Name = "Alex";

        Assert.Equal("Alex", user1.Name);
        Assert.Equal(user1, user2);
    }

    [Fact]
    public void WhenCloningARecordLikeStruct()
    {
        var user1 = new HandCraftedUserRecord("Daniel", new DateTime(1990, 3, 21));

        var user2 = user1 with { }; // shallow clone!!!

        Assert.Equal(user1.Name, user2.Name);
        Assert.Equal(user1.Birthdate, user2.Birthdate);

        Assert.Equal(user1, user2); // value based equality still works
    }

    [Fact]
    public void WhenCopyingARecordLikeStruct()
    {
        var user1 = new HandCraftedUserRecord("Daniel", new DateTime(1990, 3, 21));

        var user2 = user1 with { Name = "Alex" };

        Assert.Equal("Alex", user2.Name);
        Assert.Equal(user1.Birthdate, user2.Birthdate); // birthdate was copied from user1
        Assert.NotEqual(user1.Name, user2.Name); // only the name was changed

        Assert.NotEqual(user1, user2); // the records are no longer equal
    }

    [Fact]
    public void WhenAddingARecordLikeStructToHashSet()
    {
        var user1 = new HandCraftedUserRecord("Daniel", new DateTime(1990, 3, 21));
        var user2 = new HandCraftedUserRecord("Daniel", new DateTime(1990, 3, 21));

        var hashSet = new HashSet<HandCraftedUserRecord>();
        var user1Added = hashSet.Add(user1);
        var user2Added = hashSet.Add(user2);

        Assert.Contains(user1, hashSet);
        Assert.Contains(user2, hashSet);

        Assert.Single(hashSet);

        Assert.True(user1Added);
        Assert.False(user2Added);
    }
}
