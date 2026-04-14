using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.Business.AIAgentComponents
{
    public class Utility
    {
        public static async Task<string> ConvertingTempPathName(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{extension}");
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return tempPath;
        }
    }
}
