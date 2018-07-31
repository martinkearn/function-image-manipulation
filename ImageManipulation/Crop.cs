
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace ImageManipulation
{
    public static class Crop
    {
        [FunctionName("Crop")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            // get byte array from body
            byte[] bytes;
            using (var streamReader = new MemoryStream())
            {
                req.Body.CopyTo(streamReader);
                bytes = streamReader.ToArray();
            }

            //Crop image



            return (ActionResult)new OkObjectResult($"OK");
        }
    }
}
