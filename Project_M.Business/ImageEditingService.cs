using Microsoft.Extensions.Logging;
using Project_M.Business.Interfaces;
using Project_M.Business.Models;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Project_M.Business
{
    public class ImageEditingService : IImageEditingService
    {
        private readonly ILogger _logger;
        public ImageEditingService(ILogger<ImageEditingService> logger)
        {
            _logger = logger;
        }

        public Bitmap ApplyEffects(FileModel model, string? tintColor,int? size, int? cornerRadius)
        {
            try
            {
                Bitmap img = new Bitmap(model.File.OpenReadStream());
                if(tintColor != null)
                {
                    img = ApplyColorFilter(img, Color.FromName(tintColor));
                }
                if (size != null)
                {
                    img = new Bitmap(img, size ?? 0, size ?? 0);
                }
                if(cornerRadius != null)
                {
                    img = RoundCorners(img, cornerRadius??0);
                }
                return img;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            
        }

        private static Bitmap ApplyColorFilter(Bitmap img, Color tintColor)
        {
            Bitmap modifiedImg = new Bitmap(img.Width, img.Height);

            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    var oldColor = img.GetPixel(x, y);
                    byte red = (byte)(256.0f * (oldColor.R / 256.0f) * (tintColor.R / 256.0f));
                    byte blue = (byte)(256.0f * (oldColor.B / 256.0f) * (tintColor.B / 256.0f));
                    byte green = (byte)(256.0f * (oldColor.G / 256.0f) * (tintColor.G / 256.0f));

                    Color newColor = Color.FromArgb(red, blue, green);
                    modifiedImg.SetPixel(x, y, newColor);
                }
            }
            return modifiedImg;
        }

        private static Bitmap RoundCorners(Bitmap StartImage, int CornerRadius)
        {
            CornerRadius *= 2;
            Bitmap RoundedImage = new Bitmap(StartImage.Width, StartImage.Height);
            using (Graphics g = Graphics.FromImage(RoundedImage))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Brush brush = new TextureBrush(StartImage);
                GraphicsPath gp = new GraphicsPath();
                gp.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90);
                gp.AddArc(0 + RoundedImage.Width - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90);
                gp.AddArc(0 + RoundedImage.Width - CornerRadius, 0 + RoundedImage.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
                gp.AddArc(0, 0 + RoundedImage.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
                g.FillPath(brush, gp);
                return RoundedImage;
            }
        }
    }
}
