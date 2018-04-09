
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;

namespace FunctionApp1
{
    public static class HttpOrderFromSave
    {
        [FunctionName("HttpOrderFromSave")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            PhotoOrder orderData = null;

            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEnd();
                orderData = JsonConvert.DeserializeObject<PhotoOrder>(requestBody);
            }
            catch(Exception e)
            {
                return new BadRequestObjectResult("Invalid data");
            }

            return (ActionResult)new OkObjectResult("Order processed");
        }
    }

    public class PhotoOrder
    {
        public string CustomerEmail { get; set; }
        public string FileName { get; set; }
        public int RequiredHeight { get; set; }
        public int RequiredWidth { get; set; }
    }
}
