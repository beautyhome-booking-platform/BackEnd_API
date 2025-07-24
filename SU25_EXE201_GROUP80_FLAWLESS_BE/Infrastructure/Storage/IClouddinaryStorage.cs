using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Storage
{
    public interface IClouddinaryStorage
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
