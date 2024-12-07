using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FileService
{
    private readonly string _storagePath;

    public FileService(string storagePath)
    {
        _storagePath = storagePath;
    }

    public async Task<List<string>> SaveFilesAsync(List<IFormFile> files)
    {
        var savedFileNames = new List<string>();

        if (files == null || files.Count == 0)
        {
            return savedFileNames;
        }

        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";
                var filePath = Path.Combine(_storagePath, uniqueFileName);

                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                savedFileNames.Add(uniqueFileName);
            }
        }

        return savedFileNames;
    }
}
