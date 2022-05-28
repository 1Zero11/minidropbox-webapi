using Models;

namespace webapi.Data
{
    public interface IFileService
    {
        Task<int> DeleteFile(int id);
        Task<CustomFile> GetFileById(int id);
        IEnumerable<CustomFile> GetFileList();
        Task<int> UploadFile(IFormFile file);
    }
}