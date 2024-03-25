using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.UseCases.models
{
    public class ServiceResults
    {
        public object ResultObject { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}
