namespace Observability.ConsoleApp
{
    internal class ServiceTwo
    {
        internal void Work2()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity(
                               kind: System.Diagnostics.ActivityKind.Server,
                                              name: $"{nameof(ServiceTwo)} - {nameof(Work2)}");

            for (int i = 0; i < 10000; i++)
            {
                for (int z = 0; z < 10; z++)
                {
                    Console.WriteLine($"{i}'nin {z}i");
                }
                Console.WriteLine(i);
            }
        }
    }
}
