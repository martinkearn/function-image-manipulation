
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Net.Http;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageManipulation
{
    public static class Crop
    {
        [FunctionName("Crop")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            var x = 0;
            var y = 0;
            var width = 50;
            var height = 50;


            Image sourceImage = null;
            Image destinationImage = null;

            // get byte array from body
            byte[] bytes;
            using (var streamReader = new MemoryStream())
            {
                req.Body.CopyTo(streamReader);
                bytes = streamReader.ToArray();

                using (Bitmap temp = new Bitmap(streamReader))
                {
                    sourceImage = new Bitmap(temp);

                    destinationImage = new Bitmap(width, height);
                    Graphics g = Graphics.FromImage(destinationImage);

                    g.DrawImage(
                      sourceImage,
                      new Rectangle(x, y, width, height),
                      new Rectangle(x, y, width, height),
                      GraphicsUnit.Pixel
                    );
                }
            }

            byte[] outputBytes = null;
            using (var ms = new MemoryStream())
            {
                destinationImage.Save(ms, destinationImage.RawFormat);
                outputBytes = ms.ToArray();
            }


            return (ActionResult)new OkObjectResult(outputBytes);
        }


    }
}
