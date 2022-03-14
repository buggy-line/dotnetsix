using System.Runtime.CompilerServices;

namespace CallerArgument;

internal static class CallerArgument
{
    public static string? CheckGeneratedCallerArgument(decimal value, 
        [CallerArgumentExpression("value")] string? message = null)
    {
        Console.WriteLine($"Value: {message}");
        return message;
    }
}

internal class SimpleMath
{
    public decimal Sum(decimal a, decimal b)
    {
        return a + b;
    }
}

public class Tests
{
    [Fact]
    public void WhenCallingAMethodThatHasACallerArgumentExpression()
    {
        var integerValue = CallerArgument.CheckGeneratedCallerArgument(42);
        Assert.Equal("42", integerValue);

        var mathExpression = CallerArgument.CheckGeneratedCallerArgument(21 + 21);
        Assert.Equal("21 + 21", mathExpression);

        int? x = null;
        var nullCoalescing = CallerArgument.CheckGeneratedCallerArgument(x ?? 42);
        Assert.Equal("x ?? 42", nullCoalescing);

        var sum = (decimal a, decimal b) => a + b;
        var lambdaInvocation = CallerArgument.CheckGeneratedCallerArgument(sum(21, 21)); 
        Assert.Equal("sum(21, 21)", lambdaInvocation);

        var methodInvocation = CallerArgument.CheckGeneratedCallerArgument(new SimpleMath().Sum(21, 21));
        Assert.Equal("new SimpleMath().Sum(21, 21)", methodInvocation);
    }
}
