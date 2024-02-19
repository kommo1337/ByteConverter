using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteConverter
{
    internal class CaptchaGenerator
    {
        private static string generatedCaptchaText;

        public static Tuple<string, Bitmap> GenerateCaptcha(int width, int height, int noise)
        {
            generatedCaptchaText = GenerateRandomText(4);
            var bitmap = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(bitmap))
            {

                graphics.Clear(Color.White);


                var font = new Font("Arial", 14);
                var brush = new SolidBrush(Color.Black);
                graphics.DrawString(generatedCaptchaText, font, brush, 10, 10);


                var rand = new Random();
                for (var i = 0; i < noise; i++)
                {

                    var x1 = rand.Next(bitmap.Width);
                    var y1 = rand.Next(bitmap.Height);
                    var x2 = rand.Next(bitmap.Width);
                    var y2 = rand.Next(bitmap.Height);


                    var penColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                    var pen = new Pen(penColor);


                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }

            }

            return new Tuple<string, Bitmap>(generatedCaptchaText, bitmap);
        }


        public static string GetGeneratedCaptchaText()
        {
            return generatedCaptchaText;
        }

        public static bool VerifyCaptcha(string userInput)
        {
            return userInput == generatedCaptchaText;
        }

        private static string GenerateRandomText(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
