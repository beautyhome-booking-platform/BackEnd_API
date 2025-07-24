using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Storage
{
    public class ClouddinaryStorage : IClouddinaryStorage
    {
        private readonly Cloudinary _cloudinary;
        public ClouddinaryStorage(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret);

            _cloudinary = new Cloudinary(account)
            {
                Api = { Secure = true }
            };
        }
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is invalid");

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                UploadPreset = "ml_default",  // Upload preset có thể không dùng authenticated nữa
                UseFilename = true,
                UniqueFilename = true,
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK || uploadResult.SecureUrl == null)
                throw new Exception($"Upload failed: {uploadResult.Error?.Message}");

            // Trả về URL ảnh public trực tiếp
            return uploadResult.SecureUrl.ToString();
        }
    }
}
