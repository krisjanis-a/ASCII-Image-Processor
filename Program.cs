using System;
using System.Linq;
using System.IO;
using System.Drawing;
using ImageProcessor;
using Microsoft.Win32.SafeHandles;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace ASCIIImageProcessor
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            EntryPoint:

            Bitmap originalImage = new Bitmap(@"/Users/Kr1sh/Desktop/unknown/BMW 316i in the wild big.jpeg"); // Insert chosen image file path

            decimal originalWidth = 4608m; // Insert original image width manually
            decimal originalHeight = 3456m; // Insert original image height manually

            decimal aspectRatio = originalWidth / originalHeight;

            decimal newWidth = 600;
            decimal newHeight = (decimal)(newWidth / aspectRatio);

            int newWidthInt = (int)Math.Round(newWidth);
            int newHeightInt = (int)Math.Round(newWidth / aspectRatio);

            Size size = new Size(newWidthInt, newHeightInt);

            Bitmap img = (System.Drawing.Bitmap)ResizeImage(originalImage, size);

            int imageWidth = img.Width;
            int imageHeight = img.Height;

            Console.WriteLine("Image successfully loaded." + Environment.NewLine + "Image size" + imageWidth + "x" + imageHeight);

            var invertColors = false;

            Console.WriteLine("Turn on color inversion? [Y/N]");

            Console.WriteLine("Yes - press [y]");
            Console.WriteLine("No - press [n]");

            while (true)
            {
                ConsoleKeyInfo result = Console.ReadKey();
                Console.Clear();

                // If yes, change variable userChoiceSaveEvent to true and exit loop.
                if ((result.KeyChar == 'Y') || (result.KeyChar == 'y'))
                {
                    invertColors = true;
                    break;
                }

                // If no, change variable userChoiceSaveEvent to false and exit while loop.
                if ((result.KeyChar == 'N') || (result.KeyChar == 'n'))
                {
                    invertColors = false;
                    break;
                }

                // If any other key is pressed, do no print it on console for visually aesthetic purposes.
                {
                    Console.Clear();
                    Console.WriteLine("Turn on color inversion? [Y/N]");
                    Console.WriteLine("Yes - press [y]");
                    Console.WriteLine("No - press [n]");
                }
            }

            Tuple<int, int, int>[,] rgbPixelArray = new Tuple<int, int, int>[img.Height, img.Width];

            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    Color currentPixel = img.GetPixel(j, i);

                    int colorRed = currentPixel.R;
                    int colorGreen = currentPixel.G;
                    int colorBlue = currentPixel.B;

                    rgbPixelArray[i, j] = new Tuple<int, int, int>(colorRed, colorBlue, colorGreen);
                }
            }

            img.Dispose();

            int[,] pixelIntensityMatrix = new int[imageHeight, imageWidth];

            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                     //decimal pixelIntensity = (((rgbPixelArray[i, j].Item1 + rgbPixelArray[i, j].Item2 + rgbPixelArray[i, j].Item3) / 3));

                    // Different calculation formula:

                    decimal redComponent = (decimal)(0.21 * rgbPixelArray[i, j].Item1);
                    decimal greenComponent = (decimal)(0.72 * rgbPixelArray[i, j].Item2);
                    decimal blueComponent = (decimal)(0.07 * rgbPixelArray[i, j].Item3);

                    decimal pixelIntensity = redComponent + greenComponent + blueComponent;

                    if(invertColors == true)
                    {
                        pixelIntensity = 255 - pixelIntensity;
                    }

                    pixelIntensityMatrix[i, j] = (int)Math.Round(pixelIntensity);
                }
            }

            // Original character string: "`^\",:;Il!i~+_-?][}{1)(|\\/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$"

            //string brightnessCharacters = ("`^\",:;Il!i~+_-?][}{1)(|\\/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$");

            //string brightnessCharacters = ("`^\",:;Il!i~+_-?][1)(|\\/tfjrxn8%B@$");

            string brightnessCharacters = ("    ,';-");

            //string brightnessCharacters = ("    .;");

            decimal characterCount = brightnessCharacters.Length;

            decimal brightnessStep = Math.Round(256m / characterCount);

            List<decimal> intervalList = new List<decimal>();

            for (int i = 0; i <= characterCount;)
            {
                decimal currentInterval = i * brightnessStep;
                intervalList.Add(currentInterval);
                i++;
            }

            string[,] pixelMatrix = new string[imageHeight, imageWidth];

            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    for (int k = 0; k < characterCount;)
                    {
                        if ((intervalList[k] <= pixelIntensityMatrix[i, j]) & (pixelIntensityMatrix[i, j] < intervalList[k + 1]))
                        {
                            string charToString = Char.ToString(brightnessCharacters[k]);
                            pixelMatrix[i, j] = (charToString /*  + charToString + charToString */); // Change pixel width
                        }

                        k++;
                    }
                }
            }


            // Print the image pixel Matrix

            for (int i = 0; i < imageHeight; i++)
            {
                string currentLine = string.Empty;

                Console.ForegroundColor = ConsoleColor.Green;
                //Console.BackgroundColor = ConsoleColor.Black;

                for (int j = 0; j < imageWidth; j++)
                {
                    currentLine += pixelMatrix[i, j];
                }

                Console.WriteLine(currentLine);
            }

            Console.ResetColor();

            var restart = false;

            Console.WriteLine("Restart program? [Y/N]");

            Console.WriteLine("Yes - press [y]");
            Console.WriteLine("No - press [n]");

            while (true)
            {
                ConsoleKeyInfo result = Console.ReadKey();
                Console.Clear();

                // If yes, change variable userChoiceSaveEvent to true and exit loop.
                if ((result.KeyChar == 'Y') || (result.KeyChar == 'y'))
                {
                    restart = true;
                    break;
                }

                // If no, change variable userChoiceSaveEvent to false and exit while loop.
                if ((result.KeyChar == 'N') || (result.KeyChar == 'n'))
                {
                    restart = false;
                    break;
                }

                // If any other key is pressed, do no print it on console for visually aesthetic purposes.
                {
                    Console.Clear();
                    Console.WriteLine("Turn on color inversion? [Y/N]");
                    Console.WriteLine("Yes - press [y]");
                    Console.WriteLine("No - press [n]");
                }
            }

            if (restart == true)
            {
                goto EntryPoint;
            }
        }



        public static Image ResizeImage(Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;

            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);

            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);

            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(b);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }
    }
}