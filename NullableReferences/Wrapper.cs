namespace NullableReferences;

public class Wrapper<T> where T: notnull
{
    public Wrapper(T value)
    {
        Value = value;
    }

    public T Value { get; set; }
}
