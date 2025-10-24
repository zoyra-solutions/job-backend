using Minio;
using Minio.DataModel.Args;
using Microsoft.Extensions.Configuration;

namespace Recruitment.Infrastructure.Services;

public class FileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public FileStorageService(IMinioClient minioClient, IConfiguration configuration)
    {
        _minioClient = minioClient;
        _bucketName = configuration["MinIO:BucketName"];
    }

    public async Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType)
    {
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName)
            .WithStreamData(fileStream)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs);
        return $"{_bucketName}/{fileName}";
    }

    public async Task<Stream> GetFileAsync(string fileName)
    {
        var memoryStream = new MemoryStream();
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream));

        await _minioClient.GetObjectAsync(getObjectArgs);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName);

        await _minioClient.RemoveObjectAsync(removeObjectArgs);
    }
}