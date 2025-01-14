using Microsoft.AspNetCore.Mvc;

namespace RestabookWebApp.Helpers;

public static class FileHelper
{
    public static async Task<string> FileSaverAsync(this IFormFile file, string webRootPath, string folder)
    {
        var filePath = Path.Combine(webRootPath, folder).ToLower();
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        var path = $"/{folder}/" + Guid.NewGuid() + file.FileName;
        await using FileStream fileStream = new(webRootPath + path, FileMode.Create);
        await file.CopyToAsync(fileStream);
        return path;
    }
}