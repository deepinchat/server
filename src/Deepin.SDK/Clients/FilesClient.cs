using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Deepin.SDK.Configuration;
using Deepin.SDK.Models;

namespace Deepin.SDK.Clients;

/// <summary>
/// Client for file-related API endpoints
/// </summary>
public interface IFilesClient
{
    Task<FileInfoModel?> GetFileInfoAsync(int id, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadFileAsync(int id, CancellationToken cancellationToken = default);
    Task<FileUploadResponse?> UploadFileAsync(FileUploadRequest request, CancellationToken cancellationToken = default);
    Task<FileUploadResponse?> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of the files client
/// </summary>
public class FilesClient : BaseClient, IFilesClient
{
    public FilesClient(HttpClient httpClient, IOptions<DeepinApiOptions> options, ILogger<FilesClient> logger)
        : base(httpClient, options, logger)
    {
    }

    /// <summary>
    /// Gets file information by ID
    /// </summary>
    public async Task<FileInfoModel?> GetFileInfoAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<FileInfoModel>($"api/files/{id}", cancellationToken);
    }

    /// <summary>
    /// Downloads a file by ID
    /// </summary>
    public async Task<Stream?> DownloadFileAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogDebug("Downloading file with ID {FileId}", id);
            var response = await HttpClient.GetAsync($"api/files/download/{id}", cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogWarning("File download failed with status {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadAsStreamAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while downloading file {FileId}", id);
            throw;
        }
    }

    /// <summary>
    /// Uploads a file using a FileUploadRequest
    /// </summary>
    public async Task<FileUploadResponse?> UploadFileAsync(FileUploadRequest request, CancellationToken cancellationToken = default)
    {
        return await UploadFileAsync(request.FileStream, request.FileName, request.ContentType, cancellationToken);
    }

    /// <summary>
    /// Uploads a file with specified parameters
    /// </summary>
    public async Task<FileUploadResponse?> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogDebug("Uploading file {FileName}", fileName);

            using var content = new MultipartFormDataContent();
            using var streamContent = new StreamContent(fileStream);
            
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            content.Add(streamContent, "file", fileName);

            var response = await HttpClient.PostAsync("api/files", content, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                Logger.LogWarning("File upload failed with status {StatusCode}: {Content}", 
                    response.StatusCode, errorContent);
                throw new HttpRequestException($"File upload failed with status {response.StatusCode}", null, response.StatusCode);
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return System.Text.Json.JsonSerializer.Deserialize<FileUploadResponse>(responseContent, JsonOptions);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while uploading file {FileName}", fileName);
            throw;
        }
    }
}
