using System;

namespace Targem.Calcs
{
    static class Program
    {
        static void Main(string[] args)
        {
            var input = string.Join("", args);
            try
            {
                var result = StringCalculator.Parse(input);
                Console.WriteLine(result);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"ERROR: {exception.Message}");
            }
        }
    }
}