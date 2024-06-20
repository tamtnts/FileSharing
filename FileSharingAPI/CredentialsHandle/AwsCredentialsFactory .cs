using Amazon.IdentityManagement.Model;
using Amazon.IdentityManagement;
using Amazon.Runtime;

namespace FileSharingAPI.CredentialsHandle
{
    public class AwsCredentialsFactory : IAwsCredentialsFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonIdentityManagementService _iamClient;

        public AwsCredentialsFactory(IConfiguration configuration, IAmazonIdentityManagementService iamClient)
        {
            _configuration = configuration;
            _iamClient = iamClient;
        }

        public async Task<AWSCredentials> GenerateAWSCredentialsAsync()
        {
            var userArn = "arn:aws:iam::037183097369:user/tamnguyen";

            var getUserRequest = new GetUserRequest
            {
                UserName = userArn.Split('/').Last()
            };

            var getUserResponse = await _iamClient.GetUserAsync(getUserRequest);

            var createAccessKeyRequest = new CreateAccessKeyRequest
            {
                UserName = getUserResponse.User.UserName
            };

            var createAccessKeyResponse = await _iamClient.CreateAccessKeyAsync(createAccessKeyRequest);
            var accessKey = createAccessKeyResponse.AccessKey.AccessKeyId;
            var secretKey = createAccessKeyResponse.AccessKey.SecretAccessKey;

            return new BasicAWSCredentials(accessKey, secretKey);
        }
    }
}
