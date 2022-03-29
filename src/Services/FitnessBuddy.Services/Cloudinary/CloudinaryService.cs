namespace FitnessBuddy.Services.Cloudinary
{
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploadAsync(IFormFile file, string cloudFolder)
        {
            if (file == null)
            {
                return string.Empty;
            }

            byte[] destination;

            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                destination = ms.ToArray();
            }

            UploadResult uploadResult;

            await using (var ms = new MemoryStream(destination))
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, ms),
                    PublicId = $"fitnessbuddy/{cloudFolder}",
                    Overwrite = true,
                };

                uploadResult = await this.cloudinary.UploadAsync(uploadParams);
            }

            var secureUrl = uploadResult.Url;

            return secureUrl.OriginalString;
        }
    }
}
