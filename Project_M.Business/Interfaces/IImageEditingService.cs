using Project_M.Business.Models;
using System.Drawing;

namespace Project_M.Business.Interfaces
{
    public interface IImageEditingService
    {
        Bitmap ApplyEffects(FileModel model, string? tintColor, int? size, int? cornerRadius);
    }
}