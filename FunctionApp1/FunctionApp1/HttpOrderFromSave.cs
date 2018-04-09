
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionApp1
{
    public static class HttpOrderFromSave
    {
        [FunctionName("HttpOrderFromSave")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = null)]HttpRequest req,
            [Table("Orders", Connection = "StorageConnection")]ICollector<PhotoOrder> ordersTable,TraceWriter log)
        {
            PhotoOrder orderData = null;

            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEnd();
                orderData = JsonConvert.DeserializeObject<PhotoOrder>(requestBody);
                orderData.PartitionKey = System.DateTime.UtcNow.DayOfYear.ToString();
                orderData.RowKey = orderData.FileName;
                ordersTable.Add(orderData);
            }
            catch(Exception e)
            {
                return new BadRequestObjectResult("Invalid data");
            }

            return (ActionResult)new OkObjectResult("Order processed");
        }
    }

    public class PhotoOrder : TableEntity
    {
        public string CustomerEmail { get; set; }
        public string FileName { get; set; }
        public int RequiredHeight { get; set; }
        public int RequiredWidth { get; set; }
    }
}
