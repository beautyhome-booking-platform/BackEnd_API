using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Storage
{
    public class BlobStorage
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public BlobStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("AzureStorage:Key");
            _containerName = configuration.GetValue<string>("AzureStorage:ContainerName");
        }

        // Phương thức tải ảnh lên Blob Storage và trả về URL ảnh
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            // Kiểm tra file có hợp lệ không
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            // Kết nối với Blob Storage
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            // Tạo tên file duy nhất
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            // Tải file lên Blob Storage
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            // Trả về URL của file đã tải lên
            return blobClient.Uri.ToString();
        }
    }
}
