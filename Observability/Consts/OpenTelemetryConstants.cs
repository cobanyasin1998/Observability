using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observability.ConsoleApp.Consts
{
    internal class OpenTelemetryConstants
    {
        internal const string ServiceName = "ConsoleApp"; //CompanyName.AppName.ConsoleApp
        internal const string ServiceVersion = "1.0.0";
        internal const string ActivitySourceName = "ActivitySource.ConsoleApp";
    }
}
