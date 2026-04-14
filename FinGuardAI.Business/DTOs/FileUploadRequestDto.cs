using Microsoft.AspNetCore.Http;
using System;



namespace FinGuardAI.Business.DTOs
{
    public class FileUploadRequestDto
    {
        public IFormFile File { get; set; }
    }
}
