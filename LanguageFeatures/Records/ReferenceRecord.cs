namespace ReferenceRecords;

internal record class User
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

        Assert.Equal("Gandalf", user1.Name);
        Assert.Equal(user1, user2);
        Assert.Equal(user1.GetHashCode(), user2.GetHashCode());

        static User ChangeName(User user, string name)
        {
            user.Name = name;
            return user;
        }
    }
}
