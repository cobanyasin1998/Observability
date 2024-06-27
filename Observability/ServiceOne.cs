using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Observability.ConsoleApp
{
    internal class ServiceOne
    {
        static HttpClient httpClient = new HttpClient();

        public async Task<int> MakeRequestToGoogle()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity(
                kind: System.Diagnostics.ActivityKind.Client,
                name: $"{nameof(ServiceOne)} - {nameof(MakeRequestToGoogle)}");
            try
            {
                var eventsTags = new ActivityTagsCollection();

                eventsTags.Add("event", "Making request to google");
                eventsTags.Add("http.method", "GET");
                eventsTags.Add("http.url", "https://www.google.com");

                activity?.AddEvent(new ActivityEvent("Event: Making request to google", tags: eventsTags));
                var result = await httpClient.GetAsync("https://www.google.com");
                var responseContent = await result.Content.ReadAsStringAsync();

                ServiceTwo serviceTwo = new ServiceTwo();
                serviceTwo.Work2();

                return responseContent.Length;
            }
            catch (Exception ex)
            {
                activity?.SetStatus(Status.Error.WithDescription(ex.Message));
                throw;
            }

        }
    }
}
