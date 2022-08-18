using System.Drawing;

namespace TotalExpertTG;

public class TextAdder
{
    

    public string GenerateMeme(string text)
    {
        var firstText = "Hello";
        var secondText = "World";

        var firstLocation = new PointF(10f, 10f);
        var secondLocation = new PointF(10f, 50f);

        var imageFilePath = @"resources\picture.bmp";
        var path = Path.Combine(Directory.GetCurrentDirectory(), @"resources\FortniteExperts.png");
        var bmp1 = new Bitmap(path);
    
    
        using(Graphics graphics = Graphics.FromImage(bmp1))
        {
            using (Font arialFont =  new Font("Arial", 10))
            {
                graphics.DrawString(firstText, arialFont, Brushes.Blue, firstLocation);
                graphics.DrawString(secondText, arialFont, Brushes.Red, secondLocation);
            }
        }

        bmp1.Save(imageFilePath);//save the image file
        return imageFilePath;
    }
}
