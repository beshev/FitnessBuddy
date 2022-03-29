namespace FitnessBuddy.Services.Cloudinary
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        public Task<string> UploadAsync(IFormFile file, string fileName);
    }
}
