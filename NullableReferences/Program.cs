using NullableReferences;
{ 
    //warning: Converting null literal or possible null value to non-nullable type.
    string message = null;

    // warning: Dereference of a possible null reference.
    Console.WriteLine($"The length of the message is {message.Length}"); //exception: System.NullReferenceException at runtime

    var originalMessage = message;
    message = "Hello, World!";

    // No warning. Analysis determined "message" is not null.
    Console.WriteLine($"The length of the message is {message.Length}");

    //warning: Dereference of a possible null reference.
    Console.WriteLine(originalMessage.Length);
}


{

    string message = null;
    if (string.IsNullOrWhiteSpace(message))
    {
        Console.WriteLine($"The length of the message is {message.Length}");  //warning: Dereference of a possible null reference.
    }
}


{
    string message = null;
    if (!string.IsNullOrWhiteSpace(message))
    {
        Console.WriteLine($"The length of the message is {message.Length}");  //no warning:
    }
}


{
    var referenceWrapper = new Wrapper<string>("foobar");
    string? nullString = null;
    referenceWrapper.Value = nullString;  //warning: possible null reference assignment

    var valueWrapper = new Wrapper<int>(101);
    int? nullInt = null;
    //valueWrapper.Value = nullInt!;  //error: cannot convert int? to int
}
