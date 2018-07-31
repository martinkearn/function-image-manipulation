# Function Image Manipulation
A collection of C# Azure functions which do things with images.

## Crop
Takes an image (binary file) which has been POST'd to the function, crops it and returns the cropped image as a binary file (byte array represented as a Base64 string).

This function uses the [CoreCompat.System.Drawing](https://github.com/CoreCompat/CoreCompat) Nuget package which ports the old .net 4.6 `System.Drawing` namespace to .net core. There are various options for image manipulation in .net core, this article details some of them: https://blogs.msdn.microsoft.com/dotnet/2017/01/19/net-core-image-processing/

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
