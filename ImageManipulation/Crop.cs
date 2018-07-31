
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Drawing;
using System.IO;

namespace ImageManipulation
{
    public static class Crop
    {
        [FunctionName("Crop")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("Crop function processed a request.");

            // Get params from request. Default to 10x10px square from top left corner. If the source images is smaller than 10x10, this will throw an exception; sorry about that but this is just a sample ;)
            var parameters = req.GetQueryParameterDictionary();
            var y = (parameters.ContainsKey("y")) ? Convert.ToInt16(parameters["y"]) : 0;
            var x = (parameters.ContainsKey("x")) ? Convert.ToInt16(parameters["x"]) : 0;
            var width = (parameters.ContainsKey("width")) ? Convert.ToInt16(parameters["width"]) : 10;
            var height = (parameters.ContainsKey("height")) ? Convert.ToInt16(parameters["height"]) : 10;

            // Resize image and save to destination Image
            Image sourceImage;
            Image destinationImage;
            using (var ms = new MemoryStream())
            {
                // Copy body of request to the memory stream
                req.Body.CopyTo(ms);

                // Copy the contents of memory stream to the source image file
                sourceImage = new Bitmap(ms);

                // Create a new destination image of the required size
                destinationImage = new Bitmap(width, height);

                // Draw the destination image from the source based on bounding box 
                var boundingBox = new Rectangle(x, y, width, height);
                Graphics g = Graphics.FromImage(destinationImage);
                g.DrawImage(sourceImage, boundingBox, boundingBox,GraphicsUnit.Pixel);
            }

            // Write destination image to byte array
            byte[] outputBytes = null;
            using (var ms = new MemoryStream())
            {
                // Save image to memory stream
                destinationImage.Save(ms, destinationImage.RawFormat);

                // Save memory stream to byte array
                outputBytes = ms.ToArray();
            }

            // NOTE: There is a good tool for testing the output byte array https://codebeautify.org/base64-to-image-converter

            // Return a byte array wrapped in an OK result
            return new OkObjectResult(outputBytes);
        }


    }
}
