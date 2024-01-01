using Microsoft.AspNetCore.Mvc;
using XtraBlogWebsite.Models;

namespace XtraBlogWebsite.Extensions
{
    public static class FileManager
    {
        public static async Task<string> FileUpload(this IFormFile file,string wwwroot,string folder)
        {
            string folderpath = Path.Combine(wwwroot, "uploads", folder);
            string filename = Guid.NewGuid().ToString() + "_" + file.FileName;
            string fullpath = Path.Combine(folderpath, filename);

            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            using (FileStream stream = new FileStream(fullpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filename;
        }
    }
}
