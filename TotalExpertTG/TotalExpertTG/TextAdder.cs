using System.Drawing;

using SkiaSharp;

namespace TotalExpertTG;

public class TextAdder
{
    

    public MemoryStream GenerateMeme(string text)
    {
        var firstText = "Hello";
        var secondText = "World";

        var imageFilePath = @"resources\picture.bmp";
        var path = Path.Combine(Directory.GetCurrentDirectory(), @"resources\FortniteExperts.png");
        var stream = new MemoryStream();
       
        using var bitmap = SKBitmap.Decode(path);
        var canvas = new SKCanvas(bitmap);
        var origin = new SKPoint();
        var paint = new SKPaint
        {
            TextSize = 20,
            IsAntialias = true,
            Color = SKColors.White,
            IsStroke = false,
        };

        var lines = WrapLines(text, 600, 150, paint);
        origin.X = 17;
        origin.Y = 400;
        paint.TextAlign = SKTextAlign.Left;
        foreach (var wrappedLine in lines)
        {
            canvas.DrawText(wrappedLine, origin, paint);
            origin.Y += paint.FontSpacing;
        }
        
        canvas.Flush();
        var resultImage = SKImage.FromBitmap(bitmap);
        var data = resultImage.Encode(SKEncodedImageFormat.Png, 100);
        data.SaveTo(stream);
        return stream;
    }
    
    private List<string> WrapLines(string longLine, float lineLengthLimit, float heightLimit, SKPaint defPaint)
    {
        var wrappedLines = new List<string>();
        var lineLength = 0f;
        var line = "";
        var lineHeight = defPaint.FontSpacing + defPaint.TextSize;
        var height = lineHeight;
        
        foreach (var word in longLine.Split(' '))
        {
            var wordWithSpace = word + " ";
            var wordWithSpaceLength = defPaint.MeasureText(wordWithSpace);
            if (lineLength + wordWithSpaceLength > lineLengthLimit)
            {
                if (height > heightLimit - lineHeight)
                {
                    line += "...";
                    wrappedLines.Add(line);
                    return wrappedLines;
                }
                wrappedLines.Add(line);
                height += lineHeight;
                line = "" + wordWithSpace;
                lineLength = wordWithSpaceLength;
            }
            else
            {
                line += wordWithSpace;
                lineLength += wordWithSpaceLength;
            }
        }
        
        wrappedLines.Add(line);
        return wrappedLines;
    }
}
