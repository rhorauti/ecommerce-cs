using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace e_commerce_cs.aws
{
  public class S3Connector(IConfiguration configuration)
  {
    private readonly string _roleArn = configuration["Aws:AssumeRole:roleArn"]
      ?? throw new ArgumentNullException(nameof(configuration), "Role Arn não encontrada.");
    private readonly string _roleSessionName = configuration["Aws:AssumeRole:roleSessionName"]
      ?? throw new ArgumentNullException(nameof(configuration), "roleSessionName não encontrada.");
    private readonly RegionEndpoint _regionEndpoint = RegionEndpoint.USEast1;

    public async Task<IAmazonS3> GetS3ClientAsync()
    {
      AssumeRole assumeRole = new();
      var credentials = await assumeRole.GetSessionCredentialsAsync(_roleArn, _roleSessionName);
      return new AmazonS3Client(credentials, _regionEndpoint);
    }

    public async Task<List<string>> ListPreSignedUrlsInFolderAsync(string bucketName, string folderPath, int urlExpirationInMinutes = 60)
    {
      IAmazonS3 s3Client = await GetS3ClientAsync();
      var urls = new List<string>();

      string prefix = folderPath ?? "";
      if (!string.IsNullOrEmpty(prefix) && !prefix.EndsWith("/"))
      {
        prefix += "/";
      }

      string continuationToken = null;
      try
      {
        do
        {
          var request = new ListObjectsV2Request
          {
            BucketName = bucketName,
            Prefix = prefix,
            ContinuationToken = continuationToken
          };

          ListObjectsV2Response response = await s3Client.ListObjectsV2Async(request);

          foreach (S3Object s3Object in response.S3Objects)
          {
            if (s3Object.Key != prefix || s3Object.Size > 0)
            {
              var urlRequest = new GetPreSignedUrlRequest
              {
                BucketName = bucketName,
                Key = s3Object.Key,
                Expires = DateTime.UtcNow.AddMinutes(urlExpirationInMinutes)
              };

              var url = s3Client.GetPreSignedURL(urlRequest);
              urls.Add(url);
            }
          }

          continuationToken = response.IsTruncated ? response.NextContinuationToken : null;

        } while (continuationToken != null);
      }
      catch (AmazonS3Exception e)
      {
        Console.WriteLine($"Erro específico do S3 ao gerar URLs: {e.Message} (Código: {e.ErrorCode})");
        throw;
      }
      catch (Exception e)
      {
        Console.WriteLine($"Erro inesperado ao gerar URLs S3: {e.Message}");
        throw;
      }

      return urls;
    }
  }
}
