namespace Observability.ConsoleApp
{
    internal class ServiceHelper
    {
        internal void Work1()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity();

            var serviceOne = new ServiceOne();


            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine(i);
            }

            serviceOne.MakeRequestToGoogle().Wait();

        }
    }
}
