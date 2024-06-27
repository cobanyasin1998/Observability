using System.Diagnostics;

namespace Observability.ConsoleApp
{
    internal class ServiceHelper
    {
        internal void Work1()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity(
                kind: System.Diagnostics.ActivityKind.Server,
                name: $"{nameof(ServiceHelper)} - {nameof(Work1)}");

            var serviceOne = new ServiceOne();


            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine(i);
            }

            serviceOne.MakeRequestToGoogle().Wait();

        }

        internal void Work2()
        {
            using var activity = ActivitySourceProvider.SourceFile.StartActivity(
                kind: System.Diagnostics.ActivityKind.Server,
                name: $"{nameof(ServiceHelper)} - {nameof(Work1)}");

            activity.SetTag("file", "ServiceHelper.cs");
            activity.AddEvent(new ActivityEvent("Event: Reading file", tags: new ActivityTagsCollection() { { "file", "ServiceHelper.cs" } }));
          

        }
    }
}
