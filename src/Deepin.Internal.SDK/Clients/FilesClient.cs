using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Deepin.Internal.SDK.Configuration;
using Deepin.Internal.SDK.Models;

namespace Deepin.Internal.SDK.Clients;

/// <summary>
/// Client for file-related API endpoints
/// </summary>
public interface IFilesClient
{
    Task<Stream?> DownloadFileAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FileInfoDto?> GetFileInfoAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FileInfoDto?> UploadFileAsync(UploadFileRequest request, CancellationToken cancellationToken = default);
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
    public async Task<FileInfoDto?> GetFileInfoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<FileInfoDto>($"api/v1/files/{id}", cancellationToken);
    }

    /// <summary>
    /// Downloads a file by ID
    /// </summary>
    public async Task<Stream?> DownloadFileAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogDebug("Downloading file with ID {FileId}", id);
            var response = await HttpClient.GetAsync($"api/v1/files/download/{id}", cancellationToken);

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
    /// Uploads a file with specified parameters
    /// </summary>
    public async Task<FileInfoDto?> UploadFileAsync(UploadFileRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogDebug("Uploading file {FileName}", request.FileName);

            using var content = new MultipartFormDataContent();
            using var streamContent = new StreamContent(request.FileStream);

            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(request.ContentType);
            content.Add(streamContent, "file", request.FileName);

            var response = await HttpClient.PostAsync("api/v1/files", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                Logger.LogWarning("File upload failed with status {StatusCode}: {Content}",
                    response.StatusCode, errorContent);
                throw new HttpRequestException($"File upload failed with status {response.StatusCode}", null, response.StatusCode);
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return System.Text.Json.JsonSerializer.Deserialize<FileInfoDto>(responseContent, JsonOptions);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while uploading file {FileName}", request.FileName);
            throw;
        }
    }
}
