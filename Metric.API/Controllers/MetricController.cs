using Metric.API.OpenTelemetry;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace Metric.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            OpenTelemetryMetric.OrderCreatedEventCounter.Add(1,
                new KeyValuePair<string, object?>("event","add")
                );
            return Ok("Metric API");
        }

        [HttpGet]
        public IActionResult CounterObservableMetric()
        {
            Counter.OrderCancelledCounter += new Random().Next(1, 100);


            return Ok();
        }


        [HttpGet]
        public IActionResult UpDownCounterMetric()
        {

            OpenTelemetryMetric.CurrentStockCounter.Add(new Random().Next(-300, 300));

            return Ok();

        }

        [HttpGet]
        public IActionResult UpDownCounterObservableMetric()
        {

            Counter.CurrentStockCount += new Random().Next(-300, 300);

            return Ok();

        }



        [HttpGet]
        public IActionResult GaugeObservableMetric()
        {

            Counter.KitchenTemp = new Random().Next(-30, 60);

            return Ok();

        }


        [HttpGet]
        public IActionResult HistogramMetric()
        {


            OpenTelemetryMetric.XMethodDuration.Record(new Random().Next(500, 50000));

            return Ok();

        }

    }
}
