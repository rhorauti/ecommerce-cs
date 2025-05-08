using System.Text.Json;

using Amazon.Runtime;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;

namespace e_commerce_cs.aws
{
  public class AssumeRole()
  {
    private readonly AmazonSecurityTokenServiceClient _stsClient = new();

    public async Task<SessionAWSCredentials> GetSessionCredentialsAsync(string roleArn, string roleSessionName)
    {
      AssumeRoleRequest assumeRoleRequest = new()
      {
        RoleArn = roleArn,
        RoleSessionName = roleSessionName
      };
      AssumeRoleResponse assumeRoleResponse = await _stsClient.AssumeRoleAsync(assumeRoleRequest);
      Credentials credentials = assumeRoleResponse.Credentials;

      return new SessionAWSCredentials(
          credentials.AccessKeyId,
          credentials.SecretAccessKey,
          credentials.SessionToken
      );
    }
  }
}
