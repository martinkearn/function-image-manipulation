
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

            var x = 100;
            var y = 100;
            var width = 400;
            var height = 400;

            Image sourceImage = null;
            Image destinationImage = null;

            // resize image
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

            // write to output byte array
            byte[] outputBytes = null;
            using (var ms = new MemoryStream())
            {
                destinationImage.Save(ms, destinationImage.RawFormat);
                outputBytes = ms.ToArray();
            }

            // good tool for testing/visulaising the output byte array https://codebeautify.org/base64-to-image-converter


            return (ActionResult)new OkObjectResult(outputBytes);
        }


    }
}
