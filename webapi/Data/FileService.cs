using Models;

namespace webapi.Data;



public class FileService : IFileService
{
    private readonly FileContext _fileContext;

    public FileService(FileContext context)
    {
        _fileContext = context;
    }

    public IEnumerable<CustomFile> GetFileList()
    {
        if (_fileContext.CustomFiles == null)
        {
            return Array.Empty<CustomFile>();
        }

        return _fileContext.CustomFiles.Select(p => new CustomFile { ID = p.ID, Name = p.Name, Size = p.Size });
    }

    public async Task<int> DeleteFile(int id)
    {
        var customFile = await _fileContext.CustomFiles.FindAsync(id);

        _fileContext.CustomFiles.Remove(customFile!);
        await _fileContext.SaveChangesAsync();

        return 1;
    }

    public async Task<CustomFile> GetFileById(int id) => await _fileContext
        .CustomFiles
        .FindAsync(id);

    public async Task<int> UploadFile(IFormFile file)
    {
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        var fileBytes = ms.ToArray();
        string s = Convert.ToBase64String(fileBytes);

        CustomFile customFile = new() { Name = file.FileName, Bytes = fileBytes, Size = file.Length };

        _fileContext.CustomFiles.Add(customFile);
        await _fileContext.SaveChangesAsync();

        return 1;
    }
}