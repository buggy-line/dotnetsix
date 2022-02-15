namespace ConsoleUtilities;

public static class ConsoleAnimation
{
    public static async Task ShowConsoleAnimation()
    {
        for (int i = 0; i < 20; i++)
        {
            Console.Write("| -");
            await Task.Delay(50);
            Console.Write("\b\b\b");
            Console.Write("/ \\");
            await Task.Delay(50);
            Console.Write("\b\b\b");
            Console.Write("- |");
            await Task.Delay(50);
            Console.Write("\b\b\b");
            Console.Write("\\ /");
            await Task.Delay(50);
            Console.Write("\b\b\b");
        }
        Console.WriteLine();
    }
}
