using Core.Domain;
using Core.Infrastructure.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    /// <summary>
    /// The file service
    /// </summary>
    public class FileService : IFileService
    {
        private readonly ILogger _logger;
        private static string RootPath => Path.Combine(Directory.GetCurrentDirectory(), "data");

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="logger"></param>
        public FileService(ILogger<FileService> logger) 
        {
            _logger = logger;
        }

        ///<inheritdoc/>
        public async Task<FileDto> GetFileAsync(string fileName, string path, CancellationToken ct = default)
        {
            var filePath = Path.Combine(RootPath, path, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"FILE DOWNLOAD: File '{fileName}' not found in path '{path}'.", filePath);
            }

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            
            var memoryStream = new MemoryStream();            
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await fileStream.CopyToAsync(memoryStream);
            }

            memoryStream.Position = 0;

            var dto = new FileDto()
            {
                Content = memoryStream,
                ContentType = "application/octet-stream"
            };

            return dto;
        }

        ///<inheritdoc/>
        public async Task<IResult> UploadAsync(string path, IFormFile file, CancellationToken ct = default)
        {
            var result = new Result();

            if (file == null || file.Length == 0)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var folderPath = Path.Combine(RootPath, path);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, file.FileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream, ct);
                }
            }
            catch(Exception ex) 
            {
                _logger.LogError($"FILE UPLOAD: {ex.Message}");
                result.AddError(ex);
            }

            return result;
        }
    }
}
