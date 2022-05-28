using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using webapi.Data;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IFileService _fileService;

        public FileController(ILogger<FileController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        // GET: api/File
        [HttpGet]
        public async Task<IEnumerable<CustomFile>> GetFileList()
        {

            _logger?.LogInformation("Sending filelist to " + Request.HttpContext.Connection.RemoteIpAddress.ToString());
            return _fileService.GetFileList();
        }



        // GET: api/File/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomFile(int id)
        {

            CustomFile customFile = await _fileService.GetFileById(id);
            _logger?.LogInformation("Sending file " + customFile.Name + " to " + Request.HttpContext.Connection.RemoteIpAddress.ToString());

            return File(customFile.Bytes, "text/plain", customFile.Name);
        }



        // POST: api/File
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [RequestSizeLimit(1_000_000_000)]
        public async Task<ActionResult> PostCustomFile(List<IFormFile> files)
        {
            if (!files.Any())
            {
                return Problem("No files");
            }

            foreach (var file in files)
            {
                if (file.Length <= 0)
                {
                    return Problem("File is empty");

                }

                _logger?.LogInformation("Putting file " + file.FileName + " to db from " + Request.HttpContext.Connection.RemoteIpAddress.ToString());
                await _fileService.UploadFile(file);
            }
            return Ok();
        }

        // DELETE: api/File/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomFile(int id)
        {
            _logger?.LogInformation("Deleting file with id " + id + ", sender: " + Request.HttpContext.Connection.RemoteIpAddress.ToString());
            await _fileService.DeleteFile(id);

            return Ok();
        }
    }
}
