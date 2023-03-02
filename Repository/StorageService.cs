using System.IO.Compression;
using System.Net;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

using board_dotnet.DTO;

namespace board_dotnet.Service;

public class StorageService : IStorageService
{
    private readonly AmazonS3Client _amazonS3Client;

    private readonly string _bucketName = "wonjun-s3";
    private readonly string _base64Key = string.Empty;

    public StorageService(IConfiguration configuration)
    {
        AmazonS3Config config = new AmazonS3Config();

        config.RegionEndpoint = RegionEndpoint.GetBySystemName(configuration["Storage:S3:Region"]);

        _amazonS3Client = new AmazonS3Client(configuration["Storage:S3:AccessKey"], configuration["Storage:S3:SecretAccessKey"], config);      
    }

    public async Task<bool> UploadFile(string path, IFormFile file)
    {
        string key = string.Format("{0}/{1}", path, file.FileName);

        // MultipartUpload Part Reponse
        List<UploadPartResponse> uploadResponses = new List<UploadPartResponse>();

        // MultipartUpload 설정 초기화
        InitiateMultipartUploadResponse initResponse = await _amazonS3Client.InitiateMultipartUploadAsync(new InitiateMultipartUploadRequest {
            BucketName = _bucketName,
            Key = key,
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        });

        Stream fileStream = file.OpenReadStream();

        // Part의 최소 크기는 5MB
        long contentLength = fileStream.Length;
        long partSize = 5 * (long)Math.Pow(2, 20); // 최소크기 5MB

        try
        {
            long filePosition = 0;

            // 파일을 chunk로 쪼개서 요청 생성
            for (int i = 1; filePosition < contentLength; i++)
            {
                UploadPartRequest uploadRequest = new UploadPartRequest {
                    BucketName = _bucketName,
                    Key = key,
                    UploadId = initResponse.UploadId,
                    PartNumber = i,
                    PartSize = partSize,
                    FilePosition = filePosition,
                    InputStream = fileStream
                };

                uploadResponses.Add(await _amazonS3Client.UploadPartAsync(uploadRequest));

                filePosition += partSize;
            }

            // 완료 요청 설정
            CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest {
                BucketName = _bucketName,
                Key = key,
                UploadId = initResponse.UploadId
            };

            // 각 part의 ETAG 
            completeRequest.AddPartETags(uploadResponses);

            // 완료 요청을 보내 Upload 마무리
            // 완료 요청을 보내지 않으면 파일이 Upload 되지 않음
            CompleteMultipartUploadResponse completeUploadResponse = await _amazonS3Client.CompleteMultipartUploadAsync(completeRequest);

            if (completeUploadResponse.HttpStatusCode == HttpStatusCode.OK) 
                return true;

            else 
                return false;
        }

        catch (Exception ex)
        {
            // 오류 발생 시 Upload 중단
            // 해당 UploadId 폐기
            AbortMultipartUploadRequest abortMPURequest = new AbortMultipartUploadRequest
            {
                BucketName = _bucketName,
                Key = key,
                UploadId = initResponse.UploadId
            };

            await _amazonS3Client.AbortMultipartUploadAsync(abortMPURequest);

            return false;
        }
    }

    public async Task<AttachFileDownloadDTO?> DownloadDicrectory(string path)
    {
        AttachFileDownloadDTO result = new();

        try
        {
            ListObjectsRequest request = new ListObjectsRequest() {
                BucketName = _bucketName,   
                Prefix = path
            };

            ListObjectsResponse response = await _amazonS3Client.ListObjectsAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.OK) 
            {
                using (MemoryStream ms = new MemoryStream())  
                {  
                    using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))  
                    {
                        foreach (S3Object obj in response.S3Objects) 
                        {
                            GetObjectRequest requestObject = new GetObjectRequest() {
                                BucketName = _bucketName,
                                Key = obj.Key
                            };

                            using (GetObjectResponse responseObject = await _amazonS3Client.GetObjectAsync(requestObject)) 
                            {
                                var entry = zip.CreateEntry(responseObject.Key.Split('/').LastOrDefault()!);

                                using (var entryStream = entry.Open())
                                {
                                    await responseObject.ResponseStream.CopyToAsync(entryStream);
                                }
                            }
                        }
                    }

                    return new AttachFileDownloadDTO() { content = ms.ToArray(), contentType = "application/octet-stream", fileName = "attachFile.zip" };
                }
            }

            return null;
        }

        // 버킷 또는 객체가 없을 경우
        catch (AmazonS3Exception)
        {
            return null;
        }

        catch (Exception)
        {
            return null;
        }
    }

    public async Task<AttachFileDownloadDTO?> DownloadFile(string filePath)
    {
        AttachFileDownloadDTO result = new();

        try
        {
            // 객체 다운로드 요청 생성
            GetObjectRequest request = new GetObjectRequest() {
                BucketName = _bucketName,
                Key = filePath
            };

            using (GetObjectResponse response = await _amazonS3Client.GetObjectAsync(request))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await response.ResponseStream.CopyToAsync(ms);

                    return new AttachFileDownloadDTO() { content = ms.ToArray(), contentType = "application/octet-stream", fileName = filePath.Split('/').LastOrDefault()! };
                }
            }
        }

        // 버킷 또는 객체가 없을 경우
        catch (AmazonS3Exception)
        {
            return null;
        }

        catch (Exception)
        {
            return null;
        }
    }
    
    public async Task DeleteDirectory(string path)
    {
        try
        {
            ListObjectsRequest request = new ListObjectsRequest() {
                BucketName = _bucketName,
                Prefix = path
            };

            ListObjectsResponse response = await _amazonS3Client.ListObjectsAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.OK) response.S3Objects.ForEach(async obj => await _amazonS3Client.DeleteObjectAsync(_bucketName, obj.Key));
        }

        catch
        {
        }
    }

    public async Task DeleteFile(string filePath)
    {
        try
        {
            // 객체 삭제
            await _amazonS3Client.DeleteObjectAsync(_bucketName, filePath);
        }

        catch 
        {
        }
    }
}