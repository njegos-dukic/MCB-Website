using Core.Util;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MasterCraftBreweryAPI.Util
{
    public static class FormFileReader
    {
        public static BasicFileInfo AsBasicFileInfo(this IFormFile file)
        {
            if (file.Length < 0)
                return null;
            using Stream stream = file.OpenReadStream();
            byte[] bytes = new byte[file.Length];
            stream.Read(bytes, 0, (int)file.Length);
            return new BasicFileInfo(file.FileName, bytes);
        }
    }
}
