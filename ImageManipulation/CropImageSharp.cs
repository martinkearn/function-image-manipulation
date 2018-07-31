
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using ImageSharp;
using System.Threading.Tasks;
using System;
using ImageSharp.Formats;
using SixLabors.Primitives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace ImageManipulation
{
    public static class CropImageSharp
    {
        [FunctionName("CropImageSharp")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("Crop Image Sharp function processed a request.");

            // Get params from request. Default to 10x10px square from top left corner. If the source images is smaller than 10x10, this will throw an exception; sorry about that but this is just a sample ;)
            var parameters = req.GetQueryParameterDictionary();
            var y = (parameters.ContainsKey("y")) ? Convert.ToInt16(parameters["y"]) : 0;
            var x = (parameters.ContainsKey("x")) ? Convert.ToInt16(parameters["x"]) : 0;
            var width = (parameters.ContainsKey("width")) ? Convert.ToInt16(parameters["width"]) : 10;
            var height = (parameters.ContainsKey("height")) ? Convert.ToInt16(parameters["height"]) : 10;

            // Crop the image
            Image<Rgba32> destinationImage = null;
            using (var ms = new MemoryStream())
            {
                // Copy body of request to the memory stream
                req.Body.CopyTo(ms);

                Image<Rgba32> sourceImage = Image.Load(ms);
                var boundingBox = new Rectangle(x, y, width, height);
                //The Crop function does not seem to be avaliable in version 1.0.0-beta0004 from NuGet at July 2018 ??
                //destinationImage = sourceImage.Crop(boundingBox);
            }

            // Write destination image to byte array
            byte[] outputBytes = null;
            using (var ms = new MemoryStream())
            {
                // Save image to memory stream
                destinationImage.Save(ms, new JpegEncoder());

                // Save memory stream to byte array
                outputBytes = ms.ToArray();
            }

            // NOTE: There is a good tool for testing the output byte array https://codebeautify.org/base64-to-image-converter

            // Return a byte array wrapped in an OK result
            return new OkObjectResult(outputBytes);
        }
    }
}
