using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Media.Imaging;

namespace FormattersAndHandlers
{
    public class TextFormatter
    {
        public static TextBlock ConvertToTextBlock(string input)
        {
            var textBlock = new TextBlock();

            // Use regular expressions to find and process HTML-like tags
            var matches = Regex.Matches(input, @"<b>(.*?)</b>|<i>(.*?)</i>|<u>(.*?)</u>|([^<]*)");

            foreach (Match match in matches)
            {
                if (match.Groups[1].Success)
                {
                    // Process bold text
                    var bold = new Bold(new Run(match.Groups[1].Value));
                    textBlock.Inlines.Add(bold);
                }
                else if (match.Groups[2].Success)
                {
                    // Process italic text
                    var italic = new Italic(new Run(match.Groups[2].Value));
                    textBlock.Inlines.Add(italic);
                }
                else if (match.Groups[3].Success)
                {
                    // Process underlined text
                    var underline = new Underline(new Run(match.Groups[3].Value));
                    textBlock.Inlines.Add(underline);
                }
                else if (match.Groups[4].Success)
                {
                    // Process plain text
                    textBlock.Inlines.Add(new Run(match.Groups[4].Value));
                }
            }

            return textBlock;
        }
    }

    public class ImageHandler
    {
        public static BitmapImage ConvertBitSequenceToImage(byte[] bitSequence)
        {
            try
            {
                if (bitSequence == null || bitSequence.Length == 0)
                {
                    throw new ArgumentException("Bit sequence is empty or null.");
                }

                using (MemoryStream stream = new MemoryStream(bitSequence))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();

                    return image;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public static byte[] LoadImageFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return File.ReadAllBytes(filePath);
                }
                else
                {
                    throw new FileNotFoundException("Image file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
    }
}