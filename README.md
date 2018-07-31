# Function Image Manipulation
A collection of C# Azure functions which do things with images.

## Crop
Takes an image (binary file) which has been POST'd to the function, crops it and returns the cropped image as a binary file (byte array represented as a Base64 string).

This function uses the [CoreCompat.System.Drawing](https://github.com/CoreCompat/CoreCompat) Nuget package which ports the old .net 4.6 `System.Drawing` namespace to .net core. This does however require that the Function runs as a Windows function in Azure, Linux functions will not be able to use this library.

There are various options for image manipulation in .net core, this article details some of them: https://blogs.msdn.microsoft.com/dotnet/2017/01/19/net-core-image-processing/. 

In developing this sample I spent a decent amount of time exploring [ImageSharp](https://sixlabors.github.io/docs/articles/ImageSharp/GettingStarted.html) primarily via [Andrew Lock's 'Using ImageSharp ...' series of articles](https://andrewlock.net/using-imagesharp-to-resize-images-in-asp-net-core-a-comparison-with-corecompat-system-drawing/) but at the time of writing (July 2018) I could not get it to work with a v2 C# Function. However, ImageSharp does look like a potentially good, cross-platform library when it gets to a stable public release.

Use https://codebeautify.org/base64-to-image-converter to test the Base64 string to see what it actually looks like.

To trigger the function do a HTTP request with Postman or similar as follows:
* POST
* Body = binary file representing the source image
* Parameters = `?y=10&x=15&height=200&width=400` 
  * `x`: The X plane. The number of pixel from the left edge of the source image that the left edge of the cropped image will start
  * `y`: The Y plane. The number of pixel from the top edge of the source image that the top edge of the cropped image will start
  * `width`: The number of pixel wide the cropped image will be
  * `height`: The number of pixel high the cropped image will be
  * The example above will generate a 200 x 400 cropped images where the top left corner starts 10 pixels from the left edge of the source image and 15 pixel from the top edge of the source image

![Request details for Crop function in Postman](https://github.com/martinkearn/function-image-manipulation/raw/master/Post%20Crop.PNG)

You can test out the live version of this function using https://imagemanipulationfunctions.azurewebsites.net/api/
