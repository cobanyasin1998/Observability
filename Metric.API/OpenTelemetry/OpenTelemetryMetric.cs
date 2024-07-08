using System.Diagnostics.Metrics;

namespace Metric.API.OpenTelemetry
{
    public static class OpenTelemetryMetric
    {
        public static readonly Meter meter = new Meter("Metric.API", "1.0.0");
       
        public static Counter<int> OrderCreatedEventCounter = meter.CreateCounter<int>("order.created.event.count");


        public static ObservableCounter<int> OrderCancelledCounter = meter.CreateObservableCounter("order.cancelled.count", () => new Measurement<int>(Counter.OrderCancelledCounter));


        public static UpDownCounter<int> CurrentStockCounter = meter.CreateUpDownCounter<int>("current.stock.count");

        public static ObservableUpDownCounter<int> CurrentStockObservableCounter = meter.CreateObservableUpDownCounter("current.stock.observable.counter", () => new Measurement<int>(Counter.CurrentStockCount));


        public static ObservableGauge<int> rowKitchenTemp = meter.CreateObservableGauge<int>("room.kitchen.temp",
                () => new Measurement<int>(Counter.KitchenTemp));


        public static Histogram<int> XMethodDuration = meter.CreateHistogram<int>("x.method.duration", unit: "milliseconds");
    }
    public class Counter
    {
        public static int OrderCancelledCounter { get; set; }

        public static int CurrentStockCount { get; set; } = 1000;

        public static int KitchenTemp { get; set; } = 0;
    }
}
