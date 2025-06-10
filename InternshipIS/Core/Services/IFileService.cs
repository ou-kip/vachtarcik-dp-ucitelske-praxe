using Core.Domain;
using Core.Infrastructure.Results;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
    /// <summary>
    /// The fiel service interface
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Gets the file content
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<FileDto> GetFileAsync(string fileName, string path, CancellationToken ct = default);

        /// <summary>
        /// Uploads the file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IResult> UploadAsync(string path, IFormFile file, CancellationToken ct = default);

    }
}
