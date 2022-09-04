using Microsoft.AspNetCore.Mvc;
using Project_M.Business.Interfaces;
using Project_M.Business.Models;
using System.Drawing.Imaging;

namespace Project_M.Controllers
{
    [ApiController]
    [Route("api/image")]
    public class ImageController : Controller
    {
        private readonly IImageEditingService _imageEditingService;
        public ImageController(IImageEditingService imageEditingService)
        {
            _imageEditingService = imageEditingService;
        }

        /// <summary>
        /// Apply effects to the image
        /// </summary>
        /// <remarks>
        /// Color filter color , Size and Corner Radius can pass to the API as a query params. All the params are optional.
        /// Color should be in simple; ex: "red", Size and Corner Radius should be in px; ex: 100.
        /// </remarks>
        /// <response code="200">Returns file</response>
        /// <response code="404">File not found</response>
        [HttpPost("edit")]
        public IActionResult UploadFile([FromQuery]string? color,[FromQuery] int? size, [FromQuery] int? cornerRadius, [FromForm] FileModel model)
        {
            if (Request.Form.Files[0] != null)
            {
                model.File = Request.Form.Files[0];
            }
            else
            {
                return NotFound("File not found");
            }
            var img = _imageEditingService.ApplyEffects(model, color, size, cornerRadius);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            return File(ms.ToArray(), model.File.ContentType);

        }
    }
}
